using auth_account.Interfaces;
using auth_account.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_account.Repositories
{
  public class AuthRepository : IAuthRepository
  {
    private readonly AuthServiceDbContext context;
    private readonly DbSet<Account> collection;

    public AuthRepository(AuthServiceDbContext context) 
    {
      this.context = context;
      this.collection = context.Accounts;
    }

    public virtual async Task CreateAsync(Account account)
    {
      await context.AddAsync(account);
      await context.SaveChangesAsync();
    }

    public virtual async Task<Account> GetAsync(string username)
    {
      Account? account = await this.collection.FirstOrDefaultAsync<Account>(
        acc => acc.username == username
      );

      if (account != null) {
        return account;
      } else {
        throw new Exception("Could not find a member with this username");
      }
    }

    public virtual async Task UpdateAsync(Account updatedAccount)
    {
      Account accountFromDb = await collection.SingleAsync(acc => acc.id == updatedAccount.id);
      accountFromDb.password = updatedAccount.password;

      await context.SaveChangesAsync();
    }
  }
}