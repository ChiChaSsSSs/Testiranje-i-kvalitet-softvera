using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTemplate.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prodavnice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lokacija = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojTelefona = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodavnice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Smene",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazivSmene = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pocetakSmene = table.Column<TimeOnly>(type: "time", nullable: false),
                    krajSmene = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Smene", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proizvodi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kategorija = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cena = table.Column<double>(type: "float", nullable: false),
                    DostupnaKolicina = table.Column<int>(type: "int", nullable: false),
                    ProdavnicaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proizvodi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proizvodi_Prodavnice_ProdavnicaId",
                        column: x => x.ProdavnicaId,
                        principalTable: "Prodavnice",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Zaposleni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JMBG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slika = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prodavnicaId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    ukupnaCenaProdatihProizvoda = table.Column<double>(type: "float", nullable: true),
                    mesecniBonus = table.Column<double>(type: "float", nullable: true),
                    vodjaSmeneId = table.Column<int>(type: "int", nullable: true),
                    smenaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zaposleni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zaposleni_Prodavnice_prodavnicaId",
                        column: x => x.prodavnicaId,
                        principalTable: "Prodavnice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zaposleni_Smene_smenaId",
                        column: x => x.smenaId,
                        principalTable: "Smene",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zaposleni_Zaposleni_vodjaSmeneId",
                        column: x => x.vodjaSmeneId,
                        principalTable: "Zaposleni",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proizvodi_ProdavnicaId",
                table: "Proizvodi",
                column: "ProdavnicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_prodavnicaId",
                table: "Zaposleni",
                column: "prodavnicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_smenaId",
                table: "Zaposleni",
                column: "smenaId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_vodjaSmeneId",
                table: "Zaposleni",
                column: "vodjaSmeneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proizvodi");

            migrationBuilder.DropTable(
                name: "Zaposleni");

            migrationBuilder.DropTable(
                name: "Prodavnice");

            migrationBuilder.DropTable(
                name: "Smene");
        }
    }
}
