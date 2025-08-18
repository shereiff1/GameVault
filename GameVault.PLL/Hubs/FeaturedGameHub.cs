
using Microsoft.AspNetCore.SignalR;

namespace GameVault.PLL.Hubs
{
    public class FeaturedGameHub : Hub
    {
        public async Task SendFeaturedGame(object dto)
        {
            await Clients.All.SendAsync("UpdateFeaturedGame", dto);
        }
    }
}
