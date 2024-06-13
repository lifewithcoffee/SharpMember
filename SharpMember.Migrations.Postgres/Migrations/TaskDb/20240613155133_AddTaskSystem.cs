using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SharpMember.Migrations.Postgres.Migrations.TaskDb
{
    /// <inheritdoc />
    public partial class AddTaskSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "task");

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "task",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CommunityId = table.Column<int>(type: "integer", nullable: false),
                    MemberId = table.Column<int>(type: "integer", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskLabels",
                schema: "task",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLabels", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Milestones",
                schema: "task",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    DueTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milestones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Milestones_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "task",
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkTasks",
                schema: "task",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MilestoneId = table.Column<int>(type: "integer", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Details = table.Column<string>(type: "text", nullable: true),
                    Done = table.Column<bool>(type: "boolean", nullable: false),
                    Pinned = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTasks_Milestones_MilestoneId",
                        column: x => x.MilestoneId,
                        principalSchema: "task",
                        principalTable: "Milestones",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkTasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "task",
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CheckListItems",
                schema: "task",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkTaskId = table.Column<int>(type: "integer", nullable: true),
                    Done = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Comments = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListItems_WorkTasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalSchema: "task",
                        principalTable: "WorkTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskComments",
                schema: "task",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkTaskId = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskComments_WorkTasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalSchema: "task",
                        principalTable: "WorkTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkTaskLabelRelations",
                schema: "task",
                columns: table => new
                {
                    WorkTaskId = table.Column<int>(type: "integer", nullable: false),
                    TaskLabelId = table.Column<int>(type: "integer", nullable: false),
                    TaskLabelName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTaskLabelRelations", x => new { x.WorkTaskId, x.TaskLabelId });
                    table.ForeignKey(
                        name: "FK_WorkTaskLabelRelations_TaskLabels_TaskLabelName",
                        column: x => x.TaskLabelName,
                        principalSchema: "task",
                        principalTable: "TaskLabels",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_WorkTaskLabelRelations_WorkTasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalSchema: "task",
                        principalTable: "WorkTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItems_WorkTaskId",
                schema: "task",
                table: "CheckListItems",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_ProjectId",
                schema: "task",
                table: "Milestones",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_WorkTaskId",
                schema: "task",
                table: "TaskComments",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTaskLabelRelations_TaskLabelName",
                schema: "task",
                table: "WorkTaskLabelRelations",
                column: "TaskLabelName");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_MilestoneId",
                schema: "task",
                table: "WorkTasks",
                column: "MilestoneId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_ProjectId",
                schema: "task",
                table: "WorkTasks",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckListItems",
                schema: "task");

            migrationBuilder.DropTable(
                name: "TaskComments",
                schema: "task");

            migrationBuilder.DropTable(
                name: "WorkTaskLabelRelations",
                schema: "task");

            migrationBuilder.DropTable(
                name: "TaskLabels",
                schema: "task");

            migrationBuilder.DropTable(
                name: "WorkTasks",
                schema: "task");

            migrationBuilder.DropTable(
                name: "Milestones",
                schema: "task");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "task");
        }
    }
}
