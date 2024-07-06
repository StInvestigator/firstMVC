using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace firstMVC.Models.Forms
{
    public class ChangeCustomerDataForm
    {
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [Display(Name = "Повне ім'я")]
        public string Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Електронна адреса обов'язкова")]
        [Display(Name = "Логін / Електронна адреса")]
        public string Email { get; set; }
    }
}
