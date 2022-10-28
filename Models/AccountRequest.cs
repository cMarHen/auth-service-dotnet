namespace auth_account.Models
{
  public class AccountRequest
  {
    public string? username { get; set; }
    public string? password { get; set; }
    public string? email { get; set; }
    public string? oldPassword { get; set; }
    public string? newPassword { get; set; }
    public string? accessToken { get; set; }
    public string? refreshToken { get; set; }
  }
}