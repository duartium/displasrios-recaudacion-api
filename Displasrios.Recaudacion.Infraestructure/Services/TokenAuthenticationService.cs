using Displasrios.Recaudacion.Core.Contracts;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Core.Models.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Displasrios.Recaudacion.Infraestructure.Services
{
    public class TokenAuthenticationService : IAuthenticationService
    {
        private readonly TokenManagement _tokenManagement;
        private readonly IUserService _userService;


        public TokenAuthenticationService(IOptions<TokenManagement> tokenManagement,  IUserService userService)
        {
            _tokenManagement = tokenManagement.Value;
            _userService = userService;
        }

        public bool IsAuthenticated(UserLogin request, out string token)
        {
            token = string.Empty;
            var user = _userService.GetByAuth(request);
            if (user == null)
                return false;

            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                new Claim(ClaimTypes.Role, user.ProfileId)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_tokenManagement.Issuer, _tokenManagement.Audience, claims, expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration), signingCredentials: credentials);

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return true;
        }

        

    }
}
