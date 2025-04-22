using System.ComponentModel.DataAnnotations;

namespace SignalR.ExampleProject.Models.ViewModels
{
    public record SignInViewModel([Required] string Email, [Required] string Password);

}
