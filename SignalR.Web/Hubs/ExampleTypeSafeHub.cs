using Microsoft.AspNetCore.SignalR;
using SignalR.Web.Model;

namespace SignalR.Web.Hubs
{
    public class ExampleTypeSafeHub : Hub<IExampleTypeSafeHub>
    {
        private static int ConnectedClientsCount = 0;

        #region İstemci İşlemleri
        //Tüm istemcilere mesaj gönderir
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
        #endregion

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

        #region Stream Kullanımı


        //İstemci taradından her next ifadesi ile veri gönderildiğinde IAsyncEnumerable içerisine gelir
        //Veri gönderimlerinde metot bir kere tetiklerinir sonraki her next ifadesinde veri nameAsChunks içierisne otomatik gelir.
        public async Task BroadcastStreamDataToAllClient(IAsyncEnumerable<string> nameAsChunks)
        {
            await foreach (var name in nameAsChunks)
            {
                await Clients.All.ReceiveMessageAsStreamForAllClients(name);
                await Task.Delay(1000);
 
            }
        }

        public async Task BroadcastStreamProductDataToAllClient(IAsyncEnumerable<Product> parts)
        {
            await foreach (Product product in parts)
            {
                await Clients.All.ReceiveProductAsStreamForAllClients(product);
                await Task.Delay(1000);

            }
        }

        //Client tarafı metotdu tetikler ve cliente stream olarak veri döner
        //Metod sadece tetikleyen cliente veri gönderir, diğer clientler bu metodu bilmez.
        public async IAsyncEnumerable<string> BroadcastFromHubToClient(int count)
        {
            foreach (var item in Enumerable.Range(1,count).ToList())
            {
                await Task.Delay(1000);
                yield return $"{item}. Data";  //Foreach içerisinde yield keywordu ile anlık olarak data dönebiliyoruz.
            }
        }

        #endregion
    }
}
