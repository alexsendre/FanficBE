using FanficBE.DTOs;
using FanficBE.Models;

namespace FanficBE.API
{
    public class UserRequests
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/users/{id}", (FanficBEDbContext db, int id) =>
            {
                return db.Users.SingleOrDefault(u => u.Id == id);
            });

            app.MapPost("/checkuser", (FanficBEDbContext db, UserAuthDto userAuthDto) =>
            {
                var userUid = db.Users.SingleOrDefault(u => u.Uid == userAuthDto.Uid);

                if (userUid == null)
                {
                    return Results.NotFound();
                }
                else
                {
                    return Results.Ok(userUid);
                }
            });

            app.MapPost("/register", (FanficBEDbContext db, User user) =>
            {
                db.Users.Add(user);
                db.SaveChanges();
                return Results.Created($"/user/{user.Id}", user);
            });
        }
    }
}
