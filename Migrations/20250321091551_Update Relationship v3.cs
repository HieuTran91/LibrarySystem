using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_Payments_BorrowingID",
                table: "Borrowings");

            migrationBuilder.AlterColumn<int>(
                name: "BorrowingID",
                table: "Borrowings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BorrowingID",
                table: "Payments",
                column: "BorrowingID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Borrowings_BorrowingID",
                table: "Payments",
                column: "BorrowingID",
                principalTable: "Borrowings",
                principalColumn: "BorrowingID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Borrowings_BorrowingID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_BorrowingID",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "BorrowingID",
                table: "Borrowings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_Payments_BorrowingID",
                table: "Borrowings",
                column: "BorrowingID",
                principalTable: "Payments",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
