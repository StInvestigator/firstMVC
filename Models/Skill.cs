namespace firstMVC.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Mastery { get; set; }
        public Skill(int id, string name, int mastery)
        {
            Id = id;
            Name = name;
            Mastery = mastery;
        }
    }
}
