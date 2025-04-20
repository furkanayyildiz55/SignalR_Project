using Microsoft.AspNetCore.SignalR.Client;
using SignalRClient.ConsoleApp.Model;

Console.WriteLine("SignalR Console Client ");
Console.ReadKey();

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7256/exampleTypeSafeHub")
    .Build();

connection.StartAsync().ContinueWith((result) => {

    Console.WriteLine(result.IsCompletedSuccessfully ? "Connected" : "Connnected Failed");
});

connection.On<Product>("ReceiveTypedMessageForAllClient", (product) => {

    Console.WriteLine($"Received Message:  {product.Id} , {product.Name} , {product.Price}");

} );


Console.ReadKey();