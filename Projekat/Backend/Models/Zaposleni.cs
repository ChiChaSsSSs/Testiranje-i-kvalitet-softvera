namespace WebTemplate.Models;

public abstract class Zaposleni
{
    [Key]
    public int Id { get; set; }

    [MaxLength(20)]
    public required string Ime { get; set; }
    [MaxLength(40)]
    public required string Prezime { get; set; }
    public required string Email { get; set; }
    
    [Length(13, 13)]
    public required string JMBG { get; set; }
    public required string slika {get; set; }
    public required Prodavnica prodavnica { get; set; }
}