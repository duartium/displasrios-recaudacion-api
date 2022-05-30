using Displasrios.Recaudacion.Core.Contracts;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using System;
using System.Collections.Generic;
using System.Linq;
using UserEntity = Displasrios.Recaudacion.Core.Entities.UserEntity;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DISPLASRIOSContext _context;
        public UserRepository(DISPLASRIOSContext context)
        {
            _context = context;
        }

        public UserEntity GetByAuth(string username, string password)
        {
            return _context.Usuarios.Where(x => x.Estado && x.Usuario.Equals(username) && x.Clave.Equals(password))
                .Select(x => new UserEntity { 
                    IdUser = x.IdUsuario,
                    Username = x.Usuario,
                    ProfileId = x.PerfilId.ToString(),
                    CreatedAt = x.CreadoEn.ToString("dd-MM-yyyy")
                }).FirstOrDefault();
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
                    RoleId = x.PerfilId,
                    CreatedAt = x.CreadoEn.ToString("dd-MM-yyyy")
                }).FirstOrDefault();
        }

        public bool Create(UserCreation user)
        {
            int rowAffected = 0;

            using (var db = _context.Database.BeginTransaction())
            {
                var _user = new Usuarios { 
                    Usuario = user.Username,
                    Clave = user.Password,
                    PerfilId = user.Type,
                    Estado = true,
                    CreadoEn = DateTime.Now,
                    UsuarioCrea = user.CurrentUser
                };
                _context.Usuarios.Add(_user);
                _context.SaveChanges();
                int idUser = _user.IdUsuario;

                var employee = new Empleados
                {
                    Identificacion = user.Identification,
                    Nombres = user.Names,
                    Apellidos = user.Surnames,
                    Email = user.Email,
                    TipoEmpleado = user.Type,
                    Estado = 1,
                    CreadoEn = DateTime.Now,
                    UsuarioCrea = user.CurrentUser,
                    UsuarioId = idUser
                };
                _context.Empleados.Add(employee);
                rowAffected = _context.SaveChanges();
                

                _context.Database.CommitTransaction();
            }

            return (rowAffected > 0);
        }

    }
}
