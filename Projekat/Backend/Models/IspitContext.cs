namespace WebTemplate.Models;

public class IspitContext : DbContext
{
    // DbSet kolekcije!
    public required DbSet<Prodavnica> Prodavnice { get; set; }
    public required DbSet<Proizvod> Proizvodi { get; set; }
    public required DbSet<Zaposleni> Zaposleni { get; set; }
    public required DbSet<Smena> Smene { get; set; }
    public IspitContext(DbContextOptions options) : base(options)
    {
        
    }
}
