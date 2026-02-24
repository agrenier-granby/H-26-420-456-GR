using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace S3_A_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AjoutDepartementDansEmploye : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departements_Employes_DirecteurId",
                table: "Departements");

            migrationBuilder.DropForeignKey(
                name: "FK_Employes_Departements_DepartementId",
                table: "Employes");

            migrationBuilder.DropIndex(
                name: "IX_Departements_DirecteurId",
                table: "Departements");

            migrationBuilder.DropColumn(
                name: "DirecteurId",
                table: "Departements");

            migrationBuilder.AlterColumn<int>(
                name: "DepartementId",
                table: "Employes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employes_Departements_DepartementId",
                table: "Employes",
                column: "DepartementId",
                principalTable: "Departements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employes_Departements_DepartementId",
                table: "Employes");

            migrationBuilder.AlterColumn<int>(
                name: "DepartementId",
                table: "Employes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DirecteurId",
                table: "Departements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departements_DirecteurId",
                table: "Departements",
                column: "DirecteurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departements_Employes_DirecteurId",
                table: "Departements",
                column: "DirecteurId",
                principalTable: "Employes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employes_Departements_DepartementId",
                table: "Employes",
                column: "DepartementId",
                principalTable: "Departements",
                principalColumn: "Id");
        }
    }
}
