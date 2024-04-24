namespace FanficBE.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
