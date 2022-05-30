using Displasrios.Recaudacion.Core.Contracts;
using Displasrios.Recaudacion.Core.Entities;
using Displasrios.Recaudacion.Core.Models;

namespace Displasrios.Recaudacion.Infraestructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _rpsUser;
        public UserService(IUserRepository userRepository)
        {
            _rpsUser = userRepository;
        }
        public Core.Entities.UserEntity GetByAuth(UserLogin req)
        {
            return _rpsUser.GetByAuth(req.Username, req.Password);
        }

    }
}
