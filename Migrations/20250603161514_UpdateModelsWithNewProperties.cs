using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace randevu_kayit.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsWithNewProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdSoyad",
                table: "QuickAppointments");

            migrationBuilder.DropColumn(
                name: "Not",
                table: "QuickAppointments");

            migrationBuilder.DropColumn(
                name: "RandevuSaati",
                table: "QuickAppointments");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "DoctorSchedules");

            migrationBuilder.RenameColumn(
                name: "OlusturulmaTarihi",
                table: "Randevular",
                newName: "OlusturmaTarihi");

            migrationBuilder.RenameColumn(
                name: "Telefon",
                table: "QuickAppointments",
                newName: "Sikayet");

            migrationBuilder.RenameColumn(
                name: "RandevuTarihi",
                table: "QuickAppointments",
                newName: "TarihSaat");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "QuickAppointments",
                newName: "HastaTelefon");

            migrationBuilder.RenameColumn(
                name: "Bolum",
                table: "QuickAppointments",
                newName: "HastaAdi");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "DoctorSchedules",
                newName: "BitisSaat");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "DoctorSchedules",
                newName: "BaslangicSaat");

            migrationBuilder.AddColumn<string>(
                name: "DoktorId",
                table: "QuickAppointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "DoctorSchedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "DoctorSchedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_QuickAppointments_DoktorId",
                table: "QuickAppointments",
                column: "DoktorId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuickAppointments_AspNetUsers_DoktorId",
                table: "QuickAppointments",
                column: "DoktorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuickAppointments_AspNetUsers_DoktorId",
                table: "QuickAppointments");

            migrationBuilder.DropIndex(
                name: "IX_QuickAppointments_DoktorId",
                table: "QuickAppointments");

            migrationBuilder.DropColumn(
                name: "DoktorId",
                table: "QuickAppointments");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "DoctorSchedules");

            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "DoctorSchedules");

            migrationBuilder.RenameColumn(
                name: "OlusturmaTarihi",
                table: "Randevular",
                newName: "OlusturulmaTarihi");

            migrationBuilder.RenameColumn(
                name: "TarihSaat",
                table: "QuickAppointments",
                newName: "RandevuTarihi");

            migrationBuilder.RenameColumn(
                name: "Sikayet",
                table: "QuickAppointments",
                newName: "Telefon");

            migrationBuilder.RenameColumn(
                name: "HastaTelefon",
                table: "QuickAppointments",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "HastaAdi",
                table: "QuickAppointments",
                newName: "Bolum");

            migrationBuilder.RenameColumn(
                name: "BitisSaat",
                table: "DoctorSchedules",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "BaslangicSaat",
                table: "DoctorSchedules",
                newName: "EndTime");

            migrationBuilder.AddColumn<string>(
                name: "AdSoyad",
                table: "QuickAppointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Not",
                table: "QuickAppointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "RandevuSaati",
                table: "QuickAppointments",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "DoctorSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
