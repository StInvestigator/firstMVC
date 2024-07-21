using System.ComponentModel.DataAnnotations;

namespace firstMVC.Areas.Auth.Models.Forms
{
    public class RegisterForm : LoginForm
    {
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [Display(Name = "Повне ім'я")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Паролі повинні співпадати")]
        [Display(Name = "Підтвердження паролю")]
        public string ConfirmPassword { get; set; }
    }
}
