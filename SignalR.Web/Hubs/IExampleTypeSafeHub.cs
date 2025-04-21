using SignalR.Web.Model;

namespace SignalR.Web.Hubs
{

    //Tip güvenlikli bir hub oluşturmak için, hub sınıfını Hub<T> arayüzü ile genişletilir
    //ana sınıf artık clientlere veri göndermek için metot başlıklarını kullanabilir
    public interface IExampleTypeSafeHub
    {

        Task ReceiveMessageForAllClient(string message);
        Task ReceiveConnectedClientCountForAllClient(int clientCount);
        Task ReceiveMessageForCallerClient(string message); 
        Task ReceiveMessageForOtherClient(string message);
        Task ReceiveMessageForIndividualClient(string message);
        Task ReceiveMessageForGroupClients(string message);
        Task ReceiveTypedMessageForAllClient(Product product);
        Task ReceiveMessageAsStreamForAllClients(string name);
        Task ReceiveProductAsStreamForAllClients(Product product);

    }
}
