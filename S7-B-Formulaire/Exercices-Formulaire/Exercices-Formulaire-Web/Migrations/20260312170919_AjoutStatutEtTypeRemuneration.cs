using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exercices_Formulaire_Web.Migrations
{
    /// <inheritdoc />
    public partial class AjoutStatutEtTypeRemuneration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstEnEmploi",
                table: "Employes");

            migrationBuilder.AddColumn<int>(
                name: "Statut",
                table: "Employes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TypeRemuneration",
                table: "Employes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Statut",
                table: "Employes");

            migrationBuilder.DropColumn(
                name: "TypeRemuneration",
                table: "Employes");

            migrationBuilder.AddColumn<bool>(
                name: "EstEnEmploi",
                table: "Employes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
