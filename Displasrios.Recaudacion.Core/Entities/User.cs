using System;
using System.Collections.Generic;
using System.Text;

namespace Displasrios.Recaudacion.Core.Entities
{
    public class User
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string CreatedAt { get; set; }
        public string ProfileId { get; set; }
    }
}
