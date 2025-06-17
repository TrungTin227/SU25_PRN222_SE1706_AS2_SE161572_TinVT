using SMMS.Repositories.TinVT.Models;

namespace SMMS.Repositories.TinVT
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SU25_PRN222_SE1706_G1_SMMSContext _context;
        public HealthCheckSessionTinVTRepository HealthCheckSessionTinVTRepository { get; }
        public HealthCheckStudentTinVtRepository HealthCheckStudentTinVTRepository { get; }
        public UserAccountRepository UserAccountRepository { get; }
        //public UnitOfWork(SU25_PRN222_SE1706_G1_SMMSContext context) => _context = context;

        public UnitOfWork(SU25_PRN222_SE1706_G1_SMMSContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Khởi tạo các repository ngay lập tức
            HealthCheckSessionTinVTRepository = new HealthCheckSessionTinVTRepository(_context);
            HealthCheckStudentTinVTRepository = new HealthCheckStudentTinVtRepository(_context);
            UserAccountRepository = new UserAccountRepository(_context);
        }

        public HealthCheckSessionTinVTRepository GetHealthCheckSessionTinVTRepository() => HealthCheckSessionTinVTRepository;

        public HealthCheckStudentTinVtRepository GetHealthCheckStudentTinVTRepository() => HealthCheckStudentTinVTRepository;


        public void Dispose()
        {
            _context.Dispose();
        }
        public int SaveChangesWithTransaction()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }
    }
}
