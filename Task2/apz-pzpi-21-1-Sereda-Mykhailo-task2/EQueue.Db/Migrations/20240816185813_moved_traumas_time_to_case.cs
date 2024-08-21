using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EQueue.Db.Migrations
{
    /// <inheritdoc />
    public partial class moved_traumas_time_to_case : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisteredUnixTime",
                table: "Traumas");

            migrationBuilder.AddColumn<long>(
                name: "TraumasRegisteredUnixTime",
                table: "Cases",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TraumasRegisteredUnixTime",
                table: "Cases");

            migrationBuilder.AddColumn<long>(
                name: "RegisteredUnixTime",
                table: "Traumas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
