using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface IAuditLogService
    {
        Task LogActionAsync(
            int? userId,
            string userName,
            string actionName,
            string requestData,
            string? responseData,
            int durationMs,
            bool success,
            string? errorMessage = null,
            string? ipAddress = null,
            string? userAgent = null);

        Task<IEnumerable<AuditLog>> GetAuditLogsByUserAsync(int userId, int pageNumber = 1, int pageSize = 50);
        Task<IEnumerable<AuditLog>> GetAuditLogsByActionAsync(string actionName, int pageNumber = 1, int pageSize = 50);
        Task<IEnumerable<AuditLog>> GetFailedActionsAsync(DateTime? from = null, DateTime? to = null);
        Task<AuditLog?> GetAuditLogByIdAsync(int id);
    }
} 