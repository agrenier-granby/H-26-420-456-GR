using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exercices_Formulaire_Web.Migrations
{
    /// <inheritdoc />
    public partial class Partie4Validations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Employes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDepart",
                table: "Employes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateNaissance",
                table: "Employes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Titre",
                table: "Employes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDepart",
                table: "Employes");

            migrationBuilder.DropColumn(
                name: "DateNaissance",
                table: "Employes");

            migrationBuilder.DropColumn(
                name: "Titre",
                table: "Employes");

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Employes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
