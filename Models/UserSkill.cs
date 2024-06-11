using System.ComponentModel;

namespace firstMVC.Models
{
    public class UserSkill
    {
        public int Id { get; set; }
        [DisplayName("Навичка")]
        public Skill Skill { get; set; }

        [DisplayName("Відсоток майстерності")]
        public int Mastery { get; set; }
    }
}
