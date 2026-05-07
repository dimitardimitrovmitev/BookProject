using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookProject.Migrations
{
    /// <inheritdoc />
    public partial class AddReadingStatus2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "UserBooks");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserBooks");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "UserBooks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
