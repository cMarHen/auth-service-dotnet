using System.Text;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using auth_account.Models;
using auth_account.Interfaces;

namespace auth_account.Services
{
    public class AccountService : IAccountService
    {
        private readonly AuthServiceDbContext authDbContext;
        private readonly PasswordHasher hasher;
        private readonly IConfiguration configuration;
        private readonly JwtSecurityTokenHandler tokenHandler;

        public AccountService(IConfiguration configuration, AuthServiceDbContext context)
        {
            this.authDbContext = context;
            this.configuration = configuration;
            this.hasher = new PasswordHasher();
            this.tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<AccountResponse> createAccount(AccountRequest req)
        {
            // TODO: Look for duplicate username
            Account user = new Account();

            user.id = Guid.NewGuid();
            user.username = req.username;
            user.password =
                req.password != null ? hasher.HashPassword(req.password) : throw new Exception();

            var token = getToken(user);

            await authDbContext.AddAsync(user);
            await authDbContext.SaveChangesAsync();

            if (user != null)
            {
                AccountResponse res = new AccountResponse();
                res.AccessToken = user.username; // TODO: Populate with jwt
                return res;
            }
            else
            {
                throw new Exception("Failed to register user");
            }
        }

        public async Task<AccountResponse> login(AccountRequest account)
        {
            try
            {
                System.Console.WriteLine("-----------------");
                System.Console.WriteLine(account.username);
                Account validAccount = await authDbContext.Users.FirstOrDefaultAsync(
                    x => x.username == account.username
                );

                hasher.VerifyPassword(validAccount, validAccount.password, account.password);

                var token = getToken(validAccount);

                // Populate response
                AccountResponse res = new AccountResponse();
                res.AccessToken = token;
                res.RefreshToken = "token"; // TODO
                res.ExpiresIn = 0; // TODO

                return res;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
                throw new UnauthorizedAccessException("Username or password is wrong");
            }
        }

        public void verifyAccount(string token)
        {
          string username = this.verifyToken(token);
          System.Console.WriteLine("TILLBAKA I VERIFY USER");
          System.Console.WriteLine(username);
        }

        public Task<Account> getAccount(string token)
        {
            throw new NotImplementedException();
        }

        public void logout(string token)
        {
            throw new NotImplementedException();
        }

        public void refreshToken(string token)
        {
            throw new NotImplementedException();
        }

        private string getToken(Account account)
        {
            var authClaims = new List<Claim>
            {
                new Claim("username", account.username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("sub", account.id.ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT:Secret"])
            );

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    authSigningKey,
                    SecurityAlgorithms.HmacSha256
                )
            );

            return this.tokenHandler.WriteToken(token);
        }

        // Verify the token itself, not the account.
        // Maybe this should be done as a middleware and populate the request object with user?
        private string verifyToken(string token)
        {
          try
          {
            TokenValidationParameters valParams = new TokenValidationParameters 
            {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidIssuer = configuration["JWT:ValidIssuer"],
              ValidAudience = configuration["JWT:ValidAudience"],
              IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
            };

            // TODO: Get the username from token

            ClaimsPrincipal principal = this.tokenHandler.ValidateToken(token, valParams, out SecurityToken validatedToken);
            System.Console.WriteLine("Hejhej");
            
            
            System.Console.WriteLine(principal.Claims.ToString());

            return "hejhej";
          }
          catch (System.Exception e)
          {
            System.Console.WriteLine(e);
            throw new Exception("Error in validate token");
          }
        }

        Task<AccountResponse> IAccountService.getAccount(string token)
        {
            throw new NotImplementedException();
        }
    }
}