using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Displasrios.Recaudacion.WebApi.Hubs
{
    public class OrderHub : Hub
    {
        public Task NotificarTodos(string mensaje)
        {
            return Clients.All.SendAsync("orderentry", mensaje);
        }
    }
}
