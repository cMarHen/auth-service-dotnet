using auth_account.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace auth_account.Repositories
{
  public class AuthServiceDbContext : DbContext
  {
    public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> context)
      : base(context) {
    }
    public DbSet<Models.Account> Accounts { get; set; }
  }
}