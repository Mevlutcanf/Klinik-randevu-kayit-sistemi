using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace randevu_kayit.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentIdToQuickAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuickAppointments_Departments_DepartmentId",
                table: "QuickAppointments");

            migrationBuilder.DropForeignKey(
                name: "FK_QuickAppointments_Departments_DepartmentId1",
                table: "QuickAppointments");

            migrationBuilder.DropIndex(
                name: "IX_QuickAppointments_DepartmentId",
                table: "QuickAppointments");

            migrationBuilder.DropIndex(
                name: "IX_QuickAppointments_DepartmentId1",
                table: "QuickAppointments");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "QuickAppointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId1",
                table: "QuickAppointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuickAppointments_DepartmentId",
                table: "QuickAppointments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_QuickAppointments_DepartmentId1",
                table: "QuickAppointments",
                column: "DepartmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_QuickAppointments_Departments_DepartmentId",
                table: "QuickAppointments",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuickAppointments_Departments_DepartmentId1",
                table: "QuickAppointments",
                column: "DepartmentId1",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
