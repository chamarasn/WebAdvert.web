namespace WebAdvert.web.Models
{
    public class Advertisement
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }
    }
}