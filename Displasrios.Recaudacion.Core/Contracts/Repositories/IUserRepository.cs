using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Entities;
using Displasrios.Recaudacion.Core.Models;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Core.Contracts
{
    public interface IUserRepository
    {
        UserEntity GetByAuth(string username, string password);
        IEnumerable<UserDto> GetAll();
        UserDto Get(int id);
        bool Create(UserCreation user);
        bool Remove(int idUser);
        IEnumerable<ItemCatalogueDto> GetUserProfiles();
    }
}
