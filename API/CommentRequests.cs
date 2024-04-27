using FanficBE.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace FanficBE.API
{
    public class CommentRequests
    {
        public static void Map(WebApplication app)
        {
            // get post comments
            app.MapGet("/posts/{id}/comments", (FanficBEDbContext db, int id) =>
            {
                var post = db.Posts.Include(p => p.Comments).SingleOrDefault(p => p.Id == id);

                if (post == null)
                {
                    return Results.NotFound("Unable to find post");
                }

                var response = new
                {
                    comments = post.Comments.Select(comment => new
                    {
                        id = comment.Id,
                        authorId = comment.UserId,
                        postId = comment.PostId,
                        User = db.Users.Where(u => u.Id == comment.UserId)
                            .Select(u => u.FirstName + " " + u.LastName).FirstOrDefault(),
                        content = comment.Content,
                        createdOn = comment.CreatedOn.ToString("MM/dd/yy"),
                    }),
                };

                return Results.Ok(response);
            });

            // add comment to post
            app.MapPost("/posts/{postId}/comments", (FanficBEDbContext db, string comment, int postId, int userId) =>
            {
                if (comment == null)
                {
                    return Results.NotFound("You must type something to submit");
                }

                var response = new Comment
                {
                    PostId = postId,
                    UserId = userId,
                    Content = comment,
                };

                try
                {
                    db.Comments.Add(response);
                    db.SaveChanges();
                    return Results.Created($"/posts/{postId}/comments/{response.Id}", response);
                }
                catch (DbException ex)
                {
                    return Results.BadRequest($"There was an error: {ex.Message}");
                }
            });

            // delete comment from post
            app.MapDelete("/posts/{postId}/comments/{commentId}", (FanficBEDbContext db, int postId, int commentId) =>
            {
                var post = db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == postId);
                var comment = db.Comments.Find(commentId);

                if (post == null || comment == null)
                {
                    return Results.NotFound("Unable to find the requested data");
                }

                post.Comments.Remove(comment);
                db.SaveChanges();
                return Results.NoContent();
            });

            // update comment on a post
            app.MapPatch("/posts/{postId}/comments/{commentId}", (FanficBEDbContext db, int postId, int commentId, string updatedComment) =>
            {
                var post = db.Posts.Include(p => p.Comments).SingleOrDefault(p => p.Id == postId);
                Comment commentToUpdate = db.Comments.SingleOrDefault(c => c.Id == commentId);

                if (commentToUpdate == null)
                {
                    return Results.NotFound("There was an error, the comment you requested was not found");
                }

                commentToUpdate.Content = updatedComment;

                db.SaveChanges();
                return Results.NoContent();
            });
        }
    }
}
