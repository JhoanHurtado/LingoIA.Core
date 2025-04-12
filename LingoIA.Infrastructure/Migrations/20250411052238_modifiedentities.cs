using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LingoIA.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifiedentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssistantResponse",
                table: "messages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MispronouncedWord",
                table: "messages",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "LearningData",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssistantResponse",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "MispronouncedWord",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "LearningData");
        }
    }
}
