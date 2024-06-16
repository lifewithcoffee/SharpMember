using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SharpMember.Migrations.Postgres.Migrations.Member
{
    /// <inheritdoc />
    public partial class InitMemberTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Member");

            migrationBuilder.CreateTable(
                name: "Communities",
                schema: "Member",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Introduction = table.Column<string>(type: "text", nullable: true),
                    Announcement = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "Member",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CommunityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalSchema: "Member",
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberProfileItemTemplates",
                schema: "Member",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemName = table.Column<string>(type: "text", nullable: true),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    CommunityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberProfileItemTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberProfileItemTemplates_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalSchema: "Member",
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                schema: "Member",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CommunityRole = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    MemberNumber = table.Column<int>(type: "integer", nullable: false),
                    Renewed = table.Column<bool>(type: "boolean", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancellationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    CommunityId = table.Column<int>(type: "integer", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalSchema: "Member",
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupMemberRelation",
                schema: "Member",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    GroupRole = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMemberRelation", x => new { x.MemberId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GroupMemberRelation_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Member",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMemberRelation_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "Member",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberProfileItems",
                schema: "Member",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemValue = table.Column<string>(type: "text", nullable: true),
                    MemberId = table.Column<int>(type: "integer", nullable: false),
                    MemberProfileItemTemplateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberProfileItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberProfileItems_MemberProfileItemTemplates_MemberProfile~",
                        column: x => x.MemberProfileItemTemplateId,
                        principalSchema: "Member",
                        principalTable: "MemberProfileItemTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberProfileItems_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "Member",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberRelation_GroupId",
                schema: "Member",
                table: "GroupMemberRelation",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CommunityId",
                schema: "Member",
                table: "Groups",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProfileItems_MemberId",
                schema: "Member",
                table: "MemberProfileItems",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProfileItems_MemberProfileItemTemplateId",
                schema: "Member",
                table: "MemberProfileItems",
                column: "MemberProfileItemTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProfileItemTemplates_CommunityId",
                schema: "Member",
                table: "MemberProfileItemTemplates",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_CommunityId",
                schema: "Member",
                table: "Members",
                column: "CommunityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMemberRelation",
                schema: "Member");

            migrationBuilder.DropTable(
                name: "MemberProfileItems",
                schema: "Member");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "Member");

            migrationBuilder.DropTable(
                name: "MemberProfileItemTemplates",
                schema: "Member");

            migrationBuilder.DropTable(
                name: "Members",
                schema: "Member");

            migrationBuilder.DropTable(
                name: "Communities",
                schema: "Member");
        }
    }
}
