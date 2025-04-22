using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalR.ExampleProject.Models;

namespace SignalR.ExampleProject.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) 
        : IdentityDbContext<IdentityUser,IdentityRole,string>(options)
    {

        public DbSet<Product> products { get; set; }

    }
}
