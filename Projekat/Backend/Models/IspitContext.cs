namespace WebTemplate.Models;

public class IspitContext : DbContext
{
    // DbSet kolekcije!
    public DbSet<Prodavnica> Prodavnice { get; set; }
    public DbSet<Proizvod> Proizvodi { get; set; }
    public IspitContext(DbContextOptions options) : base(options)
    {
        
    }
}
