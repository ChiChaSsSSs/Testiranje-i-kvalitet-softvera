using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;

namespace WebTemplate.Controllers;

[ApiController]
[Route("[controller]")]
public class IspitController : ControllerBase
{
    public IspitContext Context { get; set; }

    public IspitController(IspitContext context)
    {
        Context = context;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("DodajProdavnicu")]
    public async Task<ActionResult> DodajProdavnicu([FromBody] Prodavnica prodavnica)
    {
        try
        {
            await Context.Prodavnice.AddAsync(prodavnica);
            await Context.SaveChangesAsync();
            return Ok("Dodata je prodavnica \"" + prodavnica.Naziv +"\"" + " sa ID: " + prodavnica.Id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("DodajProzivod/{idProdavnice}/{nazivProizvoda}/{kategorijaProzivoda}/{cenaProzivoda}/{kolicina}")]
    public async Task<ActionResult> DodajProzivod(int idProdavnice, string nazivProizvoda,
                                                    string kategorijaProzivoda, double cenaProzivoda, int kolicina)
    {
        try
        {
            var trazenaProdavnica = await Context.Prodavnice.FindAsync(idProdavnice);
            if (trazenaProdavnica == null)
            {
                return BadRequest("Uneta prodavnica ne postoji!");
            }
            if (kolicina > 100)
            {
                return BadRequest("Nije moguce dodati vise od 100 proizvoda!");
            }

            var noviProizvod = new Proizvod{
                Naziv = nazivProizvoda,
                Kategorija = kategorijaProzivoda,
                Cena = cenaProzivoda,
                DostupnaKolicina = kolicina,
                Prodavnica = trazenaProdavnica
            };
            await Context.Proizvodi.AddAsync(noviProizvod);
            await Context.SaveChangesAsync();
            return Ok("U prodavnicu \"" + trazenaProdavnica.Naziv + "\"" + " je dodat proizvod \"" +
                                        noviProizvod.Naziv + "\"" + " sa ID: " + noviProizvod.Id);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("ObrisiProizvod/{idProdavnice}/{nazivProizvoda}/{kategorijaProizvoda}/{cenaProizvoda}/{kolicina}")]
    public async Task<ActionResult> ObrisiProizvod(int idProdavnice, string nazivProizvoda,
                                                    string kategorijaProizvoda, double cenaProizvoda, int kolicina)
    {
        try
        {
            var trazenaProdavnica = await Context.Prodavnice.FindAsync(idProdavnice);
            if (trazenaProdavnica == null)
            {
                return BadRequest("Uneta prodavnica ne postoji");
            }

            var proizvodZaBrisanje = await Context.Proizvodi.FirstOrDefaultAsync(p =>
                p.Prodavnica!.Id == idProdavnice &&
                p.Naziv == nazivProizvoda &&
                p.Kategorija == kategorijaProizvoda &&
                p.Cena == cenaProizvoda &&
                p.DostupnaKolicina == kolicina);

            if (proizvodZaBrisanje == null)
            {
                return BadRequest("Proizvod ne postoji u prodavnici");
            }

            Context.Proizvodi.Remove(proizvodZaBrisanje);
            await Context.SaveChangesAsync();

            return Ok($"Proizvod \"{proizvodZaBrisanje.Naziv}\" je uspesno obrisan iz prodavnice \"{trazenaProdavnica.Naziv}\".");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("VratiProdavnice")]
    public async Task<ActionResult> VratiProdavnice()
    {
        try
        {
            var sveProdavnice = await Context.Prodavnice.Select(p => new{
                id = p.Id,
                naziv = p.Naziv
            }).ToListAsync();
            return Ok(sveProdavnice);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("VratiProizvodeProdavnice/{idProdavnice}")]
    public async Task <ActionResult> VratiProizvodeProdavnice (int idProdavnice)
    {
        try
        {
            var sveProdavnice = await Context.Proizvodi.Where(p => p.Prodavnica!.Id == idProdavnice)
                                                .Select(p => new
                                                            {
                                                                id = p.Id,
                                                                naziv = p.Naziv,
                                                                dostupno = p.DostupnaKolicina
                                                            }).ToListAsync();
            return Ok(sveProdavnice);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("ProdajProizvode/{idProizvoda}/{kolicina}")]
    public async Task<ActionResult> ProdajProizvode(int idProizvoda, int kolicina)
    {
        try
        {
            var trazeniProizvod = await Context.Proizvodi.FindAsync(idProizvoda);
            if (trazeniProizvod == null)
            {
                return BadRequest("Trazeni proizvod ne postoji!");
            }
            if (trazeniProizvod.DostupnaKolicina < kolicina)
            {
                return BadRequest("Nema dovoljno proizvoda u prodavnici!");
            }

            int staraKolicina = trazeniProizvod.DostupnaKolicina;
            trazeniProizvod.DostupnaKolicina = staraKolicina - kolicina;
            await Context.SaveChangesAsync();
            return Ok("Prodat je proizvod \"" + trazeniProizvod.Naziv + "\" sa ID: " + trazeniProizvod.Id
                    + "\nUkupno prodato: " + kolicina
                    + "\nStara kolicina: " + staraKolicina
                    + "\nNova kolicina: " + trazeniProizvod.DostupnaKolicina);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("DodajZaposlenog/{imeZaposlenog}/{prezimeZaposlenog}/{emailZaposlenog}/{jmbg}/{slikaZaposlenog}/{idProdavnice}/{tipZaposlenog}/{idVodjeSmene}/{idSmene}")]
    public async Task<ActionResult> DodajZaposlenog(string imeZaposlenog, string prezimeZaposlenog, string emailZaposlenog,
                                         string jmbg, string slikaZaposlenog, int idProdavnice, string tipZaposlenog, int idVodjeSmene, int idSmene)
    {
        try
        {
            var trazenaProdavnica = await Context.Prodavnice.Where(p => p.Id == idProdavnice)
                                                            .FirstOrDefaultAsync();
            if (trazenaProdavnica == null)
            {
                return BadRequest("Uneta prodavnica ne postoji");
            }
            var trazenaSmena = await Context.Smene.Where(s => s.Id == idSmene)
                                                  .FirstOrDefaultAsync();
            if (trazenaSmena == null && tipZaposlenog == "VodjaSmene")
            {
                return BadRequest("Uneta smena ne postoji");
            }

            if (tipZaposlenog == "VodjaSmene")
            {
                var noviZaposleni = new VodjaSmene{
                    Ime = imeZaposlenog,
                    Prezime = prezimeZaposlenog,
                    Email = emailZaposlenog,
                    JMBG = jmbg,
                    slika = slikaZaposlenog,
                    prodavnica = trazenaProdavnica,
                    smena = trazenaSmena!,
                    Prodavci = []
                };  

                await Context.Zaposleni.AddAsync(noviZaposleni);
                await Context.SaveChangesAsync();
                return Ok($"Dodat je novi vodja smene {noviZaposleni.Ime} {noviZaposleni.Prezime} u prodavnicu {noviZaposleni.prodavnica.Naziv}");
            }
            else if (tipZaposlenog == "Prodavac")
            {
                var trazeniVodjaSmene = await Context.Zaposleni.Where(z => z.Id == idVodjeSmene)
                                                            .FirstOrDefaultAsync();
                if (trazeniVodjaSmene == null)
                {
                    return BadRequest("Uneti vodja smene ne postoji");
                }

                var noviZaposleni = new Prodavac{
                    Ime = imeZaposlenog,
                    Prezime = prezimeZaposlenog,
                    Email = emailZaposlenog,
                    JMBG = jmbg,
                    slika = slikaZaposlenog,
                    prodavnica = trazenaProdavnica,
                    ukupnaCenaProdatihProizvoda = 0,
                    mesecniBonus = 0,
                    vodjaSmene = (VodjaSmene)trazeniVodjaSmene
                };

                await Context.Zaposleni.AddAsync(noviZaposleni);
                await Context.SaveChangesAsync();
                return Ok($"Dodat je novi prodavac {noviZaposleni.Ime} {noviZaposleni.Prezime} u prodavnicu {noviZaposleni.prodavnica.Naziv}");
            }
            else
            {
                return BadRequest("Uneti tip zaposlenog ne postoji");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("DodajSmenu/{nazivSmene}/{pocetakSmene}/{krajSmene}")]
    public async Task<ActionResult> DodajSmenu(string nazivSmene, DateTime pocetakSmene, DateTime krajSmene)
    {
        try
        {
            var novaSmena = new Smena{
                nazivSmene = nazivSmene,
                pocetakSmene = TimeOnly.FromDateTime(pocetakSmene),
                krajSmene = TimeOnly.FromDateTime(krajSmene),
                vodjeSmene = null
            };

            await Context.Smene.AddAsync(novaSmena);
            await Context.SaveChangesAsync();
            return Ok($"Dodata je nova smena {nazivSmene} koja pocinje u {pocetakSmene}, a zavrsava se u {krajSmene}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("VratiZaposleneProdavnice/{idProdavnice}")]
    public async Task<ActionResult> VratiZaposleneProdavnice(int idProdavnice)
    {
        try
        {
            var sviZaposleni = await Context.Zaposleni.Where(z => z.prodavnica.Id == idProdavnice)
                                                .Select(z => new{
                                                    imeZaposlenog = z.Ime,
                                                    prezimeZaposlenog = z.Prezime,
                                                    emailZaposlenog = z.Email,
                                                    jmbgZaposlenog = z.JMBG,
                                                    slikaZaposlenog = z.slika
                                                }).ToListAsync();
            return Ok(sviZaposleni);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("ObrisiZaposlenog/{idZaposlenog}")]
    public async Task<ActionResult> ObrisiZaposlenog(int idZaposlenog)
    {
        try
        {
            var trazeniZaposleni = await Context.Zaposleni.FindAsync(idZaposlenog);
            if (trazeniZaposleni == null)
            {
                return BadRequest("Uneti zaposleni ne postoji!");
            }

            Context.Zaposleni.Remove(trazeniZaposleni);
            await Context.SaveChangesAsync();
            return Ok("Uspesno je obrisan zaposleni");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("VratiZaposlenogSaNajvecomProdajom/{idVodjeSmene}")]
    public async Task<ActionResult> VratiZaposlenogSaNajvecomProdajom(int idVodjeSmene)
    {
        try
        {
            Prodavac ?trazeniProdavac = await Context.Zaposleni
                                                .OfType<Prodavac>()
                                                .Where(p => p.vodjaSmene!.Id == idVodjeSmene)
                                                .OrderByDescending(p => p.ukupnaCenaProdatihProizvoda)
                                                .FirstOrDefaultAsync();
            if (trazeniProdavac == null)
            {
                return BadRequest("Ne postoji ni jedan prodavac ovog vodje smene");
            }
            return Ok(trazeniProdavac);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("ResetujZaradeProdavaca/{idVodjeSmene}")]
    public async Task<ActionResult> ResetujZaradeProdavaca(int idVodjeSmene)
    {
        try
        {
            List<Prodavac> trazeniProdavci = await Context.Zaposleni
                                                    .OfType<Prodavac>()
                                                    .Where(p => p.vodjaSmene!.Id == idVodjeSmene)
                                                    .ToListAsync();
            foreach (Prodavac p in trazeniProdavci)
            {
                p.ukupnaCenaProdatihProizvoda = 0;
            }
            await Context.SaveChangesAsync();
            return Ok(trazeniProdavci);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("DodajBonusProdavcu/{idProdavca}")]
    public async Task<ActionResult> DodajBonusProdavcu(int idProdavca)
    {
        try
        {
            Prodavac ?trazeniProdavac = await Context.Zaposleni
                                                .OfType<Prodavac>()
                                                .Where(p => p.Id == idProdavca)
                                                .FirstOrDefaultAsync();
            if (trazeniProdavac == null)
            {
                return BadRequest("Trazeni prodavac ne postoji");
            }
            trazeniProdavac.mesecniBonus = trazeniProdavac.ukupnaCenaProdatihProizvoda*0.1;
            await Context.SaveChangesAsync();
            return Ok($"Bonus prodavca {trazeniProdavac.Ime} {trazeniProdavac.Prezime} je {trazeniProdavac.mesecniBonus}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("DodajProdajuProdavcu/{idProdavca}/{cenaProdaje}")]
    public async Task<ActionResult> DodajProdajuProdavcu(int idProdavca, double cenaProdaje)
    {
        try
        {
            Prodavac ?trazeniProdavac = await Context.Zaposleni
                                                .OfType<Prodavac>()
                                                .Where(p => p.Id == idProdavca)
                                                .FirstOrDefaultAsync();
            if (trazeniProdavac == null)
            {
                return BadRequest("Trazeni prodavac ne postoji");
            }
            trazeniProdavac.ukupnaCenaProdatihProizvoda += cenaProdaje;
            await Context.SaveChangesAsync();
            return Ok($"Ukupna prodaja prodavca {trazeniProdavac.Ime} {trazeniProdavac.Prezime} je {trazeniProdavac.ukupnaCenaProdatihProizvoda}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

