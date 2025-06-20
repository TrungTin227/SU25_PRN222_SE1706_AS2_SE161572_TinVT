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
            return await _unitOfWork.HealthCheckSessionTinVTRepository.GetByCodeAsync(sessionCode);
        }

        public async Task<HealthCheckSessionTinVt> CreateAsync(HealthCheckSessionTinVt session)
        {
            // Validation đơn giản
            if (string.IsNullOrEmpty(session.SessionCode))
                throw new ArgumentException("Session Code không được để trống");

            if (string.IsNullOrEmpty(session.Title))
                throw new ArgumentException("Title không được để trống");

            // Tự động sinh ID và set thông tin
            session.HealthCheckSessionTinVtid = Guid.NewGuid();
            session.CreatedBy = "TrungTin227"; // Current user
            session.IsNotificationSent = false;
            session.IsResultSent = false;
            session.TotalStudentsChecked = 0;

            return await _unitOfWork.HealthCheckSessionTinVTRepository.CreateSessionAsync(session);
        }

        public async Task<bool> UpdateAsync(HealthCheckSessionTinVt session)
        {
            // Validation đơn giản
            if (string.IsNullOrEmpty(session.SessionCode))
                throw new ArgumentException("Session Code không được để trống");

            if (string.IsNullOrEmpty(session.Title))
                throw new ArgumentException("Title không được để trống");

            // Set thông tin cập nhật
            session.UpdatedBy = "TrungTin227"; // Current user

            return await _unitOfWork.HealthCheckSessionTinVTRepository.UpdateSessionAsync(session);
        }

        public async Task<bool> DeleteAsync(Guid sessionId)
        {
            return await _unitOfWork.HealthCheckSessionTinVTRepository.DeleteSessionAsync(sessionId);
        }

        public async Task<List<HealthCheckSessionTinVt>> GetUpcomingSessionsAsync()
        {
            return await _unitOfWork.HealthCheckSessionTinVTRepository.GetUpcomingSessionsAsync();
        }

        // Implement search method đơn giản
        public async Task<List<HealthCheckSessionTinVt>> SearchAsync(string? sessionCode, string? title, string? studentId)
        {
            var allSessions = await GetAllAsync();

            var filteredSessions = allSessions.Where(session =>
            {
                // Lọc theo Session Code (bảng chính)
                bool matchSessionCode = string.IsNullOrWhiteSpace(sessionCode) ||
                    (session.SessionCode?.Contains(sessionCode, StringComparison.OrdinalIgnoreCase) == true);

                // Lọc theo Title (bảng chính)
                bool matchTitle = string.IsNullOrWhiteSpace(title) ||
                    (session.Title?.Contains(title, StringComparison.OrdinalIgnoreCase) == true);

                // Lọc theo Student ID (bảng phụ)
                bool matchStudentId = string.IsNullOrWhiteSpace(studentId) ||
                    (session.HealthCheckStudentTinVts?.Any(student =>
                        student.HealthCheckStudentTinVtid.ToString().Contains(studentId, StringComparison.OrdinalIgnoreCase)) == true);

                return matchSessionCode && matchTitle && matchStudentId;
            }).ToList();

            return filteredSessions
                .OrderByDescending(s => s.CreatedAt ?? DateTime.MinValue)
                .ToList();
        }
    }
}