using Microsoft.EntityFrameworkCore;
using SMMS.Repositories.TinVT.Base;
using SMMS.Repositories.TinVT.Models;

namespace SMMS.Repositories.TinVT
{
    public class UserAccountRepository : GenericRepository<UserAccount>
    {
        public UserAccountRepository() { }

        public UserAccountRepository(SU25_PRN222_SE1706_G1_SMMSContext context) => _context = context;

        public async Task<UserAccount> GetUserAccount(string userName, string password)
        {
            return await _context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password && u.IsActive == true) ?? new UserAccount();
        }   
    }
}
