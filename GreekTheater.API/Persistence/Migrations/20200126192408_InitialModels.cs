using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GreekTheater.API.Persistence.Migrations
{
    public partial class InitialModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTimeOffset>(nullable: true),
                    DateOfDeath = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Director",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTimeOffset>(nullable: true),
                    DateOfDeath = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Director", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Performance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    PremiereDate = table.Column<DateTimeOffset>(nullable: true),
                    DirectorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Performance_Director_DirectorId",
                        column: x => x.DirectorId,
                        principalTable: "Director",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Acting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 50, nullable: false),
                    ActorId = table.Column<int>(nullable: false),
                    PerformanceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Acting_Actor_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Acting_Performance_PerformanceId",
                        column: x => x.PerformanceId,
                        principalTable: "Performance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceGenre",
                columns: table => new
                {
                    GenreId = table.Column<byte>(nullable: false),
                    PerformanceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceGenre", x => new { x.GenreId, x.PerformanceId });
                    table.ForeignKey(
                        name: "FK_PerformanceGenre_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerformanceGenre_Performance_PerformanceId",
                        column: x => x.PerformanceId,
                        principalTable: "Performance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Actor",
                columns: new[] { "Id", "DateOfBirth", "DateOfDeath", "FirstName", "Guid", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(1976, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), null, "Christos", new Guid("7b75a444-994d-4936-96bf-9c3c0804e42d"), "Loulis" },
                    { 2, new DateTimeOffset(new DateTime(1988, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), null, "Iro", new Guid("a51f9b12-fb95-43e2-b010-733d53faf235"), "Mpezou" },
                    { 3, new DateTimeOffset(new DateTime(1948, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), new DateTimeOffset(new DateTime(2015, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Minas", new Guid("4d459461-59da-473d-938e-f5c7c03d12d7"), "Chatzisavvas" }
                });

            migrationBuilder.InsertData(
                table: "Director",
                columns: new[] { "Id", "DateOfBirth", "DateOfDeath", "FirstName", "Guid", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(1956, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), null, "Michail", new Guid("7b75a444-994d-4936-96bf-9c3c0804e42d"), "Marmarinos" },
                    { 2, new DateTimeOffset(new DateTime(1950, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), null, "Ioannis", new Guid("a51f9b12-fb95-43e2-b010-733d53faf235"), "Chouvardas" },
                    { 3, new DateTimeOffset(new DateTime(1962, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), null, "Efi", new Guid("4d459461-59da-473d-938e-f5c7c03d12d7"), "Theodorou" }
                });

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Guid", "Name" },
                values: new object[,]
                {
                    { (byte)1, new Guid("ca6a17f8-2045-4113-bbdb-3230427ae5cd"), "Musical" },
                    { (byte)2, new Guid("905d1d8f-d286-4ef2-a72e-c76353f342cb"), "Comedy" },
                    { (byte)3, new Guid("31e18a42-525d-42c2-99e6-93c6149a8d2f"), "Drama" },
                    { (byte)4, new Guid("d64e4e3c-6d5b-4818-9039-246861177572"), "Tragedy" },
                    { (byte)5, new Guid("8cc8da1e-56a7-487c-9786-548271ef9586"), "Opera" },
                    { (byte)6, new Guid("61cc3a5f-cab1-4104-8acf-3a0440ebdbc8"), "Theatre of the Absurd‎" }
                });

            migrationBuilder.InsertData(
                table: "Performance",
                columns: new[] { "Id", "DirectorId", "Guid", "PremiereDate", "Title" },
                values: new object[] { 2, 1, new Guid("a51f9b12-fb95-43e2-b010-733d53faf235"), new DateTimeOffset(new DateTime(2017, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Waiting for Godot" });

            migrationBuilder.InsertData(
                table: "Performance",
                columns: new[] { "Id", "DirectorId", "Guid", "PremiereDate", "Title" },
                values: new object[] { 1, 2, new Guid("7b75a444-994d-4936-96bf-9c3c0804e42d"), new DateTimeOffset(new DateTime(1996, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Don Juan" });

            migrationBuilder.InsertData(
                table: "Performance",
                columns: new[] { "Id", "DirectorId", "Guid", "PremiereDate", "Title" },
                values: new object[] { 3, 3, new Guid("4d459461-59da-473d-938e-f5c7c03d12d7"), null, "Loot" });

            migrationBuilder.InsertData(
                table: "Acting",
                columns: new[] { "Id", "ActorId", "Guid", "PerformanceId", "RoleName" },
                values: new object[,]
                {
                    { 3, 1, new Guid("ef8fd6fb-0f54-4247-b2c5-dcc851034b1d"), 2, "Vladimir" },
                    { 4, 2, new Guid("344ac739-b7ff-4b50-916b-77882688be7c"), 2, "Estragon" },
                    { 1, 1, new Guid("a9eb16c3-d4d6-4280-89ad-8e1bba35be38"), 1, "Don Alfonso" },
                    { 2, 3, new Guid("8b1877a9-1896-4b7b-a1c4-97957a919958"), 1, "Don Juan" },
                    { 5, 1, new Guid("8a113a93-2051-490d-9329-bad2920e9b97"), 3, "McLeavy" },
                    { 6, 2, new Guid("5c4a34ac-be45-4419-8a8b-612c020b38cf"), 3, "Fay" }
                });

            migrationBuilder.InsertData(
                table: "PerformanceGenre",
                columns: new[] { "GenreId", "PerformanceId" },
                values: new object[,]
                {
                    { (byte)3, 2 },
                    { (byte)6, 2 },
                    { (byte)1, 1 },
                    { (byte)2, 1 },
                    { (byte)3, 1 },
                    { (byte)2, 3 },
                    { (byte)3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Acting_ActorId",
                table: "Acting",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "Guid",
                table: "Acting",
                column: "Guid")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Acting_PerformanceId",
                table: "Acting",
                column: "PerformanceId");

            migrationBuilder.CreateIndex(
                name: "Guid",
                table: "Actor",
                column: "Guid")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "Guid",
                table: "Director",
                column: "Guid")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "Guid",
                table: "Genre",
                column: "Guid")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Performance_DirectorId",
                table: "Performance",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "Guid",
                table: "Performance",
                column: "Guid")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceGenre_PerformanceId",
                table: "PerformanceGenre",
                column: "PerformanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acting");

            migrationBuilder.DropTable(
                name: "PerformanceGenre");

            migrationBuilder.DropTable(
                name: "Actor");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Performance");

            migrationBuilder.DropTable(
                name: "Director");
        }
    }
}
