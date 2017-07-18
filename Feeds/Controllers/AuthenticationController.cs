using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Feeds.Middleware.DataModels;
using Feeds.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Feeds.DAL;

namespace Feeds.Controllers
{
    [Route("api/AuthenticationController")]
    public class AuthenticationController: Controller
    {
        private FeedDbContext _db;

        [HttpPost]
        [RouteAttribute("token")]
        public  string Token([FromBody] dynamic authData)
        {
             var username = (String)authData.username;
             var password = (String)authData.password;

            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                _db.Users.Add(new User { Login = username, Password = password });
                _db.SaveChanges();
                identity = GetIdentity(username, password);
                // Response.StatusCode = 400;
                // return "Invalid username or password";
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: TokenProviderOptions.ISSUER,
                    audience: TokenProviderOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(TokenProviderOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(TokenProviderOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });

        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = _db.Users.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
        public AuthenticationController(FeedDbContext db)
        {
            _db = db;
        }
    }
}
