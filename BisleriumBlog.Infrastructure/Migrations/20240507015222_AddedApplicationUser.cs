using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BisleriumBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_BlogId_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_CommentReactions_CommentId_UserId",
                table: "CommentReactions");

            migrationBuilder.DropIndex(
                name: "IX_BlogReactions_BlogId_UserId",
                table: "BlogReactions");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "BlogHistory",
                newName: "ImageUrl");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommentReactions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BlogReactions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1e0c4954-fc14-4a92-a81b-6b12d147d249", "AQAAAAIAAYagAAAAEEgE8rUjQn4XN6pDhYFH8qkQ2SQZWMONa+k3g1PehxXUHiUN8pAFcKMcnvBcBvDyew==", "72590618-2cbe-4a8b-953e-46d9ff0002c4" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogId_UserId",
                table: "Comments",
                columns: new[] { "BlogId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReactions_CommentId_UserId",
                table: "CommentReactions",
                columns: new[] { "CommentId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReactions_UserId",
                table: "CommentReactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogReactions_BlogId_UserId",
                table: "BlogReactions",
                columns: new[] { "BlogId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BlogReactions_UserId",
                table: "BlogReactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogReactions_AspNetUsers_UserId",
                table: "BlogReactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_UserId",
                table: "Blogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactions_AspNetUsers_UserId",
                table: "CommentReactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
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
                name: "IX_Comments_BlogId_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_CommentReactions_CommentId_UserId",
                table: "CommentReactions");

            migrationBuilder.DropIndex(
                name: "IX_CommentReactions_UserId",
                table: "CommentReactions");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_BlogReactions_BlogId_UserId",
                table: "BlogReactions");

            migrationBuilder.DropIndex(
                name: "IX_BlogReactions_UserId",
                table: "BlogReactions");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "BlogHistory",
                newName: "Image");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommentReactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BlogReactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36109bb5-c596-4fee-a016-1d8f8c7496cd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "741dc879-e2b8-4178-ae47-7b051c038e13", "AQAAAAIAAYagAAAAEK8L6Cr30tM3sqXS49v1dQN2Yuo942nLYRAXEnMrglPdMNKn3JmBavfcPIV39nJQoQ==", "764e38d8-1133-4cd2-8aaf-40517fef8348" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogId_UserId",
                table: "Comments",
                columns: new[] { "BlogId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentReactions_CommentId_UserId",
                table: "CommentReactions",
                columns: new[] { "CommentId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogReactions_BlogId_UserId",
                table: "BlogReactions",
                columns: new[] { "BlogId", "UserId" },
                unique: true);
        }
    }
}
