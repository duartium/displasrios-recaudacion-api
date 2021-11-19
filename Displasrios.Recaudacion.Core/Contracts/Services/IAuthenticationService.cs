using Displasrios.Recaudacion.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Displasrios.Recaudacion.Core.Contracts
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated(UserLogin request, out string token);
    }
}
