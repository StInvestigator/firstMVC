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

        [DisplayName("Чи чоловік")]
        [Required(ErrorMessage = "Стать обов'язковa")]
        public bool IsMale { get; set; }

        [DisplayName("Вік")]
        public int Age { get; set; }

        [DisplayName("Баланс")]
        [Required(ErrorMessage = "Вік обов'язковий")]
        public decimal Balance { get; set; }

        [DisplayName("День Народження")]
        [Required(ErrorMessage = "День Народження обов'язковий")]

        public DateTime Birthday { get; set; }
    }
}
