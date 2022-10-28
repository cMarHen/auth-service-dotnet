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
      this.collection = context.Collection;
    }

    public virtual async Task CreateAsync(Account account)
    {
      await collection.AddAsync(account);
      throw new NotImplementedException();
    }

    public virtual async Task<Account> GetAsync()
    {
      throw new NotImplementedException();
    }

    public virtual async Task UpdateAsync(Account account)
    {
      throw new NotImplementedException();
    }
  }
}