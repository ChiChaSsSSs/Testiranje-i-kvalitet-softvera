namespace WebTemplate.Models;

public class Prodavac : Zaposleni
{
    public double ukupnaCenaProdatihProizvoda { get; set; }
    public double mesecniBonus { get; set; }
    public VodjaSmene? vodjaSmene { get; set; }

}