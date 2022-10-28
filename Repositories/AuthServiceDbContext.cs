using Microsoft.EntityFrameworkCore;

namespace auth_account.Repositories
{
  public class AuthServiceDbContext : DbContext
  {
    public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options): base(options) {}
  
    public DbSet<Models.Account> Accounts { get; set; }
  }
}