using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MegaProject.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkerConfifure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Brigades_BrigadeId",
                table: "Workers");

            migrationBuilder.AlterColumn<int>(
                name: "BrigadeId",
                table: "Workers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Brigades_BrigadeId",
                table: "Workers",
                column: "BrigadeId",
                principalTable: "Brigades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Brigades_BrigadeId",
                table: "Workers");

            migrationBuilder.AlterColumn<int>(
                name: "BrigadeId",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Brigades_BrigadeId",
                table: "Workers",
                column: "BrigadeId",
                principalTable: "Brigades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
