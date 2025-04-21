namespace SignalR.Api.Hubs
{
    public interface IMyHub
    {
        Task ReceiveMessageForAllClient(string message);

    }
}
