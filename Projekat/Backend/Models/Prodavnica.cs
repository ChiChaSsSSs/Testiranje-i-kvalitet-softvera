namespace WebTemplate.Models;
public class Prodavnica
{  
    [Key]
    public int Id { get; set; }
    public required string Naziv { get; set; }
    public required string Lokacija { get; set; }
    public required string BrojTelefona { get; set; }
    public List<Proizvod> ?Proizvodi { get; set; }
    public List<Zaposleni> ?listaZaposlenih { get; set; }
}