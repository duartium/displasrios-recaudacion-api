using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.DTOs;
using Displasrios.Recaudacion.Core.Enums;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Microsoft.EntityFrameworkCore;
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

        public TotalCashCloseDto GetTotalsForCashClose()
        {
            var totals = new TotalCashCloseDto();

            totals.TotalSellersIncome = _context.Ingresos.Where(x => x.Estado == 1 && x.Fecha.Date == DateTime.Now.Date 
                    && x.Usuario.PerfilId == (int)Perfil.Recaudador)
               .Include(fac => fac.Usuario)
               .Select(x => x.Valor).Sum();

            totals.TotalLocal = _context.Ingresos.Where(x => x.Estado == 1 && x.Fecha.Date == DateTime.Now.Date 
                && x.Usuario.PerfilId == (int)Perfil.Administrador)
               .Include(fac => fac.Usuario)
               .Select(x => x.Valor).Sum();

            totals.TotalEgresos = _context.Entradas.Where(x => x.Estado && x.FechaEmision.Date == DateTime.Now.Date)
                                  .Select(x => x.TotalCompra).Sum();

            totals.TotalExpenses = 0; //TODO: Obtener gastos del día

            return totals;
        }


    }
}
