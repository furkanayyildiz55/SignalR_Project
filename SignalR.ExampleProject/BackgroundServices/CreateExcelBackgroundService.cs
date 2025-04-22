
using ClosedXML.Excel;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileProviders;
using SignalR.ExampleProject.Hubs;
using SignalR.ExampleProject.Models;
using System.Data;
using System.Threading.Channels;

namespace SignalR.ExampleProject.BackgroundServices
{
    public class CreateExcelBackgroundService(
        Channel<(string userId, List<Product> products)> channel,
        IFileProvider fileProvider,
        IServiceProvider serviceProvider
        ) : BackgroundService
    {
        //Uygulama ayağa kalkınca çalışır ve sadece bir keere çalışır
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Bu kısım kuyrukta mesaj var ise true olarak döner
            //kuyruktaki mesajlar bitt ise kod bloklanır, kuyruğa mesaj geldiği anda tekrar true dönerek işleme devam eder
            while ( await channel.Reader.WaitToReadAsync(stoppingToken))
            {
                var (userId,products) = await channel.Reader.ReadAsync(stoppingToken);  //stoppingToken Buranın amaçlarından biri de uygulama kapanırken ReadAsync metodunu beklememesi için

                await Task.Delay(4000);
                var wwwfootFolder = fileProvider.GetDirectoryContents("wwwroot");
                var files = wwwfootFolder.Single(x => x.Name == "files");

                var newExcelFileName = $"product-list-{Guid.NewGuid()}.xlsx";
                var newExcelFilePath = Path.Combine(files.PhysicalPath, newExcelFileName);

                var wb = new XLWorkbook();
                var ds = new DataSet();
                ds.Tables.Add(GetTable("Product List", products));
                wb.Worksheets.Add(ds);
                await using var excelFileStream = new FileStream(newExcelFilePath, FileMode.Create);

                wb.SaveAs(excelFileStream);

                //AppHub'ı neden serviceProvider içerisinde aldık
                //CreateExcelBackgroundService Singleton yaşam döngüsüne sahip, AppHub ise scope bu DI üzerinden alamadık

                using (var scope = serviceProvider.CreateScope())
                {
                    //ServiceProvider üzerinden AppHub nesnesi oluşturuluyor
                    var appHub = scope.ServiceProvider.GetRequiredService<IHubContext<AppHub>>();
                    //Identity bize kullanıcının userId bilgisini veriyor böylelikle sadece o kullanıcıda istediğimiz metodu tetikliyoruz ve veri gönderiyoruz.
                    await appHub.Clients.User(userId).SendAsync("AlertCompleteFile", $"/files/{newExcelFileName}", stoppingToken);
                }
            }
        }

        private DataTable GetTable(string tableName, List<Product> products)
        {
            var table = new DataTable { TableName = tableName };
            foreach (var item in typeof(Product).GetProperties()) table.Columns.Add(item.Name, item.PropertyType);
            products.ForEach(x => { table.Rows.Add(x.Id, x.Name, x.Price, x.Description, x.UserId); });
            return table;
        }

    }
}
