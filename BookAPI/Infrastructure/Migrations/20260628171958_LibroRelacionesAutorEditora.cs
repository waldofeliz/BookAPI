using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LibroRelacionesAutorEditora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EditoraId",
                table: "Libros",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LibroAutores",
                columns: table => new
                {
                    LibroId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibroAutores", x => new { x.LibroId, x.AutorId });
                    table.ForeignKey(
                        name: "FK_LibroAutores_Autores_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LibroAutores_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Libros_EditoraId",
                table: "Libros",
                column: "EditoraId");

            migrationBuilder.CreateIndex(
                name: "IX_LibroAutores_AutorId",
                table: "LibroAutores",
                column: "AutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libros_Editoras_EditoraId",
                table: "Libros",
                column: "EditoraId",
                principalTable: "Editoras",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libros_Editoras_EditoraId",
                table: "Libros");

            migrationBuilder.DropTable(
                name: "LibroAutores");

            migrationBuilder.DropIndex(
                name: "IX_Libros_EditoraId",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "EditoraId",
                table: "Libros");
        }
    }
}
