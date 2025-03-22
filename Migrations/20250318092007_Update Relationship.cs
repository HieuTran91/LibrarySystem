using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_Books_BookID",
                table: "Borrowings");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Borrowings_BorrowingID",
                table: "Payments");

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "PaymentMethodId", "MethodName" },
                values: new object[] { 4, "Momo" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookId",
                table: "Reviews",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_Books_BookID",
                table: "Borrowings",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Borrowings_BorrowingID",
                table: "Payments",
                column: "BorrowingID",
                principalTable: "Borrowings",
                principalColumn: "BorrowingID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Books_BookId",
                table: "Reviews",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_Books_BookID",
                table: "Borrowings");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Borrowings_BorrowingID",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Books_BookId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_BookId",
                table: "Reviews");

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodId",
                keyValue: 4);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_Books_BookID",
                table: "Borrowings",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Borrowings_BorrowingID",
                table: "Payments",
                column: "BorrowingID",
                principalTable: "Borrowings",
                principalColumn: "BorrowingID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
