namespace auth_account.Interfaces
{
  public interface IJwtHandler
  {
    public string getToken(auth_account.Models.Account account);
    public string verifyToken(string? token);
  }
}