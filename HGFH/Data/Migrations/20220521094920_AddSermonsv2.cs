using Microsoft.EntityFrameworkCore.Migrations;

namespace HGFH.Data.Migrations
{
    public partial class AddSermonsv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Sermons",
                newName: "Preacher");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Preacher",
                table: "Sermons",
                newName: "Body");
        }
    }
}
