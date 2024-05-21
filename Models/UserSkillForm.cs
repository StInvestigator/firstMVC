using System.ComponentModel;

namespace firstMVC.Models
{
    public class UserSkillForm
    {
        [DisplayName("Навичка")]
        public int SkillId { get; set; }

        [DisplayName("Відсоток майстерності")]
        public int Mastery { get; set; }
    }
}
