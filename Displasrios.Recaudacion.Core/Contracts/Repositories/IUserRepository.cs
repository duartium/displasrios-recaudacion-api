using Displasrios.Recaudacion.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Displasrios.Recaudacion.Core.Contracts
{
    public interface IUserRepository
    {
        User GetByAuth(string username, string password);
    }
}
