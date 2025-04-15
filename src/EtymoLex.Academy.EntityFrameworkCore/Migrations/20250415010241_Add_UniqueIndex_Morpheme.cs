using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtymoLex.Academy.Migrations
{
    /// <inheritdoc />
    public partial class Add_UniqueIndex_Morpheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EtymoLex_Morphemes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    OriginLanguage = table.Column<string>(type: "text", nullable: false),
                    Meaning = table.Column<string>(type: "text", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtymoLex_Morphemes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EtymoLex_Morphemes_Name",
                table: "EtymoLex_Morphemes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_EtymoLex_Morphemes_NormalizedName",
                table: "EtymoLex_Morphemes",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_EtymoLex_Morphemes_TenantId_Value",
                table: "EtymoLex_Morphemes",
                columns: new[] { "TenantId", "Value" },
                unique: true)
                .Annotation("Npgsql:NullsDistinct", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EtymoLex_Morphemes");
        }
    }
}
