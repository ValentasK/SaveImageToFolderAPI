using Microsoft.EntityFrameworkCore.Migrations;

namespace SaveImageToFolderAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageID = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Resized = table.Column<bool>(nullable: false),
                    ImageHeight = table.Column<int>(nullable: false),
                    ImageWidth = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Image");
        }
    }
}
