using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EQueue.Db.Migrations
{
    /// <inheritdoc />
    public partial class update_cases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GotIncurableDamage",
                table: "Cases",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Survived",
                table: "Cases",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GotIncurableDamage",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Survived",
                table: "Cases");
        }
    }
}
