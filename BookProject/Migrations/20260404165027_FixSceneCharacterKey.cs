using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookProject.Migrations
{
    /// <inheritdoc />
    public partial class FixSceneCharacterKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageGenerationCharacters_Characters_CharacterId",
                table: "ImageGenerationCharacters");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageGenerationCharacters_Characters_CharacterId1",
                table: "ImageGenerationCharacters");

            migrationBuilder.DropForeignKey(
                name: "FK_SceneCharacters_Characters_CharacterId",
                table: "SceneCharacters");

            migrationBuilder.DropIndex(
                name: "IX_ImageGenerationCharacters_CharacterId1",
                table: "ImageGenerationCharacters");

            migrationBuilder.DropColumn(
                name: "SceneCharacterId",
                table: "SceneCharacters");

            migrationBuilder.DropColumn(
                name: "CharacterId1",
                table: "ImageGenerationCharacters");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageGenerationCharacters_Characters_CharacterId",
                table: "ImageGenerationCharacters",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_SceneCharacters_Characters_CharacterId",
                table: "SceneCharacters",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageGenerationCharacters_Characters_CharacterId",
                table: "ImageGenerationCharacters");

            migrationBuilder.DropForeignKey(
                name: "FK_SceneCharacters_Characters_CharacterId",
                table: "SceneCharacters");

            migrationBuilder.AddColumn<int>(
                name: "SceneCharacterId",
                table: "SceneCharacters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CharacterId1",
                table: "ImageGenerationCharacters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageGenerationCharacters_CharacterId1",
                table: "ImageGenerationCharacters",
                column: "CharacterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageGenerationCharacters_Characters_CharacterId",
                table: "ImageGenerationCharacters",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "CharacterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageGenerationCharacters_Characters_CharacterId1",
                table: "ImageGenerationCharacters",
                column: "CharacterId1",
                principalTable: "Characters",
                principalColumn: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_SceneCharacters_Characters_CharacterId",
                table: "SceneCharacters",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "CharacterId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
