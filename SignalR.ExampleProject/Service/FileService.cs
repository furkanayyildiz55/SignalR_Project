using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalR.ExampleProject.Models;
using System.Threading.Channels;

namespace SignalR.ExampleProject.Service
{
    public class FileService(
        AppDbContext context, 
        IHttpContextAccessor httpContextAccessor, 
        UserManager<IdentityUser> userManager,
        Channel<(string userId, List<Product> products)> channel) : IFileService
    {

        public async Task<bool> AddMessageToQueue()
        {
            var userId = userManager.GetUserId(httpContextAccessor!.HttpContext!.User);

            var products = await context.Products.Where(x => x.UserId == userId).ToListAsync();

            //(veri,veri) parantez içerisine yazılmış veri gizli bir tuple nesnesi oluşturur
            return channel.Writer.TryWrite((userId, products));
        }
    }
}
