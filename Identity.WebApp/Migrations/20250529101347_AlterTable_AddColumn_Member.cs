using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_AddColumn_Member : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Member",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Member",
                table: "AspNetUsers");
        }
    }
}
