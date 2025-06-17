using Microsoft.EntityFrameworkCore;
using SMMS.Repositories.TinVT.Models;

namespace SMMS.Repositories.TinVT.Base
{
    public class GenericRepository<T> where T : class
    {
        protected SU25_PRN222_SE1706_G1_SMMSContext _context;

        public GenericRepository()
        {
            _context ??= new SU25_PRN222_SE1706_G1_SMMSContext();
        }

        public GenericRepository(SU25_PRN222_SE1706_G1_SMMSContext context)
        {
            _context = context;
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public void Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task<int> CreateAsync(T entity)
        {
            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }
        public void Update(T entity)
        {
            //// Turning off Tracking for UpdateAsync in Entity Framework
            _context.ChangeTracker.Clear();
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            //// Turning off Tracking for UpdateAsync in Entity Framework
            _context.ChangeTracker.Clear();
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            return await _context.SaveChangesAsync();

        }

        public async Task<bool> UpdateAsync(HealthCheckSessionTinVt model)
        {
            // Make sure you're not losing changes during update
            var existingEntity = await _context.HealthCheckSessionTinVts.FindAsync(model.HealthCheckSessionTinVtid);
            if (existingEntity == null) return false;
            
            // Update all properties
            _context.Entry(existingEntity).CurrentValues.SetValues(model);
            
            // If you need to preserve relationships that aren't being updated
            // _context.Entry(existingEntity).Collection(x => x.HealthCheckStudentTinVts).IsModified = false;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public bool Remove(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T GetById(string code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T> GetByIdAsync(string code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

        /*
        https://guidgenerator.com/
        daacb4fb-ff73-46ef-98f1-4af9aab2a30a
         */
        public T GetById(Guid code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T> GetByIdAsync(Guid code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

        #region Separating asigned entity and save operators        

        public void PrepareCreate(T entity)
        {
            _context.Add(entity);
        }

        public void PrepareUpdate(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
        }

        public void PrepareRemove(T entity)
        {
            _context.Remove(entity);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion Separating asign entity and save operators
    }
}

