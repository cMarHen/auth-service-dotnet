namespace auth_account.Models
{
  public class AccountResponse
  {
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
  }  
}