using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Newtonsoft.Json;
using WebTemplate;
using WebTemplate.Controllers;
using WebTemplate.Models;

namespace NUnitTests
{
    public class Tests
    {
        private IspitContext _context = GlobalSetup._context;
        private IspitController _controller = new IspitController(GlobalSetup._context);

        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Pokrece se test!");
        }

        // Funkcija DodajProdavnicu

        [TestCase(Description = "Provera dodavanja prodavnice", TestName = "DodajProdavnicu1")]
        public async Task TestA0()
        {
            Prodavnica novaProdavnica = new Prodavnica
            {
                Naziv = "Prodavnica1",
                Lokacija = "Nis",
                BrojTelefona = "0611231231",
                Proizvodi = new List<Proizvod>(),
                listaZaposlenih = new List<Zaposleni>()

            };
            await _controller.DodajProdavnicu(novaProdavnica);
            Prodavnica ?trazenaProdavnica = await _context.Prodavnice.FirstOrDefaultAsync(p => p.Naziv == novaProdavnica.Naziv);
            Assert.Multiple(() =>
            {
                Assert.That(trazenaProdavnica, Is.Not.Null);
                Assert.That(trazenaProdavnica?.Naziv, Is.EqualTo(novaProdavnica.Naziv));
                Assert.That(trazenaProdavnica?.Lokacija, Is.EqualTo(novaProdavnica.Lokacija));
                Assert.That(trazenaProdavnica?.BrojTelefona, Is.EqualTo(novaProdavnica.BrojTelefona));
                Assert.That(trazenaProdavnica?.Proizvodi, Is.EqualTo(novaProdavnica.Proizvodi));
                Assert.That(trazenaProdavnica?.listaZaposlenih, Is.EqualTo(novaProdavnica.listaZaposlenih));
            });
        }

        [TestCase(Description = "Provera dodavanja prodavnice", TestName = "DodajProdavnicu2")]
        public async Task TestA1()
        {
            Prodavnica novaProdavnica = new Prodavnica
            {
                Naziv = "Prodavnica2",
                Lokacija = "Beograd",
                BrojTelefona = "0651239998",
                Proizvodi = new List<Proizvod>(),
                listaZaposlenih = new List<Zaposleni>()

            };
            await _controller.DodajProdavnicu(novaProdavnica);
            Prodavnica? trazenaProdavnica = await _context.Prodavnice.FirstOrDefaultAsync(p => p.Naziv == novaProdavnica.Naziv);
            Assert.Multiple(() =>
            {
                Assert.That(trazenaProdavnica, Is.Not.Null);
                Assert.That(trazenaProdavnica?.Naziv, Is.EqualTo(novaProdavnica.Naziv));
                Assert.That(trazenaProdavnica?.Lokacija, Is.EqualTo(novaProdavnica.Lokacija));
                Assert.That(trazenaProdavnica?.BrojTelefona, Is.EqualTo(novaProdavnica.BrojTelefona));
                Assert.That(trazenaProdavnica?.Proizvodi, Is.EqualTo(novaProdavnica.Proizvodi));
                Assert.That(trazenaProdavnica?.listaZaposlenih, Is.EqualTo(novaProdavnica.listaZaposlenih));
            });
        }

        [TestCase(Description = "Provera dodavanja prodavnice", TestName = "DodajProdavnicu3")]
        public async Task TestA2()
        {
            Prodavnica novaProdavnica = new Prodavnica
            {
                Naziv = "Prodavnica3",
                Lokacija = "Prokuplje",
                BrojTelefona = "0625858977",
                Proizvodi = new List<Proizvod>(),
                listaZaposlenih = new List<Zaposleni>()

            };
            await _controller.DodajProdavnicu(novaProdavnica);
            Prodavnica? trazenaProdavnica = await _context.Prodavnice.FirstOrDefaultAsync(p => p.Naziv == novaProdavnica.Naziv);
            Assert.Multiple(() =>
            {
                Assert.That(trazenaProdavnica, Is.Not.Null);
                Assert.That(trazenaProdavnica?.Naziv, Is.EqualTo(novaProdavnica.Naziv));
                Assert.That(trazenaProdavnica?.Lokacija, Is.EqualTo(novaProdavnica.Lokacija));
                Assert.That(trazenaProdavnica?.BrojTelefona, Is.EqualTo(novaProdavnica.BrojTelefona));
                Assert.That(trazenaProdavnica?.Proizvodi, Is.EqualTo(novaProdavnica.Proizvodi));
                Assert.That(trazenaProdavnica?.listaZaposlenih, Is.EqualTo(novaProdavnica.listaZaposlenih));
            });
        }

        // Funkcija DodajProizvod

        [Test(Description = "Provera ispravnog dodavanja proizvoda")]
        [TestCase(1, "Matematika1", "Knjiga", 850, 50, TestName = "DodajProizvod1")]
        [TestCase(1, "SveskaA4", "Pribor", 200, 100, TestName = "DodajProizvod2")]
        [TestCase(2, "Solja", "Ostalo", 500, 40, TestName = "DodajProizvod3")]
        [TestCase(2, "Lopta", "Igracka", 1200, 30, TestName = "DodajProizvod4")]
        [TestCase(3, "Fizika", "Knjiga", 1000, 75, TestName = "DodajProizvod5")]
        [TestCase(3, "Lenjir", "Pribor", 100, 60, TestName = "DodajProizvod6")]
        public async Task TestA3(int idProdavnice, string nazivProizvoda, string kategorijaProizvoda, double cenaProizvoda, int kolicina)
        {
            await _controller.DodajProzivod(idProdavnice, nazivProizvoda, kategorijaProizvoda, cenaProizvoda, kolicina);
            Proizvod ?trazeniProizvod = await _context.Proizvodi.FirstOrDefaultAsync(p => p.Prodavnica!.Id == idProdavnice && p.Naziv == nazivProizvoda
                                                                    && p.Kategorija == kategorijaProizvoda && p.Cena == cenaProizvoda && p.DostupnaKolicina == kolicina);
            Assert.That(trazeniProizvod, Is.Not.Null);
        }

        [TestCase(Description = "Provera dodavanja proizvoda preko dozvoljene kolicine", TestName = "DodajProizvod7")]
        public async Task TestA4()
        {
            int idProdavnice = 1;
            string nazivProizvoda = "Blok5";
            string kategorijaProizvoda = "Pribor";
            double cenaProizvoda = 300;
            int kolicina = 120;
            var result = await _controller.DodajProzivod(idProdavnice, nazivProizvoda, kategorijaProizvoda, cenaProizvoda, kolicina);
            Assert.That((result as BadRequestObjectResult)?.Value, Is.EqualTo("Nije moguce dodati vise od 100 proizvoda!"));
        }

        [TestCase(Description = "Provera dodavanja proizvoda u nepostojecu prodavnicu", TestName = "DodajProizvod8")]
        public async Task TestA5()
        {
            int idProdavnice = 4;
            string nazivProizvoda = "Blok5";
            string kategorijaProizvoda = "Pribor";
            double cenaProizvoda = 300;
            int kolicina = 120;
            var result = await _controller.DodajProzivod(idProdavnice, nazivProizvoda, kategorijaProizvoda, cenaProizvoda, kolicina);
            Assert.That((result as BadRequestObjectResult)?.Value, Is.EqualTo("Uneta prodavnica ne postoji!"));
        }

        // Funkcija ObrisiProizvod

        [TestCase(Description = "Provera brisanja postojeceg prozivoda", TestName = "ObrisiProizvod1")]
        public async Task TestA6()
        {
            int idProdavnice = 3;
            string nazivProizvoda = "Lenjir";
            string kategorijaProizvoda = "Pribor";
            double cenaProizvoda = 100;
            int kolicina = 60;
            await _controller.ObrisiProizvod(idProdavnice, nazivProizvoda, kategorijaProizvoda, cenaProizvoda, kolicina);
            Proizvod? trazeniProizvod = await _context.Proizvodi.FirstOrDefaultAsync(p => p.Prodavnica!.Id == idProdavnice && p.Naziv == nazivProizvoda
                                                                    && p.Kategorija == kategorijaProizvoda && p.Cena == cenaProizvoda && p.DostupnaKolicina == kolicina);
            Assert.That(trazeniProizvod, Is.Null);
        }

        [TestCase(Description = "Provera brisanja nepostojeceg prozivoda", TestName = "ObrisiProizvod2")]
        public async Task TestA7()
        {
            int idProdavnice = 3;
            string nazivProizvoda = "Lenjir";
            string kategorijaProizvoda = "Pribor";
            double cenaProizvoda = 100;
            int kolicina = 60;
            var result = await _controller.ObrisiProizvod(idProdavnice, nazivProizvoda, kategorijaProizvoda, cenaProizvoda, kolicina);
            Assert.That((result as BadRequestObjectResult)?.Value, Is.EqualTo("Proizvod ne postoji u prodavnici"));
        }

        // Funkcija VratiProdavnice

        [Test(Description = "Provera da li funkcija vraca sve prodavnice")]
        [TestCase("1", "Prodavnica1", TestName = "VratiProdavnice1")]
        [TestCase("2", "Prodavnica2", TestName = "VratiProdavnice2")]
        [TestCase("3", "Prodavnica3", TestName = "VratiProdavnice3")]

        public async Task TestA8(string idProdavnice, string nazivProdavnice)
        {
            var result = await _controller.VratiProdavnice();
            var okResult = result as OkObjectResult;
            var lista = okResult!.Value;
            Assert.That(lista, Has.Some.Matches<object>(el =>
            {
                return el.GetType().GetProperty("id")?.GetValue(el, null)?.ToString() == idProdavnice &&
                       el.GetType().GetProperty("naziv")?.GetValue(el, null)?.ToString() == nazivProdavnice;
            }));
        }

        [TestCase(Description = "Provera da li funkcija vraca objekte odgovarajuceg tipa", TestName = "VratiProdavnice4")]
        public async Task TestA9()
        {
            var result = await _controller.VratiProdavnice();
            var okResult = result as OkObjectResult;
            var lista = okResult!.Value;
            Assert.That(lista, Is.All.Matches<object>(el =>
            {
                var elType = el.GetType();
                return elType.GetProperty("id") != null &&
                        elType.GetProperty("naziv") != null;
            }
            ));
        }

        [TestCase(Description = "Provera da li funkcija vraca tip Proizvod", TestName = "VratiProdavnice5")]
        public async Task TestB0()
        {
            var result = await _controller.VratiProizvodeProdavnice(1);
            var okResult = result as OkObjectResult;
            var trazeniProizvodi = okResult!.Value as List<Proizvod>;
            Assert.That(trazeniProizvodi, Is.All.TypeOf<Proizvod>());
        }

        // Funkcija VratiProizvodeProdavnice

        [TestCase(Description = "Provera da li funkcija vraca tacan broj proizvoda", TestName = "VratiProizvodeProdavnice1")]
        public async Task TestB1()
        {
            var result = await _controller.VratiProizvodeProdavnice(3);
            var okResult = result as OkObjectResult;
            var trazeniProizvodi = okResult!.Value as List<Proizvod>;
            Assert.That(trazeniProizvodi!.Count, Is.EqualTo(1));
        }

        [Test(Description = "Provera da li funkcija vraca tacne proizvode")]
        [TestCase("Solja", "Ostalo", "500", "40", TestName = "VratiProizvodeProdavnice2")]
        [TestCase("Lopta", "Igracka", "1200", "30", TestName = "VratiProizvodeProdavnice3")]
        public async Task TestB2(string naziv, string kategorija, string cena, string kolicina)
        {
            var result = await _controller.VratiProizvodeProdavnice(2);
            var okResult = result as OkObjectResult;
            var trazeniProizvodi = okResult!.Value as List<Proizvod>;
            Assert.That(trazeniProizvodi, Has.Some.Matches<object>(el =>
            {
                return el.GetType().GetProperty("Naziv")?.GetValue(el, null)?.ToString() == naziv &&
                        el.GetType().GetProperty("Kategorija")?.GetValue(el, null)?.ToString() == kategorija &&
                        el.GetType().GetProperty("Cena")?.GetValue(el, null)?.ToString() == cena &&
                        el.GetType().GetProperty("DostupnaKolicina")?.GetValue(el, null)?.ToString() == kolicina;
            }));
        }

        // Funkcija ProdajProizvode

        [TestCase(Description = "Provera da li je proizvod dobro prodat", TestName = "ProdajProizvode1")]
        public async Task TestB3()
        {
            await _controller.ProdajProizvode(1, 30);
            Proizvod ?trazeniProizvod = await _context.Proizvodi.FirstOrDefaultAsync(p => p.Id == 1);
            Assert.That(trazeniProizvod!.DostupnaKolicina, Is.EqualTo(20));
        }

        [TestCase(Description = "Provera da li nece ostati negativan broj proizvoda", TestName = "ProdajProizvode2")]
        public async Task TestB4()
        {
            await _controller.ProdajProizvode(1, 30);
            Proizvod? trazeniProizvod = await _context.Proizvodi.FirstOrDefaultAsync(p => p.Id == 1);
            Assert.That(trazeniProizvod!.DostupnaKolicina, Is.AtLeast(0));
        }

        // Funkcija DodajSmenu

        [TestCase(Description = "Provera da li je moguce dodati smenu", TestName = "DodajSmenu1")]
        public async Task TestB5()
        {
            DateTime pocetakSmene = new DateTime(2024, 12, 20, 6, 0, 0);
            DateTime krajSmene = new DateTime(2024, 12, 20, 14, 0, 0);

            var result = await _controller.DodajSmenu("Jutarnja", pocetakSmene, krajSmene);
            var okResult = result as OkObjectResult;
            string? resString = okResult!.Value as string;
            Assert.That(resString!.Contains("6:00:00 AM") && resString.Contains("2:00:00 PM"));
        }

        // Funkcija DodajZaposlenog

        [Test(Description = "Provera da li je moguce dodati zaposlenog u prodavnicu koja ne postoji")]
        [TestCase("Marko", "Markovic", "markomarkovic@gmail.com", "1234567891234", "slika.png", 4, "VodjaSmene", 0, 1, TestName = "DodajZaposlenog1")]
        public async Task TestB6(string ime, string prezime, string email, string jmbg, string slika, int prodavnica, string tip, int vodja, int smena)
        {
            var result = await _controller.DodajZaposlenog(ime, prezime, email, jmbg, slika, prodavnica, tip, vodja, smena);
            Assert.That((result as BadRequestObjectResult)?.Value, Is.EqualTo("Uneta prodavnica ne postoji"));
        }

        [Test(Description = "Provera da li je moguce dodati zaposlenog u smenu koja ne postoji")]
        [TestCase("Marko", "Markovic", "markomarkovic@gmail.com", "1234567891234", "slika.png", 1, "VodjaSmene", 0, 2, TestName = "DodajZaposlenog2")]
        public async Task TestB7(string ime, string prezime, string email, string jmbg, string slika, int prodavnica, string tip, int vodja, int smena)
        {
            var result = await _controller.DodajZaposlenog(ime, prezime, email, jmbg, slika, prodavnica, tip, vodja, smena);
            Assert.That((result as BadRequestObjectResult)?.Value, Is.EqualTo("Uneta smena ne postoji"));
        }

        [Test(Description = "Provera da li je moguce dodati vodje smena u razlicite prodavnice")]
        [TestCase("Marko", "Markovic", "markomarkovic@gmail.com", "1234567891234", "slika1.png", 1, "VodjaSmene", 0, 1, TestName = "DodajZaposlenog3")]
        [TestCase("Janko", "Jankovic", "jankojankovic@gmail.com", "1234567891235", "slika2.png", 2, "VodjaSmene", 0, 1, TestName = "DodajZaposlenog4")]
        [TestCase("Petar", "Petrovic", "petarpetrovic@gmail.com", "1234567891236", "slika3.png", 3, "VodjaSmene", 0, 1, TestName = "DodajZaposlenog5")]
        public async Task TestB8(string ime, string prezime, string email, string jmbg, string slika, int prodavnica, string tip, int vodja, int smena)
        {
            var result = await _controller.DodajZaposlenog(ime, prezime, email, jmbg, slika, prodavnica, tip, vodja, smena);
            var okResult = result as OkObjectResult;
            string? resString = okResult!.Value as string;
            Assert.That(resString!.Contains(ime) && resString.Contains(prezime));
        }

        [Test(Description = "Provera da li je moguce dodati prodavca koji nema vodju smene")]
        [TestCase("Zivko", "Zivkovic", "zivkozivkovic@gmail.com", "5555555555555", "selfie.png", 1, "Prodavac", 4, 1, TestName = "DodajZaposlenog6")]
        public async Task TestB9(string ime, string prezime, string email, string jmbg, string slika, int prodavnica, string tip, int vodja, int smena)
        {
            var result = await _controller.DodajZaposlenog(ime, prezime, email, jmbg, slika, prodavnica, tip, vodja, smena);
            Assert.That((result as BadRequestObjectResult)?.Value, Is.EqualTo("Uneti vodja smene ne postoji"));
        }

        [Test(Description = "Provera da li je moguce dodati prodavce u razlicite prodavnice")]
        [TestCase("Zivko", "Zivkovic", "zivkozivkovic@gmail.com", "5555555555555", "prodavac1.png", 1, "Prodavac", 1, 1, TestName = "DodajZaposlenog7")]
        [TestCase("Pavle", "Pavlovic", "pavlepavlovic@gmail.com", "3333333333333", "prodavac2.png", 1, "Prodavac", 1, 1, TestName = "DodajZaposlenog8")]
        [TestCase("Milos", "Milosevic", "milosmilosevic@gmail.com", "1111111111111", "prodavac3.png", 2, "Prodavac", 2, 1, TestName = "DodajZaposlenog9")]
        [TestCase("Andrija", "Antic", "andrijaantic@gmail.com", "2222222222222", "prodavac4.png", 2, "Prodavac", 2, 1, TestName = "DodajZaposlenog10")]
        [TestCase("Petar", "Milenkovic", "petarmilenkovic@gmail.com", "4444444444444", "prodavac5.png", 3, "Prodavac", 3, 1, TestName = "DodajZaposlenog11")]
        [TestCase("Goran", "Petrovic", "goranpetrovic@gmail.com", "9999999999999", "prodavac6.png", 3, "Prodavac", 3, 1, TestName = "DodajZaposlenog12")]
        public async Task TestC0(string ime, string prezime, string email, string jmbg, string slika, int prodavnica, string tip, int vodja, int smena)
        {
            var result = await _controller.DodajZaposlenog(ime, prezime, email, jmbg, slika, prodavnica, tip, vodja, smena);
            var okResult = result as OkObjectResult;
            string? resString = okResult!.Value as string;
            Assert.That(resString!.Contains(ime) && resString.Contains(prezime));
        }

        // Funkcija VratiZaposleneProdavnice

        [Test(Description = "Provera da li funkcija vraca sve zaposlene")]
        [TestCase("Marko", "Markovic", "markomarkovic@gmail.com", "1234567891234", "slika1.png", 1, TestName = "VratiZaposleneProdavnice1")]
        [TestCase("Janko", "Jankovic", "jankojankovic@gmail.com", "1234567891235", "slika2.png", 2, TestName = "VratiZaposleneProdavnice2")]
        [TestCase("Petar", "Petrovic", "petarpetrovic@gmail.com", "1234567891236", "slika3.png", 3, TestName = "VratiZaposleneProdavnice3")]
        [TestCase("Zivko", "Zivkovic", "zivkozivkovic@gmail.com", "5555555555555", "prodavac1.png", 1, TestName = "VratiZaposleneProdavnice4")]
        [TestCase("Pavle", "Pavlovic", "pavlepavlovic@gmail.com", "3333333333333", "prodavac2.png", 1, TestName = "VratiZaposleneProdavnice5")]
        [TestCase("Milos", "Milosevic", "milosmilosevic@gmail.com", "1111111111111", "prodavac3.png", 2, TestName = "VratiZaposleneProdavnice6")]
        [TestCase("Andrija", "Antic", "andrijaantic@gmail.com", "2222222222222", "prodavac4.png", 2, TestName = "VratiZaposleneProdavnice7")]
        [TestCase("Petar", "Milenkovic", "petarmilenkovic@gmail.com", "4444444444444", "prodavac5.png", 3, TestName = "VratiZaposleneProdavnice8")]
        [TestCase("Goran", "Petrovic", "goranpetrovic@gmail.com", "9999999999999", "prodavac6.png", 3, TestName = "VratiZaposleneProdavnice9")]
        public async Task TestC1(string ime, string prezime, string email, string jmbg, string slika, int idProdavnice)
        {
            var result = await _controller.VratiZaposleneProdavnice(idProdavnice);
            var okResult = result as OkObjectResult;
            var lista = okResult!.Value;
            Assert.That(lista, Has.Some.Matches<object>(el =>
            {
                return el.GetType().GetProperty("imeZaposlenog")?.GetValue(el, null)?.ToString() == ime &&
                       el.GetType().GetProperty("prezimeZaposlenog")?.GetValue(el, null)?.ToString() == prezime &&
                       el.GetType().GetProperty("emailZaposlenog")?.GetValue(el, null)?.ToString() == email &&
                       el.GetType().GetProperty("jmbgZaposlenog")?.GetValue(el, null)?.ToString() == jmbg &&
                       el.GetType().GetProperty("slikaZaposlenog")?.GetValue(el, null)?.ToString() == slika;
            }));
        }

        // Funkcija DodajProdajuProdavcu

        [Test(Description = "Provera da li je moguce prodavcima dodati prodaju")]
        [TestCase(4, 1000, TestName = "DodajProdajuProdavcu1")]
        [TestCase(5, 2000, TestName = "DodajProdajuProdavcu2")]
        [TestCase(6, 3000, TestName = "DodajProdajuProdavcu3")]
        [TestCase(7, 4000, TestName = "DodajProdajuProdavcu4")]
        [TestCase(8, 5000, TestName = "DodajProdajuProdavcu5")]
        [TestCase(9, 6000, TestName = "DodajProdajuProdavcu6")]
        public async Task TestC2(int idProdavca, double cenaProdaje)
        {
            var result = await _controller.DodajProdajuProdavcu(idProdavca, cenaProdaje);
            var okResult = result as OkObjectResult;
            string? resString = okResult!.Value as string;
            Assert.That(resString!.Contains(cenaProdaje.ToString()));
        }

        [Test(Description = "Provera da li je moguce vodjama smene dodati prodaju")]
        [TestCase(1, 1000, TestName = "DodajProdajuProdavcu7")]
        [TestCase(2, 2000, TestName = "DodajProdajuProdavcu8")]
        [TestCase(3, 3000, TestName = "DodajProdajuProdavcu9")]
        public async Task TestC3(int idProdavca, double cenaProdaje)
        {
            var result = await _controller.DodajProdajuProdavcu(idProdavca, cenaProdaje);
            var okResult = result as BadRequestObjectResult;
            string? resString = okResult!.Value as string;
            Assert.That(resString, Is.EqualTo("Trazeni prodavac ne postoji"));
        }

        // Funkcija VratiZaposlenogSaNajvecomProdajom
        [TestCase(Description = "Provera sa Id-em vodje smene koji ne postoji", TestName = "VratiZaposlenogSaNajvecomProdajom1")]
        public async Task TestC4()
        {
            var result = await _controller.VratiZaposlenogSaNajvecomProdajom(4);
            var okResult = result as BadRequestObjectResult;
            string? resString = okResult!.Value as string;
            Assert.That(resString, Is.EqualTo("Ne postoji ni jedan prodavac ovog vodje smene"));
        }

        [Test(Description = "Provera sa da li funkcija vraca zaposlene sa najvecom prodajom za svakog vodju smene")]
        [TestCase(1, "Pavle", "Pavlovic", TestName = "VratiZaposlenogSaNajvecomProdajom2")]
        [TestCase(2, "Andrija", "Antic", TestName = "VratiZaposlenogSaNajvecomProdajom3")]
        [TestCase(3, "Goran", "Petrovic", TestName = "VratiZaposlenogSaNajvecomProdajom4")]
        public async Task TestC5(int idVodjeSmene, string imeProdavca, string prezimeProdavca)
        {
            var result = await _controller.VratiZaposlenogSaNajvecomProdajom(idVodjeSmene);
            var okResult = result as OkObjectResult;
            Prodavac ?trazeniProdavac = okResult!.Value as Prodavac;
            Assert.That(trazeniProdavac!.Ime + trazeniProdavac.Prezime, Is.EqualTo(imeProdavca + prezimeProdavca));
        }

        // Funkcija DodajBonusProdavcu
        [Test(Description = "Provera da li je moguce vodjama smene dodati bonus")]
        [TestCase(1, TestName = "DodajBonusProdavcu1")]
        [TestCase(2, TestName = "DodajBonusProdavcu2")]
        [TestCase(3, TestName = "DodajBonusProdavcu3")]
        public async Task TestC6(int idProdavca)
        {
            var result = await _controller.DodajBonusProdavcu(idProdavca);
            var badResult = result as BadRequestObjectResult;
            string? resString = badResult!.Value as string;
            Assert.That(resString, Is.EqualTo("Trazeni prodavac ne postoji"));
        }

        [Test(Description = "Provera da li je moguce prodavcima dodati bonus")]
        [TestCase(5, 2000, TestName = "DodajBonusProdavcu4")]
        [TestCase(7, 4000, TestName = "DodajBonusProdavcu5")]
        [TestCase(9, 6000, TestName = "DodajBonusProdavcu6")]
        public async Task TestC7(int idProdavca, double trenutnaProdaja)
        {
            var result = await _controller.DodajBonusProdavcu(idProdavca);
            var okResult = result as OkObjectResult;
            string? resString = okResult!.Value as string;
            Assert.That(resString!.Contains((trenutnaProdaja * 0.1).ToString()));
        }

        // Funkcija ResetujZaradeProdavaca
        [Test(Description = "Provera da li se zarade prodavcima pravilno resetuju")]
        [TestCase(1, TestName = "ResetujZaradeProdavaca1")]
        [TestCase(2, TestName = "ResetujZaradeProdavaca2")]
        [TestCase(3, TestName = "ResetujZaradeProdavaca3")]
        public async Task TestC8(int idVodjeSmene)
        {
            var result = await _controller.ResetujZaradeProdavaca(idVodjeSmene);
            var okResult = result as OkObjectResult;
            List<Prodavac> ?resList = okResult!.Value as List<Prodavac>;
            foreach (Prodavac p in resList!)
                Assert.That(p.ukupnaCenaProdatihProizvoda, Is.EqualTo(0));
        }

        // Funkcija ObrisiZaposlenog
        [TestCase(Description = "Provera da je moguce obrisati zaposlenog koji ne postoji", TestName = "ObrisiZaposlenog1")]
        public async Task TestC9()
        {
            var result = await _controller.ObrisiZaposlenog(10);
            var badResult = result as BadRequestObjectResult;
            string? resString = badResult!.Value as string;
            Assert.That(resString, Is.EqualTo("Uneti zaposleni ne postoji!"));
        }

        [Test(Description = "Provera da li je moguce prodavcima dodati bonus")]
        [TestCase(5, TestName = "ObrisiZaposlenog2")]
        [TestCase(7, TestName = "ObrisiZaposlenog3")]
        [TestCase(9, TestName = "ObrisiZaposlenog4")]
        public async Task TestD0(int idZaposlenog)
        {
            await _controller.ObrisiZaposlenog(idZaposlenog);
            var trazeniZaposleni = await _context.Zaposleni.FindAsync(idZaposlenog);
            Assert.That(trazeniZaposleni, Is.Null);
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("Kraj testa!");
        }
    }
}