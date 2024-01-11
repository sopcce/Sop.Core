using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sop.Core.Authorize
{
    public class JwtTokenAuthorize
    {
        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenVm"></param>
        /// <returns></returns>
        public static string CreateToken(JwtTokenVm tokenVm)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();

            var jweIssuer = ConfigurationManager.GetAppSettings().JwtToken.Issuer;
           
            var key = Encoding.ASCII.GetBytes(jweIssuer);
            var sub = new ClaimsIdentity();
            sub.AddClaim(new Claim(ClaimTypes.Name, tokenVm.UserName, ""));
            sub.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", tokenVm.Role)));  
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = sub,
                Expires = tokenVm.Expires.ToUniversalTime(),
                IssuedAt = DateTime.Now.ToUniversalTime(),
                Issuer = tokenVm.UserName,
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedJwt = tokenHandler.WriteToken(token);

            return encodedJwt;

        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static JwtTokenVm ReadToken(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            var role = new List<string>();
            var value = new object();
            var sdasda = jwtToken.Payload.TryGetValue(ClaimTypes.Role, out value); 
            var tm = new JwtTokenVm
            {
                UserName = jwtToken.Id,
                Role = new string[] { },
            };
            return tm;
        }
    }
}
