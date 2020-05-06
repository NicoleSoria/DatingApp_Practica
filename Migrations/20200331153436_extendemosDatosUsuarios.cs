using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class extendemosDatosUsuarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Buscando",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Genero",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Intereses",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Introduccion",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KnowAs",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaVezActivo",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Fotos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    EsPrincipal = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_UserId",
                table: "Fotos",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fotos");

            migrationBuilder.DropColumn(
                name: "Buscando",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Genero",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Intereses",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Introduccion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "KnowAs",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UltimaVezActivo",
                table: "Users");
        }
    }
}
