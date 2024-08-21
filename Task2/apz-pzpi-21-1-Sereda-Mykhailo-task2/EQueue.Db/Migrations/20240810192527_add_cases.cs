using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EQueue.Db.Migrations
{
    /// <inheritdoc />
    public partial class add_cases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CaseId",
                table: "Traumas",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ClinicId = table.Column<long>(type: "bigint", nullable: true),
                    StartedTreatmentUnixTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cases_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Traumas_CaseId",
                table: "Traumas",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ClinicId",
                table: "Cases",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Traumas_Cases_CaseId",
                table: "Traumas",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Traumas_Cases_CaseId",
                table: "Traumas");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Traumas_CaseId",
                table: "Traumas");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "Traumas");
        }
    }
}
