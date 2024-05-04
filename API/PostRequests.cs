using FanficBE.Models;
using Microsoft.EntityFrameworkCore;

namespace FanficBE.API
{
    public class PostRequests
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/posts", (FanficBEDbContext db) =>
            {
                return db.Posts.ToList();
            });

            app.MapPost("/posts", (FanficBEDbContext db, Post post) =>
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return Results.Created($"/posts/{post.Id}", post);
            });

            app.MapGet("/posts/{id}", (FanficBEDbContext db, int id) =>
            {
                 return db.Posts
                    .Where(p => p.Id == id)
                    .Include(p => p.Users)
                    .Include(p => p.Categories)
                    .Select(p => new
                    {
                        p.Id,
                        p.Title,
                        p.Content,
                        p.CategoryId,
                        p.UserId,
                        Users = p.Users.Select(u => new
                        {
                            id = u.Id,
                            firstName = u.FirstName,
                            lastName = u.LastName,
                            email = u.Email,
                            bio = u.Bio
                        }),
                        Categories = p.Categories.Select(c => new
                        {
                            id = c.Id,
                            label = c.Label,
                        })
                    });
            });

            app.MapDelete("/posts/{id}", (FanficBEDbContext db, int id) =>
            {
                Post post = db.Posts.SingleOrDefault(c => c.Id == id);
                if (post == null)
                {
                    return Results.NotFound();
                }
                db.Posts.Remove(post);
                db.SaveChanges();
                return Results.NoContent();
            });

            app.MapPatch("/posts/{id}", async (FanficBEDbContext db, int id, Post updatedPost) =>
            {
                var existingPost = await db.Posts.FindAsync(id);
                if (existingPost == null)
                {
                    return Results.NotFound();
                }

                existingPost.Title = updatedPost.Title;
                existingPost.Content = updatedPost.Content;
                existingPost.CategoryId = updatedPost.CategoryId;

                await db.SaveChangesAsync();

                return Results.Ok(existingPost);
            });

            app.MapGet("/posts/user/{userId}", (FanficBEDbContext db, int userId) =>
            {
                return db.Posts.Where(post => post.UserId == userId).ToList();
            });


            app.MapGet("/posts/category/{categoryId}", (FanficBEDbContext db, int categoryId) =>
            {
                return db.Posts.Where(post => post.CategoryId == categoryId).ToList();
            });

            app.MapGet("/search", (FanficBEDbContext db, string searchValue) =>
            {

                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    return Results.BadRequest();
                }

                searchValue = searchValue.Trim();
                var userResults = db.Users
                    .Where(user =>
                        user.FirstName.ToLower().Contains(searchValue.ToLower()) ||
                        user.LastName.ToLower().Contains(searchValue.ToLower()) ||
                        user.Email.ToLower().Contains(searchValue.ToLower()) ||
                        user.Bio.ToLower().Contains(searchValue.ToLower())
                    )
                    .ToList();

                var postResults = db.Posts
                    .Where(post =>
                        post.Title.ToLower().Contains(searchValue.ToLower()) ||
                        post.Content.ToLower().Contains(searchValue.ToLower()) ||
                        post.Comments.Any(comment => comment.Content.ToLower().Contains(searchValue.ToLower()))
                    )
                    .ToList();

                var categoryResults = db.Categories
                    .Where(category =>
                        category.Label.ToLower().Contains(searchValue.ToLower())
                    )
                    .ToList();

                var commentResults = db.Comments
                    .Where(comment =>
                        comment.Content.ToLower().Contains(searchValue.ToLower())
                    )
                    .ToList();

                var searchResults = new
                {
                    Users = userResults,
                    Posts = postResults,
                    Categories = categoryResults,
                    Comments = commentResults
                };

                return Results.Ok(searchResults);
            });
        }
    }
}
