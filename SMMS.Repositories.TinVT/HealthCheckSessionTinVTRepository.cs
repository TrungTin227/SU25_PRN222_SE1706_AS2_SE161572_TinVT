using Microsoft.EntityFrameworkCore;
using SMMS.Repositories.TinVT.Base;
using SMMS.Repositories.TinVT.Models;

namespace SMMS.Repositories.TinVT
{
    public class HealthCheckSessionTinVTRepository : GenericRepository<HealthCheckSessionTinVt>
    {
        public HealthCheckSessionTinVTRepository()
        {
        }

        public HealthCheckSessionTinVTRepository(SU25_PRN222_SE1706_G1_SMMSContext context)
        {
            _context = context;
        }

        // Read operations
        public async Task<List<HealthCheckSessionTinVt>> GetAllAsync()
        {
            return await _context.HealthCheckSessionTinVts
                .Include(t => t.HealthCheckStudentTinVts)
                .ToListAsync();
        }

        public async Task<HealthCheckSessionTinVt> GetByIdAsync(Guid sessionId)
        {
            return await _context.HealthCheckSessionTinVts
                .Include(t => t.HealthCheckStudentTinVts)
                .FirstOrDefaultAsync(s => s.HealthCheckSessionTinVtid == sessionId);
        }

        // Đổi tên method này từ GetBySessionCodeAsync -> GetByCodeAsync
        public async Task<HealthCheckSessionTinVt> GetByCodeAsync(string sessionCode)
        {
            return await _context.HealthCheckSessionTinVts
                .Include(t => t.HealthCheckStudentTinVts)
                .FirstOrDefaultAsync(s => s.SessionCode == sessionCode);
        }

        // Create operation
        public async Task<HealthCheckSessionTinVt> CreateSessionAsync(HealthCheckSessionTinVt session)
        {
            session.CreatedAt = DateTime.UtcNow;
            await _context.HealthCheckSessionTinVts.AddAsync(session);
            await SaveAsync();
            return session;
        }

        // Update operation
        public async Task<bool> UpdateSessionAsync(HealthCheckSessionTinVt session)
        {
            try
            {
                var existingEntity = await _context.HealthCheckSessionTinVts
                    .FindAsync(session.HealthCheckSessionTinVtid);

                if (existingEntity == null)
                {
                    // Log: Entity not found
                    Console.WriteLine($"Entity not found: {session.HealthCheckSessionTinVtid}");
                    return false;
                }

                _context.Entry(existingEntity).CurrentValues.SetValues(session);
                existingEntity.UpdatedAt = DateTime.UtcNow;
                _context.Entry(existingEntity).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync();
                Console.WriteLine($"Update result: {result} rows affected");
                return result > 0;
            }
            catch (Exception ex)
            {
                // Log the actual exception
                Console.WriteLine($"Update failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        // Delete operation
        public async Task<bool> DeleteSessionAsync(Guid sessionId)
        {
            var session = await GetByIdAsync(sessionId);
            if (session == null)
                return false;

            _context.HealthCheckSessionTinVts.Remove(session);
            await SaveAsync();
            return true;
        }

        // Additional helper methods
        public async Task<List<HealthCheckSessionTinVt>> GetUpcomingSessionsAsync()
        {
            return await _context.HealthCheckSessionTinVts
                .Include(t => t.HealthCheckStudentTinVts)
                .Where(s => s.ScheduledCheckDate > DateTime.Now)
                .OrderBy(s => s.ScheduledCheckDate)
                .ToListAsync();
        }

        public async Task<bool> UpdateSessionStatusAsync(Guid sessionId, bool isNotificationSent, bool isResultSent)
        {
            var session = await GetByIdAsync(sessionId);
            if (session == null)
                return false;

            session.IsNotificationSent = isNotificationSent;
            session.IsResultSent = isResultSent;
            session.UpdatedAt = DateTime.UtcNow;

            await SaveAsync();
            return true;
        }
    }
}