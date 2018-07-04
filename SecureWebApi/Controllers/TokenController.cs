using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SecureWebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SecureWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    public class TokenController : Controller
    {
        private IConfiguration config;

        public TokenController(IConfiguration config)
        {
            this.config = config;
        }

        public IActionResult RequestToken([FromBody]UserModel user)
        {
            
            if(user.UserName== "JK" && user.Password=="Believe me! Its secure password")
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(config["Token:Issuer"],
                  config["Token:Issuer"],
                  expires: DateTime.Now.AddMinutes(30),
                  signingCredentials: creds);

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return BadRequest("Could not authenticate you!");
        }
    }
}