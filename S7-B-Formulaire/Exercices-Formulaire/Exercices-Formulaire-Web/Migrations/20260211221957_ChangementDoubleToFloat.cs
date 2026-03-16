using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exercices_Formulaire_Web.Migrations
{
    /// <inheritdoc />
    public partial class ChangementDoubleToFloat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "SalaireAnnuel",
                table: "Employes",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SalaireAnnuel",
                table: "Employes",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
