using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NuGet.Protocol.Resources;
using SignalR.ExampleProject.BackgroundServices;
using SignalR.ExampleProject.Hubs;
using SignalR.ExampleProject.Models;
using SignalR.ExampleProject.Service;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

//Dosyalar çalışmak için kullanılır, wwwroot okuma işlemleri kolaylaşır
builder.Services.AddSingleton<IFileProvider>( new PhysicalFileProvider(Directory.GetCurrentDirectory()));

//Uygulamanın herhangi bir yerinden geçerli HTTP bağlamına (context) erişim sağlamak için kullanılır
builder.Services.AddHttpContextAccessor();

//FileService classı içerisinde http verilerine erişebilmek için scope olarak tanımlandı, bu nedenle scoped tanılanarak her istekte yeni bir instance oluşturulması sağlandı
//builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<IFileService, FileService>(); //veya bu şekilde DI konteynırına eklenebilir
//builder.Services.Add(new ServiceDescriptor(typeof(FileService) , ServiceLifetime.Scoped )); //veya bu şekilde DI konteynırına eklenebilir

//Shannel yapısı tanımladık
builder.Services.AddSingleton(Channel.CreateUnbounded<(string userId, List<Product> products)>());
//builder.Services.AddSingleton(Channel.CreateUnbounded<Tuple<string, List<Product>>>());


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));

});

builder.Services.AddSignalR();

//??
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

//Backgroud servisi ayağa kaldırılacak
builder.Services.AddHostedService<CreateExcelBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapHub<AppHub>("/hub");

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
