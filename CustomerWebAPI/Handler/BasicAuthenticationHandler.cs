using CustomerDataAccess;
using CustomerWebAPI.DataContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CustomerWebAPI.Handler
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly CustomerDbContext _context;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder  encoder,
            ISystemClock clock,
            CustomerDbContext dbContext
            ) : base (options,logger,encoder, clock)
        {

            _context = dbContext;
        }
         
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Headers.ContainsKey("UserName") && Request.Headers.ContainsKey("Password"))
            { 

            try
                {               
                 
                string emailID = Request.Headers["UserName"];
                string pwd = Request.Headers["Password"];
                Users user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == emailID && x.Password == pwd);
                if (user == null)
                {
                        return AuthenticateResult.Fail("Invalid USerNAme or password");
                }
                else
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.UserName) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                 return   AuthenticateResult.Success(ticket);
                }
                }
                catch (Exception ex)
                {
                    AuthenticateResult.Fail("Invalid UserName or Password");
                }
            }
            return AuthenticateResult.Fail(""); 
        }
    }
}
