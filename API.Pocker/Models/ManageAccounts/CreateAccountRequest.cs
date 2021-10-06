
using System.ComponentModel.DataAnnotations;
namespace API.Pocker.Models.ManageAccounts
{
    public class CreateAccountRequest
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
