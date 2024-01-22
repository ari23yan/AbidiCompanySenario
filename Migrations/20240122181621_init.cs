using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbidiCompanySenario.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personnels",
                columns: table => new
                {
                    Personnel_Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Personnel_Code = table.Column<long>(type: "bigint", nullable: false),
                    National_Code = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnels", x => x.Personnel_Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personnels");
        }
    }
}
