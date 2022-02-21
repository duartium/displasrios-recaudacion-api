using Displasrios.Recaudacion.Core.Models;

namespace Displasrios.Recaudacion.Core.Contracts
{
    public interface IUserService
    {
        Entities.User GetByAuth(UserLogin req);
    }
}
