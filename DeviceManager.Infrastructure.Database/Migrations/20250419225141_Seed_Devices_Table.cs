using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeviceManager.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Devices_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Name", "Brand", "State", "CreationTime" },
                values: new object[,]
                {
                    { "iPhone 14", "Apple", 1, DateTime.UtcNow },
                    { "Galaxy S21", "Samsung", 1, DateTime.UtcNow },
                    { "Pixel 7", "Google", 1, DateTime.UtcNow },

                    { "iPhone 13", "Apple", 2, DateTime.UtcNow },
                    { "Galaxy Note 20", "Samsung", 2, DateTime.UtcNow },
                    { "Pixel 6", "Google", 2, DateTime.UtcNow },

                    { "iPhone SE", "Apple", 3, DateTime.UtcNow },
                    { "Galaxy A52", "Samsung", 3, DateTime.UtcNow },
                    { "Pixel 5", "Google", 3, DateTime.UtcNow }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Devices");
        }
    }
}
