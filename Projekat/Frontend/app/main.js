import { Aplikacija } from "./aplikacija.js";

const prodavnice = await fetch("https://localhost:7080/Ispit/VratiProdavnice").then(response => response.json());
const aplikacija = new Aplikacija(prodavnice);
aplikacija.pokreniAplikaciju(document.body);

