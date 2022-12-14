using auth_account.Models;

namespace auth_account.Interfaces
{
  public interface IAccountService
  {
    Task<AccountResponse> login(AccountRequest req);
    Task<AccountResponse> createAccount(AccountRequest req);
    Task<AccountResponse> getAccount(string token);
    Task patchPassword(AccountRequest req);
    AccountRequest verifyAccount(String authorizationHeader, AccountRequest req);
    void logout(string token);
    void refreshToken(string token);
  }
}