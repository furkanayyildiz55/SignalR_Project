using Microsoft.AspNetCore.SignalR;

namespace SignalR.Web.Hubs
{
    public class ExampleTypeSafeHub : Hub<IExampleTypeSafeHub>
    {
        private static int ConnectedClientsCount = 0;

        public async Task BroadcastMessageToClient(string message)
        {
            await Clients.All.ReceiveMessageForAllClient(message);
        }

        //Hub a bir istemci bağlandığında tetiklenir
        public override async Task  OnConnectedAsync()
        {
            ConnectedClientsCount++;
            await Clients.All.ReceiveConnectedClientCountForAllClient(ConnectedClientsCount);
            base.OnConnectedAsync();
        }

        //Hub dan bir istemci ayrıldığında tetiklenir
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedClientsCount--;
            await Clients.All.ReceiveConnectedClientCountForAllClient(ConnectedClientsCount);
            base.OnDisconnectedAsync(exception);
        }
    }
}
