using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingOnBoard.Migrations
{
    public partial class RemoveReasonClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Streets_ReasonsForClosing_ReasonID",
                table: "Streets");

            migrationBuilder.DropTable(
                name: "ReasonsForClosing");

            migrationBuilder.DropIndex(
                name: "IX_Streets_ReasonID",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "ReasonID",
                table: "Streets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReasonID",
                table: "Streets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReasonsForClosing",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReasonsForClosing", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Streets_ReasonID",
                table: "Streets",
                column: "ReasonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Streets_ReasonsForClosing_ReasonID",
                table: "Streets",
                column: "ReasonID",
                principalTable: "ReasonsForClosing",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
