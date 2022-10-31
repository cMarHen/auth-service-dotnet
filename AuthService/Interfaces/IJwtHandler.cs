using auth_account.Models;

namespace auth_account.Interfaces
{
  public interface IJwtHandler
  {
    public string getToken(Account account);
    public AccountRequest verifyToken(AccountRequest req);
  }
}