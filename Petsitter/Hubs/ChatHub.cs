using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Petsitter.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string fromUser, string message)
        {
            var userName = Context.User.Identity.Name;
            await Clients.All.SendAsync("ReceiveMessage", userName, message);
        }
    }
}
