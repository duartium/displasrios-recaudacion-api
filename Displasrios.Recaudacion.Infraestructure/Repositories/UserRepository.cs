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
            UserEntity userEntity = null;
            var user = _context.Usuarios.Where(x => x.Estado && x.Usuario.Equals(username.Trim()) && x.Clave.Equals(password))
                .FirstOrDefault();
                

            if(user != null && user.VerificadoEn == null)
            {
                user.VerificadoEn = DateTime.Now;
                _context.Update(user);
                _context.SaveChanges();
            }

            if(user != null)
            {
                userEntity = new UserEntity
                 {
                     IdUser = user.IdUsuario,
                     Username = user.Usuario,
                     ProfileId = user.PerfilId.ToString(),
                     CreatedAt = user.CreadoEn.ToString("dd-MM-yyyy"),
                     VerifiedAt = user.VerificadoEn.ToString()
                 };
            }

            return userEntity;
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

        public IEnumerable<CollectorResumeDto> GetCollectors()
        {
            //retorna los vendedores que aún no han entregado sus ingresos del día para el arqueo de caja
            var collectorsResume = _context.Empleados.Where(x => x.Estado == 1 && x.TipoEmpleado == 1)
                .Include(user => user.Usuario).Where(x => x.Estado == 1)
                .Select(x => new CollectorResumeDto
                {
                    IdUser = x.Usuario.IdUsuario,
                    FullUsername = x.Usuario.Usuario + " - " + x.Nombres + " " + x.Apellidos
                }).OrderBy(x => x.FullUsername).ToList();

            int[] collectorsConArqueoIds = _context.Ingresos.Where(x => x.Fecha.Date == DateTime.Now.Date)
                .Select(x => x.UsuarioId).ToArray();

            IEnumerable<CollectorResumeDto> collectorsFiltered = collectorsResume.Where(x => !collectorsConArqueoIds.Contains(x.IdUser)).ToList();
            return collectorsFiltered;
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
            return _context.Empleados.Where(emp => emp.Estado == 1 && emp.Usuario.IdUsuario == id)
                .Include(user => user.Usuario)
                .Select(x => new UserDto
                {
                    Id = x.Usuario.IdUsuario,
                    Username = x.Usuario.Usuario,
                    RoleId = x.Usuario.PerfilId,
                    FullName = x.Nombres + " " + x.Apellidos,
                    CreatedAt = x.Usuario.CreadoEn.ToString("dd-MM-yyyy")
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

        public bool Exists(string email)
        {
            var employee = _context.Empleados.Where(x => x.Email == email && x.Estado == 1).FirstOrDefault();
            return employee != null;
        }

        public bool Remove(int idUser)
        {
            var user = _context.Usuarios.Where(x => x.IdUsuario == idUser && x.Estado).First();
            user.Estado = false;
            _context.Update(user);
            int resp = _context.SaveChanges();
            return resp > 0;
        }

        public string GenerateUsername(string names, string surnames)
        {
            string nameAbreviation = names.Trim().ToLower().Substring(0, 1);
            surnames = surnames.TrimStart().TrimEnd().ToLower();

            string username = nameAbreviation;
            
            if (surnames.Contains(" "))
                surnames = surnames.Split(' ').GetValue(0).ToString();

            username += surnames;

            var user = _context.Usuarios.Where(x => x.Estado && x.Usuario == username).FirstOrDefault();
            if(user != null)//ya existe usuario, se vuelve a generar otro
            {
                //verifico si el último caracter es un número para incrementarlo caso contrario le agrego un 1
                char lastChar = username.Substring(username.Length - 1, 1).ToCharArray()[0];
                if (Char.IsNumber(lastChar))
                {
                    int number = int.Parse(lastChar.ToString());
                    number = number++;
                    username = username.Remove(username.Length - 1, 1).Insert(username.Length - 1, number.ToString());
                }
                else
                    username += "1";
            }

            return username;
        }

        public bool RegisterVerificationCode(string email, string code)
        {
            int idEmployee = _context.Empleados.Where(x => x.Email == email && x.Estado == 1)
                .Select(x => x.IdEmpleado).FirstOrDefault();

            var _passwordReset = new PasswordReset
            {
                Email = email,
                CodigoVerificacion = code,
                Fecha = DateTime.Now,
                EmpleadoId = idEmployee
            };
            _context.PasswordReset.Add(_passwordReset);
            var resp = _context.SaveChanges();
            return resp > 0;
        }

        public VerifyCodeResponse VerifyCode(string email, string code)
        {
            var response = new VerifyCodeResponse {
                IsSuccess = true,
                Message = "OK"
            };

            var rowPassword = _context.PasswordReset.Where(x => x.Email == email)
                .ToList().OrderByDescending(x => x.Fecha).FirstOrDefault();

            if (rowPassword.CodigoVerificacion != code) {
                response.IsSuccess = false;
                response.Message = "El código de verificación es incorrecto";
                return response;
            }

            rowPassword.ActualizadoEn = DateTime.Now;
            _context.Entry(rowPassword).State = EntityState.Modified;
            int resp = _context.SaveChanges();

            if (resp <= 0)
            {
                response.IsSuccess = false;
                response.Message = "Error en la verificación de código";
            }

            return response;
        }

        public ChangePasswordResponse ChangePassword(string email, string newPassword)
        {
            var response = new ChangePasswordResponse
            {
                IsSuccess = true,
                Message = "OK"
            };

            var rowUserEmployee = (from users in _context.Usuarios
                                  join empl in _context.Empleados
                                    on users.IdUsuario equals empl.UsuarioId
                                  where empl.Email == email
                                  select new { users, empl }).FirstOrDefault();
;

            if (rowUserEmployee == null)
            {
                response.IsSuccess = false;
                response.Message = $"No se encontró un usuario con el correo {email}";
                return response;
            }

            var user = rowUserEmployee.users;
            user.ModificadoEn = DateTime.Now;
            user.UsuarioMod = "reset_password";
            user.Clave = newPassword;

            _context.Entry(user).State = EntityState.Modified;
            int resp = _context.SaveChanges();

            if (resp <= 0)
            {
                response.IsSuccess = false;
                response.Message = "Error en al actualizar la contraseña";
            }

            return response;
        }

    }
}
