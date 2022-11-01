namespace auth_account.Models
{
  public class Account
  {
    public Guid id { get; set; }
    public String? username { get; set; }
    public String? password { get; set; }
    public String? email { get; set; }

    public Account()
    {
    }

    public Account(string username, string password)
    {
      this.username = username;
      this.password = password;
    }
  }
}