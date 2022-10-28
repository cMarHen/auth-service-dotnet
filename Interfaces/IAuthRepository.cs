using auth_account.Models;

namespace auth_account.Interfaces
{
  public interface IAuthRepository
  {
    Task<Account> GetAsync();
    Task CreateAsync(Account account);
    Task UpdateAsync(Account account);
  }
}