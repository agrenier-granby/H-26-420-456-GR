using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Exercices_Formulaire_Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Budget = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Superficie = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    DateEmbauche = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalaireAnnuel = table.Column<double>(type: "float", nullable: false),
                    EstEnEmploi = table.Column<bool>(type: "bit", nullable: false),
                    PaysId = table.Column<int>(type: "int", nullable: false),
                    DepartementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employes_Departements_DepartementId",
                        column: x => x.DepartementId,
                        principalTable: "Departements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employes_Pays_PaysId",
                        column: x => x.PaysId,
                        principalTable: "Pays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Pays",
                columns: new[] { "Id", "Nom", "Superficie" },
                values: new object[,]
                {
                    { 1, "Afghanistan", 0 },
                    { 2, "Albanie", 0 },
                    { 3, "Antarctique", 0 },
                    { 4, "Algérie", 0 },
                    { 5, "Samoa Américaines", 0 },
                    { 6, "Andorre", 0 },
                    { 7, "Angola", 0 },
                    { 8, "Antigua-et-Barbuda", 0 },
                    { 9, "Azerbaïdjan", 0 },
                    { 10, "Argentine", 0 },
                    { 11, "Australie", 0 },
                    { 12, "Autriche", 0 },
                    { 13, "Bahamas", 0 },
                    { 14, "Bahreïn", 0 },
                    { 15, "Bangladesh", 0 },
                    { 16, "Arménie", 0 },
                    { 17, "Barbade", 0 },
                    { 18, "Belgique", 0 },
                    { 19, "Bermudes", 0 },
                    { 20, "Bhoutan", 0 },
                    { 21, "Bolivie", 0 },
                    { 22, "Bosnie-Herzégovine", 0 },
                    { 23, "Botswana", 0 },
                    { 24, "Île Bouvet", 0 },
                    { 25, "Brésil", 0 },
                    { 26, "Belize", 0 },
                    { 27, "Territoire Britannique de l'Océan Indien", 0 },
                    { 28, "Îles Salomon", 0 },
                    { 29, "Îles Vierges Britanniques", 0 },
                    { 30, "Brunéi Darussalam", 0 },
                    { 31, "Bulgarie", 0 },
                    { 32, "Myanmar", 0 },
                    { 33, "Burundi", 0 },
                    { 34, "Bélarus", 0 },
                    { 35, "Cambodge", 0 },
                    { 36, "Cameroun", 0 },
                    { 37, "Canada", 0 },
                    { 38, "Cap-vert", 0 },
                    { 39, "Îles Caïmanes", 0 },
                    { 40, "République Centrafricaine", 0 },
                    { 41, "Sri Lanka", 0 },
                    { 42, "Tchad", 0 },
                    { 43, "Chili", 0 },
                    { 44, "Chine", 0 },
                    { 45, "Taïwan", 0 },
                    { 46, "Île Christmas", 0 },
                    { 47, "Îles Cocos (Keeling)", 0 },
                    { 48, "Colombie", 0 },
                    { 49, "Comores", 0 },
                    { 50, "Mayotte", 0 },
                    { 51, "République du Congo", 0 },
                    { 52, "République Démocratique du Congo", 0 },
                    { 53, "Îles Cook", 0 },
                    { 54, "Costa Rica", 0 },
                    { 55, "Croatie", 0 },
                    { 56, "Cuba", 0 },
                    { 57, "Chypre", 0 },
                    { 58, "République Tchèque", 0 },
                    { 59, "Bénin", 0 },
                    { 60, "Danemark", 0 },
                    { 61, "Dominique", 0 },
                    { 62, "République Dominicaine", 0 },
                    { 63, "Équateur", 0 },
                    { 64, "El Salvador", 0 },
                    { 65, "Guinée Équatoriale", 0 },
                    { 66, "Éthiopie", 0 },
                    { 67, "Érythrée", 0 },
                    { 68, "Estonie", 0 },
                    { 69, "Îles Féroé", 0 },
                    { 70, "Îles (malvinas) Falkland", 0 },
                    { 71, "Géorgie du Sud et les Îles Sandwich du Sud", 0 },
                    { 72, "Fidji", 0 },
                    { 73, "Finlande", 0 },
                    { 74, "Îles Åland", 0 },
                    { 75, "France", 0 },
                    { 76, "Guyane Française", 0 },
                    { 77, "Polynésie Française", 0 },
                    { 78, "Terres Australes Françaises", 0 },
                    { 79, "Djibouti", 0 },
                    { 80, "Gabon", 0 },
                    { 81, "Géorgie", 0 },
                    { 82, "Gambie", 0 },
                    { 83, "Territoire Palestinien Occupé", 0 },
                    { 84, "Allemagne", 0 },
                    { 85, "Ghana", 0 },
                    { 86, "Gibraltar", 0 },
                    { 87, "Kiribati", 0 },
                    { 88, "Grèce", 0 },
                    { 89, "Groenland", 0 },
                    { 90, "Grenade", 0 },
                    { 91, "Guadeloupe", 0 },
                    { 92, "Guam", 0 },
                    { 93, "Guatemala", 0 },
                    { 94, "Guinée", 0 },
                    { 95, "Guyana", 0 },
                    { 96, "Haïti", 0 },
                    { 97, "Îles Heard et Mcdonald", 0 },
                    { 98, "Saint-Siège (état de la Cité du Vatican)", 0 },
                    { 99, "Honduras", 0 },
                    { 100, "Hong-Kong", 0 },
                    { 101, "Hongrie", 0 },
                    { 102, "Islande", 0 },
                    { 103, "Inde", 0 },
                    { 104, "Indonésie", 0 },
                    { 105, "République Islamique d'Iran", 0 },
                    { 106, "Iraq", 0 },
                    { 107, "Irlande", 0 },
                    { 108, "Israël", 0 },
                    { 109, "Italie", 0 },
                    { 110, "Côte d'Ivoire", 0 },
                    { 111, "Jamaïque", 0 },
                    { 112, "Japon", 0 },
                    { 113, "Kazakhstan", 0 },
                    { 114, "Jordanie", 0 },
                    { 115, "Kenya", 0 },
                    { 116, "République Populaire Démocratique de Corée", 0 },
                    { 117, "République de Corée", 0 },
                    { 118, "Koweït", 0 },
                    { 119, "Kirghizistan", 0 },
                    { 120, "République Démocratique Populaire Lao", 0 },
                    { 121, "Liban", 0 },
                    { 122, "Lesotho", 0 },
                    { 123, "Lettonie", 0 },
                    { 124, "Libéria", 0 },
                    { 125, "Jamahiriya Arabe Libyenne", 0 },
                    { 126, "Liechtenstein", 0 },
                    { 127, "Lituanie", 0 },
                    { 128, "Luxembourg", 0 },
                    { 129, "Macao", 0 },
                    { 130, "Madagascar", 0 },
                    { 131, "Malawi", 0 },
                    { 132, "Malaisie", 0 },
                    { 133, "Maldives", 0 },
                    { 134, "Mali", 0 },
                    { 135, "Malte", 0 },
                    { 136, "Martinique", 0 },
                    { 137, "Mauritanie", 0 },
                    { 138, "Maurice", 0 },
                    { 139, "Mexique", 0 },
                    { 140, "Monaco", 0 },
                    { 141, "Mongolie", 0 },
                    { 142, "République de Moldova", 0 },
                    { 143, "Montserrat", 0 },
                    { 144, "Maroc", 0 },
                    { 145, "Mozambique", 0 },
                    { 146, "Oman", 0 },
                    { 147, "Namibie", 0 },
                    { 148, "Nauru", 0 },
                    { 149, "Népal", 0 },
                    { 150, "Pays-Bas", 0 },
                    { 151, "Antilles Néerlandaises", 0 },
                    { 152, "Aruba", 0 },
                    { 153, "Nouvelle-Calédonie", 0 },
                    { 154, "Vanuatu", 0 },
                    { 155, "Nouvelle-Zélande", 0 },
                    { 156, "Nicaragua", 0 },
                    { 157, "Niger", 0 },
                    { 158, "Nigéria", 0 },
                    { 159, "Niué", 0 },
                    { 160, "Île Norfolk", 0 },
                    { 161, "Norvège", 0 },
                    { 162, "Îles Mariannes du Nord", 0 },
                    { 163, "Îles Mineures Éloignées des États-Unis", 0 },
                    { 164, "États Fédérés de Micronésie", 0 },
                    { 165, "Îles Marshall", 0 },
                    { 166, "Palaos", 0 },
                    { 167, "Pakistan", 0 },
                    { 168, "Panama", 0 },
                    { 169, "Papouasie-Nouvelle-Guinée", 0 },
                    { 170, "Paraguay", 0 },
                    { 171, "Pérou", 0 },
                    { 172, "Philippines", 0 },
                    { 173, "Pitcairn", 0 },
                    { 174, "Pologne", 0 },
                    { 175, "Portugal", 0 },
                    { 176, "Guinée-Bissau", 0 },
                    { 177, "Timor-Leste", 0 },
                    { 178, "Porto Rico", 0 },
                    { 179, "Qatar", 0 },
                    { 180, "Réunion", 0 },
                    { 181, "Roumanie", 0 },
                    { 182, "Fédération de Russie", 0 },
                    { 183, "Rwanda", 0 },
                    { 184, "Sainte-Hélène", 0 },
                    { 185, "Saint-Kitts-et-Nevis", 0 },
                    { 186, "Anguilla", 0 },
                    { 187, "Sainte-Lucie", 0 },
                    { 188, "Saint-Pierre-et-Miquelon", 0 },
                    { 189, "Saint-Vincent-et-les Grenadines", 0 },
                    { 190, "Saint-Marin", 0 },
                    { 191, "Sao Tomé-et-Principe", 0 },
                    { 192, "Arabie Saoudite", 0 },
                    { 193, "Sénégal", 0 },
                    { 194, "Seychelles", 0 },
                    { 195, "Sierra Leone", 0 },
                    { 196, "Singapour", 0 },
                    { 197, "Slovaquie", 0 },
                    { 198, "Viet Nam", 0 },
                    { 199, "Slovénie", 0 },
                    { 200, "Somalie", 0 },
                    { 201, "Afrique du Sud", 0 },
                    { 202, "Zimbabwe", 0 },
                    { 203, "Espagne", 0 },
                    { 204, "Sahara Occidental", 0 },
                    { 205, "Soudan", 0 },
                    { 206, "Suriname", 0 },
                    { 207, "Svalbard etÎle Jan Mayen", 0 },
                    { 208, "Swaziland", 0 },
                    { 209, "Suède", 0 },
                    { 210, "Suisse", 0 },
                    { 211, "République Arabe Syrienne", 0 },
                    { 212, "Tadjikistan", 0 },
                    { 213, "Thaïlande", 0 },
                    { 214, "Togo", 0 },
                    { 215, "Tokelau", 0 },
                    { 216, "Tonga", 0 },
                    { 217, "Trinité-et-Tobago", 0 },
                    { 218, "Émirats Arabes Unis", 0 },
                    { 219, "Tunisie", 0 },
                    { 220, "Turquie", 0 },
                    { 221, "Turkménistan", 0 },
                    { 222, "Îles Turks et Caïques", 0 },
                    { 223, "Tuvalu", 0 },
                    { 224, "Ouganda", 0 },
                    { 225, "Ukraine", 0 },
                    { 226, "L'ex-République Yougoslave de Macédoine", 0 },
                    { 227, "Égypte", 0 },
                    { 228, "Royaume-Uni", 0 },
                    { 229, "Île de Man", 0 },
                    { 230, "République-Unie de Tanzanie", 0 },
                    { 231, "États-Unis", 0 },
                    { 232, "Îles Vierges des États-Unis", 0 },
                    { 233, "Burkina Faso", 0 },
                    { 234, "Uruguay", 0 },
                    { 235, "Ouzbékistan", 0 },
                    { 236, "Venezuela", 0 },
                    { 237, "Wallis et Futuna", 0 },
                    { 238, "Samoa", 0 },
                    { 239, "Yémen", 0 },
                    { 240, "Serbie-et-Monténégro", 0 },
                    { 241, "Zambie", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employes_DepartementId",
                table: "Employes",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Employes_PaysId",
                table: "Employes",
                column: "PaysId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employes");

            migrationBuilder.DropTable(
                name: "Departements");

            migrationBuilder.DropTable(
                name: "Pays");
        }
    }
}
