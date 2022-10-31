using auth_account.Models;
using auth_account.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace auth_account.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAccountService accountService;

        public AuthController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> getStartPoint(AccountRequest req) {
          // Verify JWT
          try
          {
            // accountService.verifyAccount(req.accessToken);

          return Ok();
        
          }
          catch (System.Exception)
          {
            return Unauthorized();
          }  
        }

        /* [Authorize] */
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login(AccountRequest req)
        {
          try
          {
            AccountResponse res = await accountService.login(req);

            return Ok(res);
          }
          catch (System.Exception e)
          {
            System.Console.WriteLine("Sysout rad 34 i /AuthController, login()");
            System.Console.WriteLine(e);
            return Unauthorized();
          }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> register(AccountRequest req) {
          try
          {
            var res = await accountService.createAccount(req);

            return Ok(res);
          }
          catch (System.Exception e)
          {
            System.Console.WriteLine("Sysout rad 345i /AuthController, register()");
            System.Console.WriteLine(e);
            return Unauthorized();
          }
        }

        [HttpPatch]
        [Route("editpw")]
        public async Task<IActionResult> patchPassword(AccountRequest user) {
          try
          {
            await accountService.patchPassword(user);
            return Ok();
          }
          catch (System.Exception)
          {
            return Unauthorized("Failed to edit credentials.");
          }
        }

        // Use this to authenticate user
        // Refresh token if needed
        // [Authorize]
        [Route("authenticate")]
        [HttpPost]
        public async Task<IActionResult> authenticate(AccountRequest req)
        {
          try
          {
            // Verify Authorization header
            if (!Request.Headers.ContainsKey("Authorization"))
            {
              throw new Exception();
            }

            accountService.verifyAccount(Request.Headers["Authorization"].ToString(), req);
            
            return Ok("Autentisierad");
          }
          catch (System.Exception e)
          {
            System.Console.WriteLine(e);
            return Unauthorized();
          }
        }
    }
}
