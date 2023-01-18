using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedStrength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Strenght",
                table: "Characters",
                newName: "Strength");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Strength",
                table: "Characters",
                newName: "Strenght");
        }
    }
}
