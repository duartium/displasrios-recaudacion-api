using Displasrios.Recaudacion.Core.Contracts;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Models;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
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
            return _context.Empleados.Where(x => x.Estado == 1)
                .Include(user => user.Usuario).Where(x => x.Estado == 1)
                .Select(x => new UserDto{ 
                    Id = x.Usuario.IdUsuario,
                    Username = x.Usuario.Usuario,
                    CreatedAt = x.Usuario.CreadoEn.ToString("dd-MM-yyyy"),
                    FullName = x.Nombres + " " + x.Apellidos,
                    Email = x.Email,
                    Identification = x.Identificacion,
                    RoleId = x.Usuario.Perfil.IdPerfil,
                    RoleName = x.Usuario.Perfil.Nombre,
                }).OrderBy(x => x.FullName).ToList();
        }

        public IEnumerable<ItemCatalogueDto> GetUserProfiles()
        {
            return _context.Perfiles.Where(x => x.Estado)
                .Select(x => new ItemCatalogueDto
                {
                    Id = x.IdPerfil,
                    Description = x.Nombre
                }).OrderBy(x => x.Description).ToList();
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
                    CodigoVerificacion = user.CodeEmailVerification,
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

        public bool Exists(string username, string identification, out string message)
        {
            message = String.Empty;
            int count = -1;

            count = (from client in _context.Clientes
                     where client.Estado
                     && client.Identificacion == identification
                     select client).Count();

            if (count > 0) {
                message = $"El usuario {username} ya está en uso, por favor cambialo para continuar con el registro.";
                return true;
            };

            count = (from user in _context.Usuarios
                     where user.Estado
                     && user.Usuario == username
                     select user).Count();
            
            if (count > 0)
            {
                message = $"Ya existe un usuario con identificación {identification}.";
                return true;
            };

            return false;
        }

        public bool Remove(int idUser)
        {
            var user = _context.Usuarios.Where(x => x.IdUsuario == idUser && x.Estado).First();
            user.Estado = false;
            _context.Update(user);
            int resp = _context.SaveChanges();
            return resp > 0;
        }
    }
}
