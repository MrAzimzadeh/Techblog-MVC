using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddADSModel1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAds_Advertisement_AdvertisementId",
                table: "UserAds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advertisement",
                table: "Advertisement");

            migrationBuilder.RenameTable(
                name: "Advertisement",
                newName: "Advertisements");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advertisements",
                table: "Advertisements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAds_Advertisements_AdvertisementId",
                table: "UserAds",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAds_Advertisements_AdvertisementId",
                table: "UserAds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advertisements",
                table: "Advertisements");

            migrationBuilder.RenameTable(
                name: "Advertisements",
                newName: "Advertisement");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advertisement",
                table: "Advertisement",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAds_Advertisement_AdvertisementId",
                table: "UserAds",
                column: "AdvertisementId",
                principalTable: "Advertisement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
