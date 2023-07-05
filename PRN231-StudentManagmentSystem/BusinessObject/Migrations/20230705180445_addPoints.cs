using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    public partial class addPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Credit",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPassed",
                table: "Points",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsPassed",
                table: "Points");
        }
    }
}
