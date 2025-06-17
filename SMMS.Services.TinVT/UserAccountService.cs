using SMMS.Repositories.TinVT;
using SMMS.Repositories.TinVT.Models;

namespace SMMS.Services.TinVT
{
    public class UserAccountService
    {
        private readonly UserAccountRepository _userAccountRepository;
        public UserAccountService() => _userAccountRepository ??= new UserAccountRepository();
        public async Task<UserAccount> GetUserAccount(string userName, string password)
        {
            return await _userAccountRepository.GetUserAccount(userName, password);
        }

    }
}
