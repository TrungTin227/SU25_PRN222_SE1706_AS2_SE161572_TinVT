using SMMS.Repositories.TinVT;
using SMMS.Repositories.TinVT.Models;

namespace SMMS.Services.TinVT
{
    public class HealthCheckSessionTinVTService : IHealthCheckSessionTinVTService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HealthCheckSessionTinVTService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HealthCheckSessionTinVt>> GetAllAsync()
        {
            return await _unitOfWork.HealthCheckSessionTinVTRepository.GetAllAsync();
        }

        public async Task<HealthCheckSessionTinVt> GetByIdAsync(Guid sessionId)
        {
            return await _unitOfWork.HealthCheckSessionTinVTRepository.GetByIdAsync(sessionId);
        }

        public async Task<HealthCheckSessionTinVt> GetByCodeAsync(string sessionCode)
        {
            return await _unitOfWork.HealthCheckSessionTinVTRepository.GetBySessionCodeAsync(sessionCode);
        }

        public async Task<HealthCheckSessionTinVt> CreateAsync(HealthCheckSessionTinVt session)
        {
            return await _unitOfWork.HealthCheckSessionTinVTRepository.CreateSessionAsync(session);
        }

        public async Task<bool> UpdateAsync(HealthCheckSessionTinVt session)
        {
            return await _unitOfWork.HealthCheckSessionTinVTRepository.UpdateSessionAsync(session);
        }

        public async Task<bool> DeleteAsync(Guid sessionId)
        {
            return await _unitOfWork.HealthCheckSessionTinVTRepository.DeleteSessionAsync(sessionId);
        }
    }
}
