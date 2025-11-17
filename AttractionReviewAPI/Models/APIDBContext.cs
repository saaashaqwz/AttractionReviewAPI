using Microsoft.EntityFrameworkCore;

namespace AttractionReviewAPI;

public class APIDBContext : DbContext
{
    public DbSet<Attraction> Attractions { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public APIDBContext(DbContextOptions<APIDBContext> options)
        : base(options) { }
}