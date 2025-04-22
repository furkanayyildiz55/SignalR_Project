using System.ComponentModel.DataAnnotations;

namespace SignalR.ExampleProject.Models.ViewModels
{
    public record SignUpViewModel([Required] string Email, [Required] string Password, [Required] string ConfirmPassword);
}
