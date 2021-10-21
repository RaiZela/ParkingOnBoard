using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingOnBoard.Migrations
{
    public partial class TablesRelationshpis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StreetID",
                table: "ParkingSlots",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSlots_StreetID",
                table: "ParkingSlots",
                column: "StreetID");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSlots_Streets_StreetID",
                table: "ParkingSlots",
                column: "StreetID",
                principalTable: "Streets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSlots_Streets_StreetID",
                table: "ParkingSlots");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSlots_StreetID",
                table: "ParkingSlots");

            migrationBuilder.DropColumn(
                name: "StreetID",
                table: "ParkingSlots");
        }
    }
}
