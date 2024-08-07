﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using firstMVC.Models;

namespace firstMVC.Areas.Admin.Models.Forms
{
    public class ProfessionForm
    {
        public ProfessionForm() { }
        public ProfessionForm(Profession profession)
        {
            Id = profession.Id;
            Name = profession.Name;
        }
        public int Id { get; set; }

        [DisplayName("Назва")]
        [Required(ErrorMessage = "Професія необхідна")]
        public string Name { get; set; }

        [DisplayName("Лого")]
        public IFormFile? Image { get; set; }
    }
}
