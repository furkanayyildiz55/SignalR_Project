namespace SignalR.Web.Hubs
{

    //Tip güvenlikli bir hub oluşturmak için, hub sınıfını Hub<T> arayüzü ile genişletilir
    //ana sınıf artık clientlere veri göndermek için metot başlıklarını kullanabilir
    public interface IExampleTypeSafeHub
    {

        Task ReceiveMessageForAllClient(string message);
        Task ReceiveConnectedClientCountForAllClient(int clientCount);
    }
}
