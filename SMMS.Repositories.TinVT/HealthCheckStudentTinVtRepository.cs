using SMMS.Repositories.TinVT.Base;
using SMMS.Repositories.TinVT.Models;

namespace SMMS.Repositories.TinVT
{
    public class HealthCheckStudentTinVtRepository : GenericRepository<HealthCheckStudentTinVt>
    {
        public HealthCheckStudentTinVtRepository() { }
        public HealthCheckStudentTinVtRepository(SU25_PRN222_SE1706_G1_SMMSContext context) : base(context)
        {
        }
    }
}
