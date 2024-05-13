using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace firstMVC.Models
{
    public class User
    {
        [DisplayName("Ім'я")]
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [MinLength(3, ErrorMessage = "Мінімум 3 символи")]
        [MaxLength(20, ErrorMessage = "Максимум 20 символів")]
        public string Name { get; set; }


        [DisplayName("Пароль")]
        [Required(ErrorMessage = "Пароль обов'язковий")]
        [MinLength(8, ErrorMessage = "Мінімум 8 символів")]
        [MaxLength(20, ErrorMessage = "Максимум 20 символів")]
        public string Password { get; set; }
    }
}
