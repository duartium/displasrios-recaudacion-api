using Displasrios.Recaudacion.Core.Contracts;
using Displasrios.Recaudacion.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Displasrios.Recaudacion.Infraestructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _rpsUser;
        public UserService(IUserRepository userRepository)
        {
            _rpsUser = userRepository;
        }
        public bool IsValid(UserLogin req)
        {
            var user =_rpsUser.GetByAuth(req.Username, req.Password);
            return true;
        }
    }
}
