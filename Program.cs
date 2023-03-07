using Microsoft.EntityFrameworkCore;
using NodaTime;

using AppDbContext db = new();
await db.Database.EnsureDeletedAsync();
await db.Database.EnsureCreatedAsync();

DateTimeZone timeZone = DateTimeZoneProviders.Tzdb["Asia/Tehran"];
var result = await db.Purchases
	.GroupBy(p => p.CreatedAt.InZone(timeZone).Date)
	.Select(g => new
	{
		Date = g.Key,
		Count = g.Count(),
	})
	.ToListAsync();

public class Purchase
{
	public int Id { get; set; }
	public Instant CreatedAt { get; set; }
}

public class AppDbContext : DbContext
{
	public DbSet<Purchase> Purchases => Set<Purchase>();

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=123456;Database=temp;", o => o.UseNodaTime());
}
