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
                return db.Posts.Where(c => c.Id == id).FirstOrDefault();
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
        }
    }
}
