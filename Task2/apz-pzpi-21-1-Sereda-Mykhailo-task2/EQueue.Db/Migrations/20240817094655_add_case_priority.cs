using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EQueue.Db.Migrations
{
    /// <inheritdoc />
    public partial class add_case_priority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CasePriorities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    DamagePriority = table.Column<float>(type: "real", nullable: false),
                    DeathPriority = table.Column<float>(type: "real", nullable: false),
                    CombinedPriority = table.Column<float>(type: "real", nullable: false),
                    PriorityPeriodStartUnix = table.Column<long>(type: "bigint", nullable: false),
                    PriorityPeriodEndUnix = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePriorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasePriorities_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CasePriorities_CaseId",
                table: "CasePriorities",
                column: "CaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CasePriorities");
        }
    }
}
