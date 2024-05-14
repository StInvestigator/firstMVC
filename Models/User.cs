﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace firstMVC.Models
{
    public class User
    {
        [DisplayName("Ім'я")]
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [MaxLength(20, ErrorMessage = "Максимум 20 символів")]
        public string Name { get; set; }

        [DisplayName("Вік")]
        [Required(ErrorMessage = "Вік обов'язковий")]

        public int Age { get; set; }

        [DisplayName("Чи чоловік")]
        public bool IsMale { get; set; }


        [DisplayName("Баланс")]
        [Required(ErrorMessage = "Баланс обов'язковий")]
        public decimal Balance { get; set; }

        [DisplayName("День Народження")]
        [Required(ErrorMessage = "День Народження обов'язковий")]

        public DateTime Birthday { get; set; }
    }
}
