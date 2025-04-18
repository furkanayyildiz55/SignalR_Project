namespace SignalR.Web.Hubs
{
    public interface IExampleTypeSafeHub
    {
        /// <summary>
        ///         //Tip güvenlikli bir hub oluşturmak için, hub sınıfını Hub<T> arayüzü ile genişletilir
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task ReceiveMessageForAllClient(string message);
    }
}
