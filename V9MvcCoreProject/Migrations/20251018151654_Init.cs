using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace V9MvcCoreProject.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationFunctionalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionalityName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ActionMethodName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsMenuItem = table.Column<bool>(type: "bit", nullable: true),
                    MenuReferenceName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationFunctionalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CNIC = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    RoleTemplateId = table.Column<int>(type: "int", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    State = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "NULL"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControllerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FormName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IconCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListTypeId = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Application = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Logged = table.Column<DateTime>(type: "datetime", nullable: true),
                    Level = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Logger = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Callsite = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DeviceID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IP = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FunctionName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ControllerName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    RequestParameters = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    RequestResponse = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Channel = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    LogId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    RequestDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ErrorCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequestId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    FormName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullAccess = table.Column<bool>(type: "bit", nullable: true),
                    ApplicationFunctionalityId = table.Column<int>(type: "int", nullable: true),
                    AllowAccess = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Controller = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Action = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Path = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Method = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    QueryString = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    RequestBody = table.Column<string>(type: "text", nullable: false),
                    ResponseBody = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Datetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsException = table.Column<bool>(type: "bit", nullable: false),
                    Exception = table.Column<string>(type: "text", nullable: false),
                    IsAjaxRequest = table.Column<bool>(type: "bit", nullable: false),
                    LogId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivityLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserHistoryLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ActionMethod = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    NewValueJson = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    OldValueJson = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHistoryLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    LoginTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LoginStatus = table.Column<bool>(type: "bit", nullable: false),
                    StatusMessage = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionTemplateDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    FormName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    FunctionalityId = table.Column<int>(type: "int", nullable: false),
                    IsAllow = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionTemplateDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionTemplateDetail_PermissionTemplate",
                        column: x => x.TemplateId,
                        principalTable: "PermissionTemplate",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ApplicationFunctionalities",
                columns: new[] { "Id", "ActionMethodName", "FormId", "FunctionalityName", "IsActive", "IsMenuItem", "MenuReferenceName" },
                values: new object[,]
                {
                    { 1, "PermissionTemplate", 1, "Add Role", true, true, "User Permission" },
                    { 2, "ChangePermissionTemplate", 1, "Change Role", true, true, "User Permission" },
                    { 3, "Index", 2, "Index", true, true, "Home" },
                    { 4, "Privacy", 2, "Privacy", true, true, "Home" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1, "CONC-STATIC-001", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CNIC", "City", "ConcurrencyStamp", "CreatedBy", "DateCreated", "Email", "EmailConfirmed", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImageUrl", "RoleTemplateId", "SecurityStamp", "State", "TwoFactorEnabled", "UpdatedBy", "UserName" },
                values: new object[] { 1, 0, "4210148778829", null, "CONC-STATIC-001", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "abdul_mussawir@hotmail.com", true, true, false, null, "ABDUL_MUSSAWIR@HOTMAIL.com", "ABDULMUSSAWIR", "AQAAAAIAAYagAAAAEDN/RRsQwvn4ZWb/dI201CZbl7rgbfCY02kOuHqICxhOBNTyA8Ul8zL7RJmktCoSbw==", null, false, null, 1, "SEC-STATIC-001", null, false, null, "abdulmussawir" });

            migrationBuilder.InsertData(
                table: "FormDetail",
                columns: new[] { "Id", "ActionName", "ControllerName", "DisplayName", "DisplayOrder", "FormName", "IconCode", "IsActive" },
                values: new object[,]
                {
                    { 1, "Index", "Permission", "User Permission", 1, "Permission", null, true },
                    { 2, "ViewsHome", "Home", "Home", 2, "ViewsHome", null, true }
                });

            migrationBuilder.InsertData(
                table: "PermissionTemplate",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsActive", "TemplateName", "UpdatedBy", "UpdatedDate" },
                values: new object[] { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Super Admin", null, null });

            migrationBuilder.InsertData(
                table: "PermissionTemplateDetail",
                columns: new[] { "Id", "FormName", "FunctionalityId", "IsAllow", "TemplateId" },
                values: new object[,]
                {
                    { 1, "Permission", 1, true, 1 },
                    { 2, "Permission", 2, true, 1 },
                    { 3, "ViewsHom", 3, true, 1 },
                    { 4, "ViewsHom", 4, true, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IDX_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IDX_IcNumber",
                table: "AspNetUsers",
                column: "CNIC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Salary",
                table: "Employees",
                column: "Salary");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionTemplateDetail_TemplateId",
                table: "PermissionTemplateDetail",
                column: "TemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationFunctionalities");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "FormDetail");

            migrationBuilder.DropTable(
                name: "ListItems");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "PermissionTemplateDetail");

            migrationBuilder.DropTable(
                name: "UserAccess");

            migrationBuilder.DropTable(
                name: "UserActivityLogs");

            migrationBuilder.DropTable(
                name: "UserHistoryLogs");

            migrationBuilder.DropTable(
                name: "UserLoginLogs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PermissionTemplate");
        }
    }
}
