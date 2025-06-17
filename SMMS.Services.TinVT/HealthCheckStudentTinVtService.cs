using SMMS.Repositories.TinVT;
using SMMS.Repositories.TinVT.Models;

namespace SMMS.Services.TinVT
{
    public class HealthCheckStudentTinVtService
    {
        private readonly HealthCheckStudentTinVtRepository _repository;

        public HealthCheckStudentTinVtService(HealthCheckStudentTinVtRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<HealthCheckStudentTinVt>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
