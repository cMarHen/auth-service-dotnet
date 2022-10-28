using auth_account.Models;

namespace auth_account.Interfaces
{
  public interface IAuthRepository
  {
    Task<Account> GetAsync(string username);
    Task CreateAsync(Account account);
    Task UpdateAsync(Account account);
  }
}