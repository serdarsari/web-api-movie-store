using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieStore.API.Db.Migrations
{
    public partial class RelationshipsFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MovieDirectors_MovieId",
                table: "MovieDirectors",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieActors_MovieId",
                table: "MovieActors",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorAwardWinners_AwardId",
                table: "DirectorAwardWinners",
                column: "AwardId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorAwardWinners_AwardId",
                table: "ActorAwardWinners",
                column: "AwardId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorAwardWinners_Awards_AwardId",
                table: "ActorAwardWinners",
                column: "AwardId",
                principalTable: "Awards",
                principalColumn: "AwardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DirectorAwardWinners_Awards_AwardId",
                table: "DirectorAwardWinners",
                column: "AwardId",
                principalTable: "Awards",
                principalColumn: "AwardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActors_Movies_MovieId",
                table: "MovieActors",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDirectors_Movies_MovieId",
                table: "MovieDirectors",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorAwardWinners_Awards_AwardId",
                table: "ActorAwardWinners");

            migrationBuilder.DropForeignKey(
                name: "FK_DirectorAwardWinners_Awards_AwardId",
                table: "DirectorAwardWinners");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieActors_Movies_MovieId",
                table: "MovieActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieDirectors_Movies_MovieId",
                table: "MovieDirectors");

            migrationBuilder.DropIndex(
                name: "IX_MovieDirectors_MovieId",
                table: "MovieDirectors");

            migrationBuilder.DropIndex(
                name: "IX_MovieActors_MovieId",
                table: "MovieActors");

            migrationBuilder.DropIndex(
                name: "IX_DirectorAwardWinners_AwardId",
                table: "DirectorAwardWinners");

            migrationBuilder.DropIndex(
                name: "IX_ActorAwardWinners_AwardId",
                table: "ActorAwardWinners");
        }
    }
}
