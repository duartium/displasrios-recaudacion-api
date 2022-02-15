using Displasrios.Recaudacion.Core.Contracts;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Entities;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using System.Collections.Generic;
using System.Linq;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DISPLASRIOSContext _context;
        public UserRepository(DISPLASRIOSContext context)
        {
            _context = context;
        }

        public User GetByAuth(string username, string password)
        {
            return new User { IdUser = 213, Username = "Byron Duarte", CreatedAt = "2021/11/19" };
        }

        public IEnumerable<UserDto> GetAll()
        {
            return _context.Usuarios.Where(x => x.Estado == true)
                .Select(x => new UserDto{ 
                    Id = x.IdUsuario,
                    Username = x.Usuario,
                    CreatedAt = x.CreadoEn.ToString("dd-MM-yyyy")
                }).ToList();
        }

        public UserDto Get(int id)
        {
            return _context.Usuarios.Where(x => x.Estado && x.IdUsuario == id)
                .Select(x => new UserDto
                {
                    Id = x.IdUsuario,
                    Username = x.Usuario,
                    CreatedAt = x.CreadoEn.ToString("dd-MM-yyyy")
                }).FirstOrDefault();
        }
    }
}
