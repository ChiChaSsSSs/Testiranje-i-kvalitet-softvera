namespace WebTemplate.Models;

public class VodjaSmene : Zaposleni
{
    public required Smena smena { get; set; }
    public List<Prodavac>? Prodavci { get; set; }

}