using Microsoft.EntityFrameworkCore;
using PickTimeTogether.Models;

namespace PickTimeTogether;

public class AppDbContext : DbContext
{
    public DbSet<Poll> Polls => Set<Poll>();
    public DbSet<PollResponse> PollResponses => Set<PollResponse>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
        Database.EnsureCreated();
    }
}
