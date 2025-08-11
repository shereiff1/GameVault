using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameVault.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddingFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_companies_CompanyId",
                table: "games");

            migrationBuilder.DropForeignKey(
                name: "FK_inventoryItems_inventories_CompanyId",
                table: "inventoryItems");

            //migrationBuilder.DropTable(
            //    name: "inventories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_companies",
                table: "companies");

            migrationBuilder.RenameTable(
                name: "companies",
                newName: "Companies");

            //migrationBuilder.AddColumn<string>(
            //    name: "Description",
            //    table: "games",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_games_Companies_CompanyId",
                table: "games",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_inventoryItems_Companies_CompanyId",
                table: "inventoryItems",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_Companies_CompanyId",
                table: "games");

            migrationBuilder.DropForeignKey(
                name: "FK_inventoryItems_Companies_CompanyId",
                table: "inventoryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "games");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "games");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "companies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_companies",
                table: "companies",
                column: "CompanyId");

            migrationBuilder.CreateTable(
                name: "inventories",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventories", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_inventories_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_games_companies_CompanyId",
                table: "games",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_inventoryItems_inventories_CompanyId",
                table: "inventoryItems",
                column: "CompanyId",
                principalTable: "inventories",
                principalColumn: "CompanyId");
        }
    }
}
