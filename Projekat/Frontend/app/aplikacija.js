export class Aplikacija{

    constructor(prodavnice){
        this.prodavnice = prodavnice;
        this.nizLabela = ["Naziv:","Kategorija:","Cena:","Kolicina:"];
        this.kategorije = ["Knjiga","Igracka","Pribor","Ostalo"];
        this.boje = ["#1ecbe1","#a35d52","#6718e7","#e11ec6","#20df3b","#906996","#7a8583","#ffa900"];
    }
        
    pokreniAplikaciju(container){
        document.body.innerHTML = "";

        const celaAplikacija = document.createElement("div");
        celaAplikacija.classList.add("celaAplikacija");
        container.appendChild(celaAplikacija);

        this.prodavnice.forEach(p => {
            let jednaProdavnica = document.createElement("div");
            jednaProdavnica.classList.add("jednaProdavnica");
            celaAplikacija.append(jednaProdavnica);

            this.nacrtajFormu(jednaProdavnica, p);
            this.nacrtajProizvode(jednaProdavnica, p);
        });
    }

    nacrtajFormu(container, p){
        const forma = document.createElement("div");
        forma.classList.add("celaForma");
        container.appendChild(forma);

        const divZaLabelu = document.createElement("div");
        divZaLabelu.classList.add("divZaLabelu");
        forma.appendChild(divZaLabelu);
        const lbl = document.createElement("label");
        lbl.innerHTML = "Upis proizvoda";
        divZaLabelu.appendChild(lbl);

        const divZaPodatke = document.createElement("div");
        divZaPodatke.classList.add("divZaPodatke");
        forma.appendChild(divZaPodatke);
        this.nizLabela.forEach((element, index) => {
            let lblPodataka = document.createElement("label");
            lblPodataka.innerHTML = element;
            divZaPodatke.appendChild(lblPodataka);
            let poljeZaPodatke;

            if (index == 1)
            {
                poljeZaPodatke = document.createElement("select");
                this.kategorije.forEach(element => {
                    let opcija = document.createElement("option");
                    opcija.value = element;
                    opcija.innerHTML = element;
                    poljeZaPodatke.appendChild(opcija);
                });
            }
            else
            {
                poljeZaPodatke = document.createElement("input");
                if (index != 0)
                {
                    poljeZaPodatke.type = "number";
                    poljeZaPodatke.min = "0";
                }
            }
            divZaPodatke.appendChild(poljeZaPodatke);
        });

        const dugme = document.createElement("button");
        dugme.classList.add("zaDugme");
        dugme.innerHTML = "Dodaj proizvod";
        dugme.value = p.id;
        dugme.onclick = async () => {
            await this.dodajProizvod(dugme);
        }
        divZaPodatke.appendChild(dugme);
    }

    async nacrtajProizvode(container, p){
        const listaProizvoda = await fetch("https://localhost:7080/Ispit/VratiProizvodeProdavnice/" + p.id)
                                        .then(response => response.json());

        const zaProizvode = document.createElement("div");
        zaProizvode.classList.add("zaProizvode");
        container.appendChild(zaProizvode);

        const divZaLabelu = document.createElement("div");
        divZaLabelu.classList.add("divZaLabelu");
        divZaLabelu.innerHTML = "Prodavnica: " + p.naziv;
        zaProizvode.appendChild(divZaLabelu);

        const divZaProizvode = document.createElement("div");
        divZaProizvode.classList.add("divZaProizvode");
        zaProizvode.appendChild(divZaProizvode);

        listaProizvoda.forEach((proizvod, index) => {
            let lbl = document.createElement("label");
            lbl.innerHTML = proizvod.naziv + ": " + proizvod.dostupno;
            lbl.style.gridColumnStart = "1";
            lbl.style.gridColumnEnd = "10";
            divZaProizvode.appendChild(lbl);

            let vizuelniPrikaz = document.createElement("div");
            vizuelniPrikaz.classList.add("zaVizuelno");
            vizuelniPrikaz.style.borderColor = this.boje[index % this.boje.length];
            divZaProizvode.appendChild(vizuelniPrikaz);
            let boja = document.createElement("div");
            boja.style.backgroundColor = this.boje[index % this.boje.length];
            boja.style.height = "100%";
            boja.style.width = proizvod.dostupno + "%";
            vizuelniPrikaz.appendChild(boja);

            let promenaKolicine = document.createElement("div");
            promenaKolicine.classList.add("promenaKolicine");
            divZaProizvode.appendChild(promenaKolicine);
            let lblKolicina = document.createElement("label");
            lblKolicina.innerHTML = "Kolicina:";
            promenaKolicine.appendChild(lblKolicina);
            let izborKolicine = document.createElement("input");
            izborKolicine.type = "number";
            izborKolicine.min = "0";
            promenaKolicine.appendChild(izborKolicine);
            let dugmeZaIzmenu = document.createElement("button");
            dugmeZaIzmenu.innerHTML = "Prodaj";
            dugmeZaIzmenu.value = proizvod.id;
            dugmeZaIzmenu.onclick = async() =>{
                await this.prodajProizvod(dugmeZaIzmenu, p.id);
            }
            promenaKolicine.appendChild(dugmeZaIzmenu);
        });
    }

    async dodajProizvod(dugme){
        const roditelj = dugme.parentElement;
        const vrednosti = roditelj.querySelectorAll("input");
        const db = roditelj.querySelector("select");

        await fetch("https://localhost:7080/Ispit/DodajProzivod/"
                  + dugme.value + "/" + vrednosti[0].value + "/"+ db.value + "/" + vrednosti[1].value + "/" + vrednosti[2].value,{
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                  });
        
        this.pokreniAplikaciju(document.body);
    }

    prodajProizvod = async(dugme, idProdavnice) =>{
        const roditelj = dugme.parentElement;
        const trazeni = roditelj.querySelector("input");

        const res = await fetch("https://localhost:7080/Ispit/ProdajProizvode/"
            + dugme.value + "/" + trazeni.value,{
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
            });

        this.pokreniAplikaciju(document.body);
    }
}