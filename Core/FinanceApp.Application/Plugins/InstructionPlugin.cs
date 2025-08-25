using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Plugins
{
    using Microsoft.SemanticKernel;
    using System.ComponentModel;
    using FinanceApp.Application.Interfaces;
    using FinanceApp.Application.Interfaces.Services;
    using FinanceApp.Application.Features.Rules;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;
    using FinanceApp.Application.Hubs;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
    using Newtonsoft.Json.Linq;
    using FinanceApp.Application.DTOs;
    using System.Text.Json;
    using FinanceApp.Application.Features.Results.MembershipResult;
    using FinanceApp.Application.Features.Results.InvestmentPlanResults;

    public class InstructionPlugin(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        [KernelFunction("get_most_recent_instruction_info")]
        [Description("Kullanıcının tarih olarak en yakın olan instruction bilgilerini döndürür, En yakın tarihli talimatınının Adını, Ödeme tarihini , Fiyatını, ödenip ödenmediğini getirir eğer true ise ödenmiş false ise ödenmemiştir. string tokenı AI serviceden alır")]
        [return: Description("en yakın tarihli Instruction  bilgilerini döndürür")]
        public async Task<string> GetMostRecentInstructionTitleAsync(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.finstats.net/api/Instruction/GetNameMostRecentInstruction");
            request.Headers.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return $"Hata: {response.StatusCode}";

            return await response.Content.ReadAsStringAsync();
        }

        [KernelFunction("get_most_expensive_expense_info")]
        [Description("Kullanıcının yapmış olduğu harcamların en pahalı olanının bilgilerini döndürür, En pahalı harcamanın, Adını , Ödeme tarihini , Fiyatını bilgi olarak içerir , bu harcama olduğu için zaten ödenmiş olan harcamaları uygulamama ekliyorum. string tokenı AI serviceden alır")]
        [return: Description("Kullanıcının yapmış olduğu en pahalı harcamnın  bilgilerini döndürür")]
        public async Task<string> TheMostExpensiveExpenseByUserAsync(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.finstats.net/api/Expense/TheMostExpensiveExpenseByUser");
            request.Headers.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return $"Hata: {response.StatusCode}";

            return await response.Content.ReadAsStringAsync();
        }

        [KernelFunction("get_daily_expense_list_info")]
        [Description("Kullanıcının gider bilgilerini datetime göre döner, tarihe göre filtrelediği giderlerin, Tarihini , Toplam o gün harcanan fiyatı ve o gün kaçtane harcama yaptığı bilgilerini döner, bu harcama olduğu için zaten ödenmiş olan harcamaları uygulamama ekliyorum. string tokenı AI serviceden alır")]
        [return: Description("Kullanıcının günlük giderlerini tarihe göre filtreleyerek getirir.")]
        public async Task<IList<DailyIncomeExpenseDto>> DailyExpenseListByUserAsync(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.finstats.net/api/Expense/GetDailyExpenseList");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Hata: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<IList<DailyIncomeExpenseDto>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new List<DailyIncomeExpenseDto>();
        }

        [KernelFunction("get_all_memberships_by_user")]
        [Description("Kullanıcının bütün abonelikleri ile ilgili bilgileri içeren bir liste döner. Listede aboneliklerin Adı, Başlangıç tarihi , Bitiş tarihi, Abonelik Plan adı ve IsDeletedi yani true ise plan iptal edilmiş bitiş tarihine kadar kullanabilirsin false ise her ay otomatik yenileyecek kendini yani iptal edilmemiş. string tokenı AI serviceden alır")]
        [return: Description("Kullanıcının Aboneliklerinin bilgilerini getirir.")]
        public async Task<IList<GetAllMembershipsByUserQueryResult>> GetAllMembershipsByUser(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.finstats.net/api/Membership/GetAllMembershipsByUser");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Hata: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<IList<GetAllMembershipsByUserQueryResult>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new List<GetAllMembershipsByUserQueryResult>();
        }


        [KernelFunction("get_all_investmentplan_by_user")]
        [Description("Kullanıcıya ait yatırım planlarını liste halinde getirir.'Name' adı , 'Description' açıklaması , 'TargetPrice' hedef biriktirceğin parayı gösterir, 'CurrentAmount' şuana kadar biriktirdiğin parayı gösterir , 'TargetDate' ne zamana kadar tamamlaman gerektiğini gösterir , 'IsCompleted' true ise tamamlanmış false ise daha tamamlanmamıştır. 'InvestmentCategory' yatırım planımın kategorisini temsil ediyor 'InvestmentFrequency' yatırım planına ne kadar sıklıkla para atmam gerektiğini gösteriyor 1 se her gün para atmak lazım 2 ise haftada 1 atmak lazım 3 ise ayda 1 para atmak lazım 'PerPaymentAmount' bu sıkılıkta ne kadar para atmam gerektiğini gösteriyor 'HowManyDaysLeft' buda target date' a kaç gün kaldığını gösteriyor. string tokenı AI serviceden alır")]
        [return: Description("Kullanıcının Yatırım Planlarına dair bilgileri getirir.")]
        public async Task<IList<GetAllInvestmenPlanByUserQueryResult>> GetAllInvestmenPlanByUser(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.finstats.net/api/InvestmentPlan/GetAllInvestmentPlanByUser");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Hata: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<IList<GetAllInvestmenPlanByUserQueryResult>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new List<GetAllInvestmenPlanByUserQueryResult>();
        }


        [KernelFunction("get_monthly_profitorloss_by_user")]
        [Description("Kullanıcının Son 1 aydaki kar zarar durumunu getirir. 'Income' son 1 aydaki gelen para 'Expens' son 1 aydaki harcanan para 'ProfitLoss' ise  kar zarar durumu +yasa profit loss karda - ise zararda bu son geçtiğimiz bir ayın kar zarar durumunu gösteriyor. .string tokenı AI serviceden alır")]
        [return: Description("Kullanıcının gelir , gider ve kar zarar bilgileri getirir.")]
        public async Task<MonthlyProfitLossDto> MonthlyProfitLossByUser(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.finstats.net/api/Expense/MonthlyProfitLossByUser");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Hata: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MonthlyProfitLossDto>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new MonthlyProfitLossDto();
        }


        [KernelFunction("get_monthly_expense_report_mail")]
        [Description("Kullanıcının son 1 ayda yaptığı harcamaları Gider , Harcama raporu olarak kullanıcıya maille gönderir.")]
        [return: Description("Kullanıcının Harcama raporunu kullanıcının mailine gönderir. Eğer kullanıcı Harcama raporu ya da Gider Raporu derse bu endpoint çalışsın")]
        public async Task<string> MonthlyExpenseReport(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.finstats.net/api/Expense/MonthlyExpenseReport");
            request.Headers.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return $"Hata: {response.StatusCode}";

            return await response.Content.ReadAsStringAsync();
        }



        [KernelFunction("get_nextmonth_instruction_report_mail")]
        [Description("Kullanıcının önümüzdeki 1 aydaki talimatlarını kullanıcıya maille gönderir.")]
        [return: Description("Kullanıcının Talimat raporunu kullanıcının mailine gönderir. Eğer kullanıcı Talimat raporu derse bu endpoint çalışsın")]
        public async Task<string> NextMonthInstructionReport(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.finstats.net/api/Instruction/NextMonthInstructionReport");
            request.Headers.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return $"Hata: {response.StatusCode}";

            return await response.Content.ReadAsStringAsync();
        }
    }

}

