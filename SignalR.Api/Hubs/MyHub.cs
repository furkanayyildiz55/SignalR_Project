using Microsoft.AspNetCore.SignalR;

namespace SignalR.Api.Hubs
{
    public class MyHub : Hub<IMyHub>
    {
        public async Task BroadcastMessageToClient(string message)
        {
            await Clients.All.ReceiveMessageForAllClient(message);
        }
    }
}
