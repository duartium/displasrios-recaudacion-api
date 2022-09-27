using Displasrios.Recaudacion.Core.DTOs.Reports;
using Microsoft.EntityFrameworkCore;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class DISPLASRIOSContext
    {
        public virtual DbSet<BestCustomersDto> BestCustomersResume { get; set; }
        public virtual DbSet<MostSelledProductDto> MostSelledProduct { get; set; }
    }
}
