using Microsoft.EntityFrameworkCore;

namespace AttractionReviewAPI;

public class APIDBContext : DbContext
{
    public DbSet<Attraction> Attractions { get; set; }
    public DbSet<Review> Reviews { get; set; }
    
    public APIDBContext(DbContextOptions<APIDBContext> options)
        : base(options) { }
}