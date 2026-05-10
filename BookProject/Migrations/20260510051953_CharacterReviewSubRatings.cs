using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookProject.Migrations
{
    /// <inheritdoc />
    public partial class CharacterReviewSubRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "CharacterReviews");

            migrationBuilder.AddColumn<string>(
                name: "AiImageUrl",
                table: "CharacterReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "CharacterDevelopment",
                table: "CharacterReviews",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ConsistencyBelievability",
                table: "CharacterReviews",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DepthComplexity",
                table: "CharacterReviews",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ImpactOnStory",
                table: "CharacterReviews",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Memorability",
                table: "CharacterReviews",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OverallRating",
                table: "CharacterReviews",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AiImageUrl",
                table: "CharacterReviews");

            migrationBuilder.DropColumn(
                name: "CharacterDevelopment",
                table: "CharacterReviews");

            migrationBuilder.DropColumn(
                name: "ConsistencyBelievability",
                table: "CharacterReviews");

            migrationBuilder.DropColumn(
                name: "DepthComplexity",
                table: "CharacterReviews");

            migrationBuilder.DropColumn(
                name: "ImpactOnStory",
                table: "CharacterReviews");

            migrationBuilder.DropColumn(
                name: "Memorability",
                table: "CharacterReviews");

            migrationBuilder.DropColumn(
                name: "OverallRating",
                table: "CharacterReviews");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "CharacterReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
