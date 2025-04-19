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

        //Sadece çağrıyı yapan istemciye mesaj gönderir
        public async Task BroadcastMessageToCallerClient(string message)   //Bu metot istemci tarafından tetiklenir
        {
            await Clients.Caller.ReceiveMessageForCallerClient(message);  //burası ise istemcide bir metot tetikler 
        }

        //Sadece diğer istemcilere mesaj gönderir, çağrıyı yapan isytemciye gönderilmez
        public async Task BroadcastMessageToOtherClient(string message)
        {
            await Clients.Others.ReceiveMessageForOtherClient(message);
        }


        //spesific bir istemciye mesaj gönderir
        public async Task BroadcastMessageToIndividualClient(string connectionId, string message)
        {
            await Clients.Client(connectionId).ReceiveMessageForIndividualClient(message);
        } 
    }
}
