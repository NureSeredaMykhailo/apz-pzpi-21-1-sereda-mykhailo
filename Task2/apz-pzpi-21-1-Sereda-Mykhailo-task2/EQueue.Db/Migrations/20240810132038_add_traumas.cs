using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EQueue.Db.Migrations
{
    /// <inheritdoc />
    public partial class add_traumas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Traumas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegisteredUnixTime = table.Column<long>(type: "bigint", nullable: false),
                    TraumaTypeId = table.Column<long>(type: "bigint", nullable: true),
                    TraumaPlaceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traumas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Traumas_TraumaPlaces_TraumaPlaceId",
                        column: x => x.TraumaPlaceId,
                        principalTable: "TraumaPlaces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Traumas_TraumaTypes_TraumaTypeId",
                        column: x => x.TraumaTypeId,
                        principalTable: "TraumaTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Traumas_TraumaPlaceId",
                table: "Traumas",
                column: "TraumaPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Traumas_TraumaTypeId",
                table: "Traumas",
                column: "TraumaTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Traumas");
        }
    }
}
