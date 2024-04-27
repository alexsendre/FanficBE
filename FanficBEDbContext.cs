using Microsoft.EntityFrameworkCore;
using FanficBE.Models;

namespace FanficBE
{
    public class FanficBEDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }

        public FanficBEDbContext(DbContextOptions<FanficBEDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User[]
            {
                new User { Id = 1, Uid = "user1", FirstName = "John", LastName = "Doe", Image = "image1.jpg", Email = "john@example.com", Bio = "Bio of user 1", CreatedOn = DateTime.Now, Staff = false },
                new User { Id = 2, Uid = "user2", FirstName = "Jane", LastName = "Doe", Image = "image2.jpg", Email = "jane@example.com", Bio = "Bio of user 2", CreatedOn = DateTime.Now, Staff = false },
                new User { Id = 3, Uid = "user3", FirstName = "Alex", LastName = "Smith", Image = "image3.jpg", Email = "alex@example.com", Bio = "Bio of user 3", CreatedOn = DateTime.Now, Staff = false },
                new User { Id = 4, Uid = "user4", FirstName = "Emily", LastName = "Johnson", Image = "image4.jpg", Email = "emily@example.com", Bio = "Bio of user 4", CreatedOn = DateTime.Now, Staff = false }
            });

            modelBuilder.Entity<Post>().HasData(new Post[]
            {
                new Post { Id = 1, UserId = 1, Title = "Post 1 Title", Content = "Content of post 1", CategoryId = 1, },
                new Post { Id = 2, UserId = 2, Title = "Post 2 Title", Content = "Content of post 2", CategoryId = 2, },
                new Post { Id = 3, UserId = 3, Title = "Post 3 Title", Content = "Content of post 3", CategoryId = 3, },
                new Post { Id = 4, UserId = 4, Title = "Post 4 Title", Content = "Content of post 4", CategoryId = 4, }
            });

            modelBuilder.Entity<Category>().HasData(new Category[]
            {
                new Category { Id = 1, Label = "category one" },
                new Category { Id = 2, Label = "category two" },
                new Category { Id = 3, Label = "category three" },
                new Category { Id = 4, Label = "category four" }
            });

            modelBuilder.Entity<Comment>().HasData(new Comment[]
            {
                new Comment { Id = 2, UserId = 3, PostId = 2, Content = "comment 2!", CreatedOn = DateTime.Now },
                new Comment { Id = 3, UserId = 2, PostId = 3, Content = "a new comment, totally new", CreatedOn = DateTime.Now },
                new Comment { Id = 4, UserId = 1, PostId = 1, Content = "this is also a comment", CreatedOn = DateTime.Now },
                new Comment { Id = 1, UserId = 4, PostId = 1, Content = "this is a comment!", CreatedOn = DateTime.Now },
            });
        }
    }
}