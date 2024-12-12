namespace WebTemplate.Models;
public class Proizvod
{
    public int Id { get; set; }
    public required string Naziv { get; set; }
    public required string Kategorija { get; set; }
    public double Cena { get; set; }
    public int DostupnaKolicina { get; set; }
    public Prodavnica ?Prodavnica { get; set; }
}