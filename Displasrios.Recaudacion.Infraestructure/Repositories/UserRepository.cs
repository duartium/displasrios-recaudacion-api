using Displasrios.Recaudacion.Core.Contracts;
using Displasrios.Recaudacion.Core.Entities;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User GetByAuth(string username, string password)
        {
            return new User { IdUser = 213, Username = "Byron Duarte", CreatedAt = "2021/11/19" };
        }
    }
}
