namespace firstMVC.Models
{
    public class AboutMe
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public AboutMe(string name, string surname, int age, string location, string imageUrl) 
        {
            Name = name;
            Surname = surname;
            Age = age;
            Location = location;
            ImageUrl = imageUrl;
        }
    }
}
