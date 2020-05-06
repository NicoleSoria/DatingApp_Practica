using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class agregoEntidadMensajes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mensajes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmisorId = table.Column<int>(nullable: false),
                    DestinatarioId = table.Column<int>(nullable: false),
                    Contenido = table.Column<string>(nullable: true),
                    Leido = table.Column<bool>(nullable: false),
                    FechaLeido = table.Column<DateTime>(nullable: true),
                    FechaEnviado = table.Column<DateTime>(nullable: false),
                    EmisorEliminar = table.Column<bool>(nullable: false),
                    DestinatarioEliminar = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensajes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mensajes_Users_DestinatarioId",
                        column: x => x.DestinatarioId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mensajes_Users_EmisorId",
                        column: x => x.EmisorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_DestinatarioId",
                table: "Mensajes",
                column: "DestinatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_EmisorId",
                table: "Mensajes",
                column: "EmisorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mensajes");
        }
    }
}
