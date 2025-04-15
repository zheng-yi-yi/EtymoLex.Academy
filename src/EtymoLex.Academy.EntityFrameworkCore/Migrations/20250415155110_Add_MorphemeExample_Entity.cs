using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtymoLex.Academy.Migrations
{
    /// <inheritdoc />
    public partial class Add_MorphemeExample_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EtymoLex_MorphemeExamples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Word = table.Column<string>(type: "text", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    Breakdown = table.Column<string>(type: "text", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtymoLex_MorphemeExamples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EtymoLex_MorphemeExamples_EtymoLex_Morphemes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "EtymoLex_Morphemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EtymoLex_MorphemeExamples_ParentId",
                table: "EtymoLex_MorphemeExamples",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_EtymoLex_MorphemeExamples_TenantId_Word",
                table: "EtymoLex_MorphemeExamples",
                columns: new[] { "TenantId", "Word" },
                unique: true)
                .Annotation("Npgsql:NullsDistinct", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EtymoLex_MorphemeExamples");
        }
    }
}
