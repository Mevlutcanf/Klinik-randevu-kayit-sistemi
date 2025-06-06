using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace randevu_kayit.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPropertiesAndQuickAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HastaTelefon",
                table: "QuickAppointments",
                newName: "Telefon");

            migrationBuilder.RenameColumn(
                name: "HastaAdi",
                table: "QuickAppointments",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "LaboratuvarSonuclari",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RadyolojiSonuclari",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Recete",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tani",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tedavi",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdSoyad",
                table: "QuickAppointments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TCKimlikNo",
                table: "QuickAppointments",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Uzmanlik",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KanGrubu",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Adres",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdSoyad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "About",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcilDurumKisisi",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcilDurumTelefonu",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alerjiler",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AlkolKullanimi",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Boy",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CalismaSaatleri",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cinsiyet",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeneyimYili",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiplomaNo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Kilo",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KronikHastaliklar",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KullanilanIlaclar",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MezunOlduguUniversite",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MezuniyetYili",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SigaraKullanimi",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LaboratuvarSonuclari",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "RadyolojiSonuclari",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "Recete",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "Tani",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "Tedavi",
                table: "Randevular");

            migrationBuilder.DropColumn(
                name: "AdSoyad",
                table: "QuickAppointments");

            migrationBuilder.DropColumn(
                name: "TCKimlikNo",
                table: "QuickAppointments");

            migrationBuilder.DropColumn(
                name: "AcilDurumKisisi",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AcilDurumTelefonu",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Alerjiler",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AlkolKullanimi",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Boy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CalismaSaatleri",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Cinsiyet",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeneyimYili",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DiplomaNo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Kilo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "KronikHastaliklar",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "KullanilanIlaclar",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MezunOlduguUniversite",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MezuniyetYili",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SigaraKullanimi",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Telefon",
                table: "QuickAppointments",
                newName: "HastaTelefon");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "QuickAppointments",
                newName: "HastaAdi");

            migrationBuilder.AlterColumn<string>(
                name: "Uzmanlik",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KanGrubu",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Adres",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdSoyad",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "About",
                table: "AspNetUsers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
