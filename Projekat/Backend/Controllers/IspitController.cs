using System.Linq.Expressions;

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
}
