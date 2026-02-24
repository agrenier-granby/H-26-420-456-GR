using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace S3_A_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AjoutSuperficieDansPays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SuperficieM2",
                table: "Pays",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuperficieM2",
                table: "Pays");
        }
    }
}
