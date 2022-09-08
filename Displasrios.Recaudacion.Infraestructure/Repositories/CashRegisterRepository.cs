using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.Enums;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Displasrios.Recaudacion.Infraestructure.Repositories
{
    public class CashRegisterRepository : ICashRegisterRepository
    {
        private readonly DISPLASRIOSContext _context;

        public CashRegisterRepository(DISPLASRIOSContext context)
        {
            _context = context;
        }

        public bool Close(decimal value, string observations, int idUsuario)
        {
            var cashRegister = new MovimientosCaja
            {
                Fecha = DateTime.Now,
                MontoRecibido = value,
                Observaciones = observations,
                TipoMovimiento = (int)CashMovement.CIERRE,
                UsuarioId = idUsuario,
            };

            _context.MovimientosCaja.Add(cashRegister);
            int resp = _context.SaveChanges();
            return resp > 0;
        }

        public bool IsOpenendCash()
        {
            var cashRegister = _context.MovimientosCaja.Where(x => x.Fecha.Date == DateTime.Now.Date
                                && x.TipoMovimiento == (int)CashMovement.APERTURA).FirstOrDefault();

            return cashRegister != null;
        }

        public bool Open(decimal initialValue, int idUsuario)
        {
            var cashRegister = new MovimientosCaja
            {
                Fecha = DateTime.Now,
                MontoRecibido = initialValue,
                TipoMovimiento = (int)CashMovement.APERTURA,
                UsuarioId = idUsuario,
            };

            _context.MovimientosCaja.Add(cashRegister);
            int resp = _context.SaveChanges();
            return resp > 0;
        }
    }
}
