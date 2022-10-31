namespace auth_account.Interfaces
{
  public interface IPasswordHasher
  {
    string HashPassword(string inputPassword);
    void VerifyPassword(Models.Account user, string hashedPassword, string providedPassword);
  }
}