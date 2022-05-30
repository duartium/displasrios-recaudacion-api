using Displasrios.Recaudacion.Core.Models;

namespace Displasrios.Recaudacion.Core.Contracts
{
    public interface IUserService
    {
        Entities.UserEntity GetByAuth(UserLogin req);
    }
}
