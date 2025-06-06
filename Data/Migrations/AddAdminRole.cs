using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Identity;

#nullable disable

namespace randevu_kayit.Data.Migrations
{
    public partial class AddAdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Admin role
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { "1", "Admin", "ADMIN", Guid.NewGuid().ToString() }
            );

            // Add Admin user
            var hasher = new PasswordHasher<IdentityUser>();
            var adminPassword = hasher.HashPassword(null, "Admin123!");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", 
                               "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", 
                               "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount", "AdSoyad" },
                values: new object[] { "1", "admin@admin.com", "ADMIN@ADMIN.COM", "admin@admin.com", "ADMIN@ADMIN.COM", true,
                                    adminPassword, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), null, false,
                                    false, null, false, 0, "System Admin" }
            );

            // Add Admin user to Admin role
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "1", "1" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove Admin user from Admin role
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "1", "1" }
            );

            // Remove Admin user
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1"
            );

            // Remove Admin role
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1"
            );
        }
    }
} 