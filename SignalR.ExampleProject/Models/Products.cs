using Microsoft.EntityFrameworkCore;

namespace SignalR.ExampleProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        [Precision(18, 2)]
        public decimal Price { get; set; }
        public string Description { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }
}



//  = default!; ifadeleri parametrenin default değer alamayacağını belirtir
//  [Precision(18,2)] 18 karakterlik bir dize olabilir, son iki karakteri 2 hane
