using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace randevu_kayit.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicalHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HastaId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoktorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TarihSaat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tani = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tedavi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YazilanIlaclar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notlar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabSonuclari = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SonrakiRandevuTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalHistories_AspNetUsers_DoktorId",
                        column: x => x.DoktorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalHistories_AspNetUsers_HastaId",
                        column: x => x.HastaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_DoktorId",
                table: "MedicalHistories",
                column: "DoktorId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_HastaId",
                table: "MedicalHistories",
                column: "HastaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalHistories");
        }
    }
}
