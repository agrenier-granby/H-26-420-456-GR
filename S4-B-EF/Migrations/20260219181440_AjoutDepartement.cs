using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace S3_A_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AjoutDepartement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartementId",
                table: "Employes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BudgetAnnuel = table.Column<double>(type: "float", nullable: false),
                    DirecteurId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departements_Employes_DirecteurId",
                        column: x => x.DirecteurId,
                        principalTable: "Employes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employes_DepartementId",
                table: "Employes",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_Departements_DirecteurId",
                table: "Departements",
                column: "DirecteurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employes_Departements_DepartementId",
                table: "Employes",
                column: "DepartementId",
                principalTable: "Departements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employes_Departements_DepartementId",
                table: "Employes");

            migrationBuilder.DropTable(
                name: "Departements");

            migrationBuilder.DropIndex(
                name: "IX_Employes_DepartementId",
                table: "Employes");

            migrationBuilder.DropColumn(
                name: "DepartementId",
                table: "Employes");
        }
    }
}
