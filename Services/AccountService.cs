using auth_account.Models;
using auth_account.Interfaces;

namespace auth_account.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAuthRepository authRepository;
        private readonly PasswordHasher hasher;
        private readonly IJwtHandler jwtHandler;

        public AccountService(IJwtHandler jwtHandler, IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
            this.hasher = new PasswordHasher();
            this.jwtHandler = jwtHandler;
        }

        public async Task<AccountResponse> createAccount(AccountRequest req)
        {
            // TODO: Look for duplicate username
            Account user = new Account();

            user.id = Guid.NewGuid();
            user.username = req.username;
            user.email = req.email;
            user.password =
                req.password != null ? hasher.HashPassword(req.password) : throw new Exception();

            var token = this.jwtHandler.getToken(user);

            await authRepository.CreateAsync(user);

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

        public async Task<AccountResponse> login(AccountRequest req)
        {
            try
            {
                Account validAccount = await authRepository.GetAsync(req.username!);

                hasher.VerifyPassword(validAccount, validAccount.password!, req.password!);

                var token = this.jwtHandler.getToken(validAccount);

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
        public async Task patchPassword(AccountRequest req) // TODO: patchUsrname&Pw
        {
            try
            {
                String sub = this.jwtHandler.verifyToken(req).id!;
                Account validAccount = await authRepository.GetAsync(req.username!);

                // Acts as a login
                hasher.VerifyPassword(validAccount, validAccount.password!, req.oldPassword!);

                // Hash the new password
                validAccount.password = 
                req.newPassword != null ? hasher.HashPassword(req.newPassword) : throw new Exception();
                
                 await this.authRepository.UpdateAsync(validAccount);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
                throw new UnauthorizedAccessException("Fail");
            }
        }

        public AccountRequest verifyAccount(String authorizationHeader, AccountRequest req)
        {
          String[] schemeAndToken = authorizationHeader.Split(' ');

          // Verify Bearer _and_ token
          if (schemeAndToken[0].Equals("Bearer") && schemeAndToken[1].Length > 300)
          {
            req.accessToken = schemeAndToken[1];
            return jwtHandler.verifyToken(req); // Edit verifyToken to take a string
          } else 
          {
            throw new UnauthorizedAccessException();
          }
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

        Task<AccountResponse> IAccountService.getAccount(string token)
        {
            throw new NotImplementedException();
        }
  }
}