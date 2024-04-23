namespace FanficBE.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Staff { get; set; }
        public string Uid { get; set; }
    }
}
