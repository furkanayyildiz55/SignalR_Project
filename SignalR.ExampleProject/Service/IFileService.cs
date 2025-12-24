namespace SignalR.ExampleProject.Service
{
    public interface IFileService
    {
        public Task<bool> AddMessageToQueue();
    }
}
