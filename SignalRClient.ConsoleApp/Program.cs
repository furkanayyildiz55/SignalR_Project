using Microsoft.AspNetCore.SignalR.Client;
using SignalRClient.ConsoleApp.Model;

Console.WriteLine("SignalR Console Client ");
Console.WriteLine("Başlamak için bir tuşa tıklayınız");
Console.ReadKey();

//Uzak sunucudaki SignalR hub'ına bağlanmak için bir HubConnectionBuilder nesnesi oluşturuyoruz
var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7256/exampleTypeSafeHub")
    .Build();

//Bağlantıyı başlatıyoruz ve bağlantının başarılı olup olmadığını kontrol ediyoruz
connection.StartAsync().ContinueWith((result) => {

    Console.WriteLine(result.IsCompletedSuccessfully ? "Connected Successfully" : "Connnected Failed");
});

//Sunucu tarafından gönderilen mesajları dinlemek için bir olay dinleyici ekliyoruz
connection.On<Product>("ReceiveTypedMessageForAllClient", (product) => {

    Console.WriteLine($"Received Product:  {product.Id} , {product.Name} , {product.Price}");
});


Thread.Sleep(3000);
Console.WriteLine("Sunucuya Mesaj Göndermek İçin Tıklayın");
ConsoleKeyInfo keyInfo = Console.ReadKey();

connection.InvokeAsync("BroadcastMessageToClient", "Merhaba Ben FormApplication.");


Console.ReadKey();