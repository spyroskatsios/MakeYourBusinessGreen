using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MakeYourBusinessGreen.Infastructure.Persistence.Migrations
{
    public partial class ReadInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MakeYourBusinessGreen");

            migrationBuilder.CreateTable(
                name: "Offices",
                schema: "MakeYourBusinessGreen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suggestions",
                schema: "MakeYourBusinessGreen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suggestions_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalSchema: "MakeYourBusinessGreen",
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatusChangeEvents",
                schema: "MakeYourBusinessGreen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    From = table.Column<int>(type: "int", nullable: false),
                    To = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModeratorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuggestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusChangeEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusChangeEvents_Suggestions_SuggestionId",
                        column: x => x.SuggestionId,
                        principalSchema: "MakeYourBusinessGreen",
                        principalTable: "Suggestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusChangeEvents_SuggestionId",
                schema: "MakeYourBusinessGreen",
                table: "StatusChangeEvents",
                column: "SuggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_OfficeId",
                schema: "MakeYourBusinessGreen",
                table: "Suggestions",
                column: "OfficeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusChangeEvents",
                schema: "MakeYourBusinessGreen");

            migrationBuilder.DropTable(
                name: "Suggestions",
                schema: "MakeYourBusinessGreen");

            migrationBuilder.DropTable(
                name: "Offices",
                schema: "MakeYourBusinessGreen");
        }
    }
}
