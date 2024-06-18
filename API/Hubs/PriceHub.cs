using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class PriceHub : Hub
    {
        public async Task Subscribe(string instrument)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, instrument);
            //Use SignalR groups to manage subscribers for different financial instruments. This allows for efficient broadcasting to specific groups of clients.
        }

        public async Task Unsubscribe(string instrument)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, instrument);
        }

        public async Task SendPriceUpdate(string instrument, string result)
        {
            await Clients.Group(instrument).SendAsync("ReceivePriceUpdate", instrument, result);
        }
    }
}
