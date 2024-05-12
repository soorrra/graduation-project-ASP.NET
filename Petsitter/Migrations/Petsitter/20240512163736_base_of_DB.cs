using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petsitter.Migrations.Petsitter
{
    public partial class base_of_DB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Availability",
                columns: table => new
                {
                    availabilityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    startDate = table.Column<DateTime>(type: "date", nullable: true),
                    endDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availability", x => x.availabilityID);
                });

            migrationBuilder.CreateTable(
                name: "IPNs",
                columns: table => new
                {
                    paymentID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    custom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cart = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    create_time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payerID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payerFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payerLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payerMiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payerCountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payerStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    intent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentState = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPNs", x => x.paymentID);
                });

            migrationBuilder.CreateTable(
                name: "PetType",
                columns: table => new
                {
                    petType = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PetType__3408B3AE78D2F573", x => x.petType);
                });

            migrationBuilder.CreateTable(
                name: "ServiceType",
                columns: table => new
                {
                    serviceName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceType", x => x.serviceName);
                });

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    userType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserType__73837898450D54D6", x => x.userType);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    lastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    phoneNumber = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    streetAddress = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    city = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    postalCode = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    userType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ProfileImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userID);
                    table.ForeignKey(
                        name: "FK__User__userType__267ABA7A",
                        column: x => x.userType,
                        principalTable: "UserType",
                        principalColumn: "userType");
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    chatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user1ID = table.Column<int>(type: "int", nullable: false),
                    user2ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.chatID);
                    table.ForeignKey(
                        name: "FK_Chats_User1",
                        column: x => x.user1ID,
                        principalTable: "User",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chats_User2",
                        column: x => x.user2ID,
                        principalTable: "User",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    petID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    birthYear = table.Column<int>(type: "int", nullable: true),
                    sex = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    petSize = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    instructions = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: true),
                    userID = table.Column<int>(type: "int", nullable: true),
                    petType = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    PetImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.petID);
                    table.ForeignKey(
                        name: "FK__Pet__petType__32E0915F",
                        column: x => x.petType,
                        principalTable: "PetType",
                        principalColumn: "petType");
                    table.ForeignKey(
                        name: "FK__Pet__userID__31EC6D26",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "Sitter",
                columns: table => new
                {
                    sitterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ratePerPetPerDay = table.Column<decimal>(type: "money", nullable: true),
                    profileBio = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    userID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sitter", x => x.sitterID);
                    table.ForeignKey(
                        name: "FK__Sitter__userID__29572725",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    messageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chatID = table.Column<int>(type: "int", nullable: false),
                    fromUserID = table.Column<int>(type: "int", nullable: false),
                    toUserID = table.Column<int>(type: "int", nullable: false),
                    messageText = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    timestamp = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.messageID);
                    table.ForeignKey(
                        name: "FK_Messages_Chats",
                        column: x => x.chatID,
                        principalTable: "Chats",
                        principalColumn: "chatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_FromUsers",
                        column: x => x.fromUserID,
                        principalTable: "User",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_ToUsers",
                        column: x => x.toUserID,
                        principalTable: "User",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    bookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    price = table.Column<decimal>(type: "money", nullable: true),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startDate = table.Column<DateTime>(type: "date", nullable: true),
                    endDate = table.Column<DateTime>(type: "date", nullable: true),
                    specialRequests = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: true),
                    rating = table.Column<int>(type: "int", nullable: true),
                    review = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: true),
                    complaint = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: true),
                    sitterID = table.Column<int>(type: "int", nullable: true),
                    userID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.bookingID);
                    table.ForeignKey(
                        name: "FK__Booking__userID__3B75D760",
                        column: x => x.sitterID,
                        principalTable: "Sitter",
                        principalColumn: "sitterID");
                    table.ForeignKey(
                        name: "FK__Booking__userID__3C69FB99",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "SitterAvailability",
                columns: table => new
                {
                    sitterID = table.Column<int>(type: "int", nullable: false),
                    availabilityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SitterAv__2595E50B9A99093F", x => new { x.sitterID, x.availabilityID });
                    table.ForeignKey(
                        name: "FK__SitterAva__avail__38996AB5",
                        column: x => x.availabilityID,
                        principalTable: "Availability",
                        principalColumn: "availabilityID");
                    table.ForeignKey(
                        name: "FK__SitterAva__sitte__37A5467C",
                        column: x => x.sitterID,
                        principalTable: "Sitter",
                        principalColumn: "sitterID");
                });

            migrationBuilder.CreateTable(
                name: "SitterPetType",
                columns: table => new
                {
                    sitterID = table.Column<int>(type: "int", nullable: false),
                    petType = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SitterPe__9D2BD2361D4E7ABE", x => new { x.sitterID, x.petType });
                    table.ForeignKey(
                        name: "FK__SitterPet__petTy__2F10007B",
                        column: x => x.petType,
                        principalTable: "PetType",
                        principalColumn: "petType");
                    table.ForeignKey(
                        name: "FK__SitterPet__sitte__2E1BDC42",
                        column: x => x.sitterID,
                        principalTable: "Sitter",
                        principalColumn: "sitterID");
                });

            migrationBuilder.CreateTable(
                name: "SitterServiceType",
                columns: table => new
                {
                    sitterID = table.Column<int>(type: "int", nullable: false),
                    serviceName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKSitterSe095FA3AD9E661664", x => new { x.sitterID, x.serviceName });
                    table.ForeignKey(
                        name: "FKSitterSerservic30F848ED",
                        column: x => x.serviceName,
                        principalTable: "ServiceType",
                        principalColumn: "serviceName");
                    table.ForeignKey(
                        name: "FKSitterSersitte2FF462B4",
                        column: x => x.sitterID,
                        principalTable: "Sitter",
                        principalColumn: "sitterID");
                });

            migrationBuilder.CreateTable(
                name: "BookingPet",
                columns: table => new
                {
                    bookingID = table.Column<int>(type: "int", nullable: false),
                    petID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPet", x => new { x.bookingID, x.petID });
                    table.ForeignKey(
                        name: "FK__BookingPe__booki__3E52440B",
                        column: x => x.bookingID,
                        principalTable: "Booking",
                        principalColumn: "bookingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__BookingPe__petID__3F466844",
                        column: x => x.petID,
                        principalTable: "Pet",
                        principalColumn: "petID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_sitterID",
                table: "Booking",
                column: "sitterID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_userID",
                table: "Booking",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPet_petID",
                table: "BookingPet",
                column: "petID");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_user1ID",
                table: "Chats",
                column: "user1ID");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_user2ID",
                table: "Chats",
                column: "user2ID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_chatID",
                table: "Messages",
                column: "chatID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_fromUserID",
                table: "Messages",
                column: "fromUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_toUserID",
                table: "Messages",
                column: "toUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Pet_petType",
                table: "Pet",
                column: "petType");

            migrationBuilder.CreateIndex(
                name: "IX_Pet_userID",
                table: "Pet",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Sitter_userID",
                table: "Sitter",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_SitterAvailability_availabilityID",
                table: "SitterAvailability",
                column: "availabilityID");

            migrationBuilder.CreateIndex(
                name: "IX_SitterPetType_petType",
                table: "SitterPetType",
                column: "petType");

            migrationBuilder.CreateIndex(
                name: "IX_SitterServiceType_serviceName",
                table: "SitterServiceType",
                column: "serviceName");

            migrationBuilder.CreateIndex(
                name: "IX_User_userType",
                table: "User",
                column: "userType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingPet");

            migrationBuilder.DropTable(
                name: "IPNs");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "SitterAvailability");

            migrationBuilder.DropTable(
                name: "SitterPetType");

            migrationBuilder.DropTable(
                name: "SitterServiceType");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Availability");

            migrationBuilder.DropTable(
                name: "ServiceType");

            migrationBuilder.DropTable(
                name: "Sitter");

            migrationBuilder.DropTable(
                name: "PetType");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserType");
        }
    }
}
