namespace firstMVC.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Text { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public CurrentStatus Status { get; set; }
    }
}
