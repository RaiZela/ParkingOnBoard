 using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingOnBoard.Migrations
{
    public partial class ReasonParameter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReasonID",
                table: "Streets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReasonID",
                table: "States",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReasonForClosing",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReasonForClosing", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Streets_ReasonID",
                table: "Streets",
                column: "ReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_States_ReasonID",
                table: "States",
                column: "ReasonID");

            migrationBuilder.AddForeignKey(
                name: "FK_States_ReasonForClosing_ReasonID",
                table: "States",
                column: "ReasonID",
                principalTable: "ReasonForClosing",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Streets_ReasonForClosing_ReasonID",
                table: "Streets",
                column: "ReasonID",
                principalTable: "ReasonForClosing",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_States_ReasonForClosing_ReasonID",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_Streets_ReasonForClosing_ReasonID",
                table: "Streets");

            migrationBuilder.DropTable(
                name: "ReasonForClosing");

            migrationBuilder.DropIndex(
                name: "IX_Streets_ReasonID",
                table: "Streets");

            migrationBuilder.DropIndex(
                name: "IX_States_ReasonID",
                table: "States");

            migrationBuilder.DropColumn(
                name: "ReasonID",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "ReasonID",
                table: "States");
        }
    }
}
