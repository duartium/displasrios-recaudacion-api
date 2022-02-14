using System;
using System.Collections.Generic;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class Parametros
    {
        public int IdParametro { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Estado { get; set; }
    }
}
