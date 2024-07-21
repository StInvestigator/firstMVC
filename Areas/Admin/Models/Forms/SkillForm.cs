using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using firstMVC.Models;

namespace firstMVC.Areas.Admin.Models.Forms
{
    public class SkillForm
    {
        public SkillForm() { }
        public SkillForm(Skill skill)
        {
            Id = skill.Id;
            Name = skill.Name;
        }
        public int Id { get; set; }

        [DisplayName("Назва")]
        [Required(ErrorMessage = "Назва необхідна")]
        public string Name { get; set; }

        [DisplayName("Лого")]
        public IFormFile? Image { get; set; }
    }
}
