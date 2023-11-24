using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkShortener.Migrations
{
    /// <inheritdoc />
    public partial class addUniqueConstraintToLongUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Links",
                keyColumn: "LongUrl",
                keyValue: null,
                column: "LongUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "LongUrl",
                table: "Links",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Links_LongUrl",
                table: "Links",
                column: "LongUrl",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Links_LongUrl",
                table: "Links");

            migrationBuilder.AlterColumn<string>(
                name: "LongUrl",
                table: "Links",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
