using SMMS.Repositories.TinVT.Models;

namespace SMMS.Repositories.TinVT
{
    public interface IUnitOfWork : IDisposable
    {
        HealthCheckSessionTinVTRepository HealthCheckSessionTinVTRepository { get;}
        HealthCheckStudentTinVtRepository HealthCheckStudentTinVTRepository { get;}
        UserAccountRepository UserAccountRepository { get; }

        int SaveChangesWithTransaction();
        Task<int> SaveChangesWithTransactionAsync();
        
    }
}
