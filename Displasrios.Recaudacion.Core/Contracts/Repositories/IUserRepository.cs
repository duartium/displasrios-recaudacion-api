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
        IEnumerable<CollectorResumeDto> GetCollectors();
        UserDto Get(int id);
        bool Create(UserCreation user);
        bool Remove(int idUser);
        IEnumerable<ItemCatalogueDto> GetUserProfiles();
        string GenerateUsername(string names, string surnames);
        bool Exists(string email);
        bool RegisterVerificationCode(string email, string code);
        VerifyCodeResponse VerifyCode(string email, string code);
        ChangePasswordResponse ChangePassword(string email, string code);
    }
}
