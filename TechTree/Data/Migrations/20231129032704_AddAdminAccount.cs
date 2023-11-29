using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Primitives;
using System.Text;

#nullable disable

namespace TechTree.Data.Migrations
{
    public partial class AddAdminAccount : Migration
    {
        const string ADMIN_USER_GUID = "9dda0423-cf65-4274-aa8a-c97194371580";
        const string ADMIN_ROLE_GUID = "bbbd643a-019a-499a-814c-73107a42abfd";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var haspedPassword = hasher.HashPassword(null, "Welcome@123");

            StringBuilder str = new StringBuilder();

            str.AppendLine("INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName,Email,EmailConfirmed,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,AccessFailedCount,NormalizedEmail,PasswordHash,SecurityStamp,FirstName)"); str.Append("VALUES(");
            str.Append($"'{ADMIN_USER_GUID}',");
            str.Append("'admin@learners.com',");
            str.Append("'ADMIN@LEARNERS.COM',");
            str.Append("'admin@learners.com',");
            str.Append("0,");
            str.Append("0,");
            str.Append("0,");
            str.Append("0,");
            str.Append("0,");
            str.Append("'ADMIN@LEARNERS.COM',");
            str.Append($"'{haspedPassword}',");
            str.Append("'',");
            str.Append("'Admin'");
            str.Append(")");

            migrationBuilder.Sql(str.ToString());
            migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id,Name,NormalizedName) VALUES('{ADMIN_ROLE_GUID}','admin','ADMIN')");
            migrationBuilder.Sql($"INSERT INTO AspNetUserRoles (UserId,RoleId) VALUES('{ADMIN_USER_GUID}','{ADMIN_ROLE_GUID}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles where UserId='{ADMIN_USER_GUID}' and RoleId='{ADMIN_ROLE_GUID}'");
            migrationBuilder.Sql($"DELETE FROM AspNetUsers where Id='{ADMIN_USER_GUID}'");
            migrationBuilder.Sql($"DELETE FROM AspNetRoles where Id='{ADMIN_ROLE_GUID}'");
        }
    }
}
