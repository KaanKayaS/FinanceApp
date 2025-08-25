# 💰 FinStats Backend

<div align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8.0" />
  <img src="https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white" alt="SQL Server" />
  <img src="https://img.shields.io/badge/OpenAI-412991?style=for-the-badge&logo=openai&logoColor=white" alt="OpenAI" />
  <img src="https://img.shields.io/badge/SignalR-512BD4?style=for-the-badge&logo=signalr&logoColor=white" alt="SignalR" />
</div>

https://finstats.net

## 📖 Proje Hakkında

FinanceApp, modern web teknolojileri kullanılarak geliştirilmiş kapsamlı bir kişisel finans yönetim platformunun backend API'sidir. Clean Architecture prensiplerine dayalı olarak tasarlanmış, ölçeklenebilir ve güvenli bir RESTful API sağlar.

## 🚀 Özellikler

### 💼 Finansal Yönetim
- 💰 Gelir ve gider takibi
- 💳 Kredi kartı yönetimi
- 📊 Finansal raporlama ve analitik
- 🔄 Abonelik yönetimi
- 📈 Bütçe planlama

### 🤖 AI Entegrasyonu
- 💬 Real-time AI finansal danışmanlık
- 🧮 Akıllı hesaplama pluginleri
- 📝 Yapay zeka destekli öneriler
- 🔄 SignalR ile canlı chat

### 🔐 Güvenlik ve Kimlik Doğrulama
- 🔑 JWT tabanlı authentication
- 👤 ASP.NET Core Identity
- 🛡️ Rate limiting
- 🌐 CORS politikaları

### 📧 Bildirim ve Raporlama
- 📧 SendGrid ile e-posta gönderimi
- 📄 PDF rapor oluşturma
- ⏰ Zamanlanmış görevler (Hangfire)
- 📊 Otomatik raporlama

## 🏗️ Teknoloji Stack'i

### Core Framework
- **.NET 8.0** - Modern C# development platform
- **ASP.NET Core Web API** - RESTful API framework

### Veritabanı
- **Microsoft SQL Server** - Primary database
- **Entity Framework Core 8.0.17** - ORM
- **ASP.NET Core Identity** - User management

### AI ve Machine Learning
- **Microsoft Semantic Kernel 1.61.0** - AI orchestration
- **OpenAI GPT-3.5 Turbo** - AI chat integration
- **Custom AI Plugins** - Calculator & Instruction plugins

### Real-time Communication
- **SignalR** - Real-time web functionality
- **WebSocket** - Bidirectional communication

### Background Processing
- **Hangfire** - Background job processing
- **SQL Server Storage** - Job persistence

### Validation ve Mapping
- **FluentValidation 12.0.0** - Model validation
- **AutoMapper 12.0.0** - Object-to-object mapping

### Patterns ve Architecture
- **MediatR 12.5.0** - CQRS and Mediator pattern
- **Clean Architecture** - Layered architecture
- **Repository Pattern** - Data access abstraction
- **Unit of Work** - Transaction management

### Testing
- **xUnit** - Unit testing framework
- **Moq 4.20.72** - Mocking library
- **FluentAssertions 8.5.0** - Test assertions

### Logging ve Monitoring
- **Serilog** - Structured logging
- **SQL Server Sink** - Database logging

### Utilities
- **SendGrid 9.29.3** - Email service
- **QuestPDF 2025.7.0** - PDF generation
- **Swagger/OpenAPI** - API documentation

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler
- .NET 8.0 SDK
- SQL Server (LocalDB veya Express)
- Visual Studio 2022 / VS Code
- SendGrid API Key (email için)
- OpenAI API Key (AI features için)

### Kurulum Adımları

1. **Repository'yi klonlayın**
```bash
git clone https://github.com/yourusername/FinanceApp.git
cd FinanceApp
```

2. **Bağımlılıkları yükleyin**
```bash
dotnet restore
```

3. **Veritabanı bağlantı ayarları**
`appsettings.json` dosyasında connection string'i güncelleyin:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FinanceAppDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

4. **Migration'ları çalıştırın**
```bash
dotnet ef database update --project FinanceApp/Infrastructure/FinanceApp.Persistence --startup-project FinanceApp/Presentation/FinanceApp.Api
```

5. **API Keys ayarlayın**
`appsettings.json` dosyasına gerekli API key'leri ekleyin:
```json
{
  "SendGrid": {
    "ApiKey": "your-sendgrid-api-key"
  },
  "OpenAI": {
    "ApiKey": "your-openai-api-key"
  }
}
```

6. **Uygulamayı çalıştırın**
```bash
dotnet run --project FinanceApp/Presentation/FinanceApp.Api
```

API şu adreste çalışacaktır: `https://localhost:5001`

## 📚 API Dokümantasyonu

Swagger UI: `https://localhost:5001/swagger`

### Ana Endpoint'ler

#### Authentication
- `POST /api/auth/login` - Kullanıcı girişi
- `POST /api/auth/register` - Kullanıcı kaydı
- `POST /api/auth/refresh-token` - Token yenileme

#### Finans Yönetimi
- `GET /api/expenses` - Gider listesi
- `POST /api/expenses` - Yeni gider ekleme
- `GET /api/incomes` - Gelir listesi
- `POST /api/incomes` - Yeni gelir ekleme

#### AI Chat
- `POST /chat` - AI ile sohbet
- SignalR Hub: `/ai-hub` - Real-time chat

#### Raporlama
- `GET /api/reports/monthly` - Aylık rapor
- `GET /api/reports/pdf` - PDF rapor oluştur

## 🏛️ Proje Yapısı

```
FinanceApp/
├── Core/
│   ├── FinanceApp.Domain/          # Domain entities ve business logic
│   └── FinanceApp.Application/     # Application services ve use cases
├── Infrastructure/
│   ├── FinanceApp.Infrastructure/  # External services (JWT, Email)
│   └── FinanceApp.Persistence/     # Data access ve repositories
├── Presentation/
│   └── FinanceApp.Api/            # Web API controllers
└── FinanceApp.Tests/              # Unit ve integration testler
```

## 🧪 Test Çalıştırma

```bash
# Tüm testleri çalıştır
dotnet test

# Coverage raporu ile
dotnet test --collect:"XPlat Code Coverage"
```

## 🔧 Geliştirme

### Code Style
- C# Coding Conventions
- Clean Code principles
- SOLID principles

### Yeni Migration Ekleme
```bash
dotnet ef migrations add MigrationName --project FinanceApp/Infrastructure/FinanceApp.Persistence --startup-project FinanceApp/Presentation/FinanceApp.Api
```

### Background Jobs
Hangfire dashboard: `https://localhost:5001/hangfire`

## 🚀 Production Deployment

### Environment Variables
```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=your-production-db-connection
SendGrid__ApiKey=your-sendgrid-key
OpenAI__ApiKey=your-openai-key
```


## 👨‍💻 Geliştirici

**Kaan Kaya**
- GitHub: [@KaanKayaS](https://github.com/KaanKayaS)
- LinkedIn: [LinkedIn](https://www.linkedin.com/in/kaankaya07)


<div align="center">
  Made with ❤️ by Kaan Kaya
</div>
