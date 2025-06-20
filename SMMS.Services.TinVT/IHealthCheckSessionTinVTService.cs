using SMMS.Repositories.TinVT.Models;

namespace SMMS.Services.TinVT
{
    public interface IHealthCheckSessionTinVTService
    {
        Task<List<HealthCheckSessionTinVt>> GetAllAsync();
        Task<HealthCheckSessionTinVt> GetByIdAsync(Guid sessionId);
        Task<HealthCheckSessionTinVt> GetByCodeAsync(string sessionCode);
        Task<HealthCheckSessionTinVt> CreateAsync(HealthCheckSessionTinVt session);
        Task<bool> UpdateAsync(HealthCheckSessionTinVt session);
        Task<bool> DeleteAsync(Guid sessionId);
        Task<List<HealthCheckSessionTinVt>> GetUpcomingSessionsAsync();
        Task<List<HealthCheckSessionTinVt>> SearchAsync(string? sessionCode, string? title, string? studentId);

    }
}