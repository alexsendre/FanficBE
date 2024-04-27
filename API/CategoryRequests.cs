namespace FanficBE.API
{
    public class CategoryRequests
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/categories", (FanficBEDbContext db) =>
            {
                return db.Categories.ToList();
            });

            app.MapGet("/categories/{id}", (FanficBEDbContext db, int id) =>
            {
                return db.Categories.SingleOrDefault(c => c.Id == id);
            });
        }
    }
}
