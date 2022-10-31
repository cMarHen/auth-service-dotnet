using auth_account.Interfaces;
using auth_account.Models;
using BCrypt.Net;
using BC = BCrypt.Net.BCrypt;

namespace auth_account.Services
{
  public class PasswordHasher : IPasswordHasher
  {
    public string HashPassword(string inputPassword)
    {
      return BC.EnhancedHashPassword(inputPassword, hashType: HashType.SHA384);
    }

    public void VerifyPassword(Account user, string hashedPassword, string providedPassword)
    {
      try
      {
        var isVerified = BC.EnhancedVerify(providedPassword, hashedPassword, hashType: HashType.SHA384);

        if (user == null || !isVerified)
        {
          throw new Exception("Username or password is incorrect");
        }
      }
      catch (System.Exception e)
      {
        System.Console.WriteLine(e);
        throw new Exception("Failed to verify user");
      }
    }
  }
}