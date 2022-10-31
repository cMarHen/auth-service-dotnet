using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using auth_account.Interfaces;
using auth_account.Models;
using Microsoft.IdentityModel.Tokens;

namespace auth_account.Services
{
    public class JwtHandler : IJwtHandler
    {
        private readonly IConfiguration config;
        private readonly JwtSecurityTokenHandler tokenHandler;

        public JwtHandler(IConfiguration config)
        {
            this.config = config;
            this.tokenHandler = new JwtSecurityTokenHandler();
        }

        public string getToken(Account account)
        {
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, account.username!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, account.id.ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["JWT:Secret"])
            );

            var token = new JwtSecurityToken(
                issuer: config["JWT:ValidIssuer"],
                audience: config["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    authSigningKey,
                    SecurityAlgorithms.HmacSha256
                )
            );

            return this.tokenHandler.WriteToken(token);
        }

        public AccountRequest verifyToken(AccountRequest req)
        {
            try
            {
                TokenValidationParameters valParams = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = config["JWT:ValidIssuer"],
                    ValidAudience = config["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["JWT:Secret"])
                    )
                };

                ClaimsPrincipal principal = this.tokenHandler.ValidateToken(
                    req.accessToken,
                    valParams,
                    out SecurityToken validatedToken
                );
                
                String sub = principal.FindFirstValue(ClaimTypes.NameIdentifier);
                String usernameFromToken = principal.FindFirstValue(ClaimTypes.Name);

                // Verify guid and username
                if (sub.Length != 36 || req.username != usernameFromToken) {
                    throw new Exception("Username from body and token is not the same");
                }

                req.id = sub;
                req.accessToken = req.accessToken; // TODO: Refresh token if needed, check exptime and so on

                return req;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
                throw new Exception("Error in validate token");
            }
        }
    }
}
