using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Entities;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Core.Contracts
{
    public interface IUserRepository
    {
        User GetByAuth(string username, string password);
        IEnumerable<UserDto> GetAll();
        UserDto Get(int id);
    }
}
