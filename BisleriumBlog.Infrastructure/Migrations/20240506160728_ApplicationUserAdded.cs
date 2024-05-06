using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BisleriumBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUserAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9ff91bc3-f998-4f96-b726-d06ee51d1d10", "AQAAAAIAAYagAAAAEF4wTzAs+TSpQfd5FWhDU40ZYsYf+0JqOMMIxhofz+TK8lFux7TOgyxTTtUAGE/dJQ==", "46426573-9702-4bfd-a0c6-5ffb9d6d4bfe" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReactions_UserId",
                table: "CommentReactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogReactions_UserId",
                table: "BlogReactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogReactions_AspNetUsers_UserId",
                table: "BlogReactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_UserId",
                table: "Blogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactions_AspNetUsers_UserId",
                table: "CommentReactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogReactions_AspNetUsers_UserId",
                table: "BlogReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_UserId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactions_AspNetUsers_UserId",
                table: "CommentReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_CommentReactions_UserId",
                table: "CommentReactions");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_BlogReactions_UserId",
                table: "BlogReactions");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "741dc879-e2b8-4178-ae47-7b051c038e13", "AQAAAAIAAYagAAAAEK8L6Cr30tM3sqXS49v1dQN2Yuo942nLYRAXEnMrglPdMNKn3JmBavfcPIV39nJQoQ==", "764e38d8-1133-4cd2-8aaf-40517fef8348" });
        }
    }
}
