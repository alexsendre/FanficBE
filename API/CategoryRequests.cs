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
        }
    }
}
