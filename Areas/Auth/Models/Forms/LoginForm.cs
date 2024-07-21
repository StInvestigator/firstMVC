using System.ComponentModel.DataAnnotations;

namespace firstMVC.Areas.Auth.Models.Forms
{
    public class LoginForm
    {
        [EmailAddress]
        [Required(ErrorMessage = "Електронна адреса обов'язкова")]
        [Display(Name = "Логін / Електронна адреса")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль обов'язковий")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
