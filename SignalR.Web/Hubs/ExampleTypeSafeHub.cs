using Microsoft.AspNetCore.SignalR;
using SignalR.Web.Model;

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

        #region Grup İşlemleri
        //Grup ismi vererek içindeki clientlere mesaj göndeme
        public async Task BroadcastMessageToGroupClients(string groupName, string message)
        {
            await Clients.Group(groupName).ReceiveMessageForGroupClients(message);
        }

        //clienti grupa ekleme
        public async Task AddGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessageForCallerClient($"(Caller) Yeni bir gruba eklendin. Grup Adı: {groupName}");
            await Clients.Groups(groupName).ReceiveMessageForGroupClients($"(Groups) Yeni bir kullanıcı bizim gruba eklendi.");
        }
        //clienti gruptan çıkarma
        public async Task RemoveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessageForCallerClient($"(Caller) Bir gruptan Ayrıldın. Grup Adı: {groupName}");
            await Clients.Groups(groupName).ReceiveMessageForGroupClients($"(Groups) Yeni bir kullanıcı bizim gruptan ayrıldı.");
        }

        #endregion

        #region Tip Güvenlikli Gönderim

        public async Task BroadcastTypedMessageToAllClient(Product product)
        {
            await Clients.All.ReceiveTypedMessageForAllClient(product);
        }


        #endregion
    }
}
