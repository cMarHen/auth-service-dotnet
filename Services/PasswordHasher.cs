using auth_account.Interfaces;
using auth_account.Models;

namespace auth_account.Services
{
  public class PasswordHasher : IPasswordHasher
  {
    public string HashPassword(string inputPassword)
    {
      throw new NotImplementedException();
    }

    public void VerifyPassword(Account user, string hashedPassword, string providedPassword)
    {
      throw new NotImplementedException();
    }
  }
}