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
                return db.Posts
                    .Include(p => p.Categories) 
                    .Include(p => p.Comments) 
                    .ToListAsync();
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
                    .Include(p => p.Categories)
                    .Include(p => p.Comments)
                    .Select(p => new
                    {
                        p.Id,
                        p.Title,
                        p.Content,
                        p.CategoryId,
                        p.UserId,
                        Categories = p.Categories.Select(c => new
                        {
                            id = c.Id,
                            label = c.Label,
                        }),
                        Comments = p.Comments.Select(comment => new
                        {
                            id = comment.Id,
                            userId = comment.UserId,
                            postId = comment.PostId,
                            content = comment.Content,
                            createdOn = comment.CreatedOn
                        })
                    })
                    .SingleOrDefault(); 
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

            app.MapGet("/search", async (FanficBEDbContext db, string searchValue) =>
            {
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    return Results.BadRequest();
                }

                searchValue = searchValue.Trim();

                var postResults = await db.Posts
                    .Include(post => post.User)
                    .Where(post =>
                        post.Title.ToLower().Contains(searchValue.ToLower()) ||
                        post.Content.ToLower().Contains(searchValue.ToLower()) ||
                        post.Comments.Any(comment => comment.Content.ToLower().Contains(searchValue.ToLower())) ||
                        post.User.FirstName.ToLower().Contains(searchValue.ToLower()) ||
                        post.User.LastName.ToLower().Contains(searchValue.ToLower()) ||
                        post.User.Email.ToLower().Contains(searchValue.ToLower()) ||
                        post.User.Bio.ToLower().Contains(searchValue.ToLower()) ||
                        post.Categories.Any(category => category.Label.ToLower().Contains(searchValue.ToLower()))
                    )
                    .Include(post => post.Categories)
                    .Include(post => post.Comments)
                    .ToListAsync();

                return Results.Ok(postResults);
            });
        }
    }
}
