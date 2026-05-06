using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Epidemiologia.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enfermedad",
                columns: table => new
                {
                    Id_enfermedad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cod_enferemedad = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enfermedad", x => x.Id_enfermedad);
                });

            migrationBuilder.CreateTable(
                name: "Paciente",
                columns: table => new
                {
                    Id_paciente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cod_paciente = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paciente", x => x.Id_paciente);
                });

            migrationBuilder.CreateTable(
                name: "RegistroCaso",
                columns: table => new
                {
                    Id_RegistroCaso = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cod_Caso = table.Column<string>(type: "text", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    Id_paciente = table.Column<int>(type: "integer", nullable: false),
                    Id_enfermedad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroCaso", x => x.Id_RegistroCaso);
                    table.ForeignKey(
                        name: "FK_RegistroCaso_Enfermedad_Id_enfermedad",
                        column: x => x.Id_enfermedad,
                        principalTable: "Enfermedad",
                        principalColumn: "Id_enfermedad",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistroCaso_Paciente_Id_paciente",
                        column: x => x.Id_paciente,
                        principalTable: "Paciente",
                        principalColumn: "Id_paciente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistroCaso_Id_enfermedad",
                table: "RegistroCaso",
                column: "Id_enfermedad");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroCaso_Id_paciente",
                table: "RegistroCaso",
                column: "Id_paciente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistroCaso");

            migrationBuilder.DropTable(
                name: "Enfermedad");

            migrationBuilder.DropTable(
                name: "Paciente");
        }
    }
}
