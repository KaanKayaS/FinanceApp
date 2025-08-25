using FinanceApp.Application;
using FinanceApp.Application.DTOs;
using FinanceApp.Application.Exceptions;
using FinanceApp.Application.Hubs;
using FinanceApp.Application.Interfaces.Hangfire;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Plugins;
using FinanceApp.Infrastructure;
using FinanceApp.Persistence;
using FinanceApp.Persistence.AI;
using FinanceApp.Persistence.Data;
using Hangfire;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.SemanticKernel;
using OpenAI;
using QuestPDF.Infrastructure;
using System.ClientModel;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var openAiSettings = builder.Configuration.GetSection("OpenAI");


builder.Services
    .AddKernel()
    .AddOpenAIChatCompletion(
             modelId: "openai/gpt-3.5-turbo",
             openAIClient: new OpenAIClient(
                 credential: new ApiKeyCredential(openAiSettings["ApiKey"]),
                 options: new OpenAIClientOptions
                 {
                     Endpoint = new Uri(openAiSettings["Endpoint"])
                 })
).Plugins.AddFromType<CalculatorPlugin>()
         .AddFromType<InstructionPlugin>();




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var env = builder.Environment;


builder.Configuration
    .SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("https://finstats.net")  // Angular uygulamamızın URL'i
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials()
                   .SetIsOriginAllowed(s => true);
        });
});


builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("ForgotPassword", limiterOptions =>
    {
        limiterOptions.PermitLimit = 200; 
        limiterOptions.Window = TimeSpan.FromMinutes(15);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});


builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("AILimiter", limiterOptions =>
    {
        limiterOptions.PermitLimit = 200;
        limiterOptions.Window = TimeSpan.FromMinutes(15);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});



builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHangfireServer();

builder.Services.AddHttpClient();
builder.Services.AddSignalR();
//builder.Services.AddSingleton<AIService>();
builder.Services.AddSingleton<HistoryService>();
builder.Services.AddMemoryCache(); // memory cache




builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;

    await SeedData.SeedUsersAsync(services);
    
    // Sistem ayarlarını başlat
    var systemSettingsService = services.GetRequiredService<ISystemSettingsService>();
    await systemSettingsService.InitializeDefaultSettingsAsync();
}

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");


//app.MapPost("/chat", async (AIService aiService, ChatRequestVM chatRequest, CancellationToken cancellationToken)
//    => await aiService.GetMessageStreamAsync(chatRequest.Prompt, chatRequest.ConnectionId, cancellationToken));



app.MapPost("/chat", async (ChatRequestVM chatRequest, CancellationToken cancellationToken, IServiceProvider serviceProvider) =>
{
    try
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var aiService = scope.ServiceProvider.GetRequiredService<AIService>();
        
        await aiService.GetMessageStreamAsync(chatRequest.Prompt, chatRequest.ConnectionId, cancellationToken);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        Console.WriteLine("AIService HATASI:");
        Console.WriteLine(ex.ToString());

        if (ex is ClientResultException clientEx && clientEx.Message is not null)
        {
            Console.WriteLine("DETAYLI HATA: " + clientEx.Message);
        }

        return Results.Problem("AIService HATASI: " + ex.Message);
    }
})
.RequireRateLimiting("AILimiter");


app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization(); 

app.ConfigureExceptionHandlingMiddleware();


app.UseHangfireDashboard("/hangfire"); // http://localhost:5055/hangfire gibi çalışır


RecurringJob.AddOrUpdate<IMembershipRenewalService>(
    "membership-renewal-job",
    x => x.RenewMembershipsAsync(),
    Cron.Daily()
);

app.MapHub<AIHub>("/ai-hub");

app.MapControllers();

app.Run();
