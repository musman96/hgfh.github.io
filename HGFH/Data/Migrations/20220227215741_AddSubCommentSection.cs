using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HGFH.Data.Migrations
{
    public partial class AddSubCommentSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                 name: "SubComments",
                 columns: table => new
                 {
                     Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                     Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                     CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                     CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                     ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_SubComments", x => x.Id);
                     //table.ForeignKey(
                     //    name: "FK_Comments_Blogs_BlogPostId",
                     //    column: x => x.BlogPostId,
                     //    principalTable: "Blogs",
                     //    principalColumn: "Id",
                     //    onDelete: ReferentialAction.Cascade);
                 });
            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_SubComments",
            //    table: "SubComments",
            //    column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SubComments_BlogPostId",
                table: "SubComments",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_SubComments_CommentId",
                table: "SubComments",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubComments_Blogs_BlogPostId",
                table: "SubComments",
                column: "BlogPostId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubComments_Comments_CommentId",
                table: "SubComments",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubComments_Blogs_BlogPostId",
                table: "SubComments");

            migrationBuilder.DropForeignKey(
                name: "FK_SubComments_Comments_CommentId",
                table: "SubComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubComments",
                table: "SubComments");

            migrationBuilder.DropIndex(
                name: "IX_SubComments_BlogPostId",
                table: "SubComments");

            migrationBuilder.DropIndex(
                name: "IX_SubComments_CommentId",
                table: "SubComments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SubComments");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "SubComments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SubComments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "SubComments");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "SubComments");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "SubComments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SubComments");

            migrationBuilder.RenameTable(
                name: "SubComments",
                newName: "SubComment");

            migrationBuilder.AlterColumn<Guid>(
                name: "CommentId",
                table: "SubComment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "BlogPostId",
                table: "SubComment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_SubComment_Blogs_BlogPostId",
                table: "SubComment",
                column: "BlogPostId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubComment_Comments_CommentId",
                table: "SubComment",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
