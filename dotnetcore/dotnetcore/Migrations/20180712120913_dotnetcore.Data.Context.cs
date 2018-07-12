using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnetcore.Migrations
{
    public partial class dotnetcoreDataContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Country = table.Column<string>(maxLength: 50, nullable: false),
                    City = table.Column<string>(maxLength: 50, nullable: false),
                    Street = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Announcement",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Image = table.Column<byte[]>(nullable: false),
                    Summary = table.Column<string>(maxLength: 255, nullable: true),
                    Details = table.Column<string>(maxLength: 255, nullable: false),
                    ShowAsPushNotification = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcement", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Award",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Logo = table.Column<byte[]>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Award", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AwardCriteria",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Logo = table.Column<byte[]>(nullable: false),
                    Weight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AwardCriteria", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AwardQuote",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Image = table.Column<byte[]>(nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: false),
                    ContactName = table.Column<string>(maxLength: 50, nullable: false),
                    JobTitle = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AwardQuote", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    WorkHours = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CountryCode = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackReceiver",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackReceiver", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Homebanner",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Image = table.Column<byte[]>(nullable: false),
                    URL = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homebanner", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Nationality",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationality", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    Details = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PeopleGroup",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleGroup", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Image = table.Column<byte[]>(nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: false),
                    PostingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PhotoAlbum",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    CoverImage = table.Column<byte[]>(nullable: false),
                    Details = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoAlbum", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Video",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    URL = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: false),
                    PostingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VideoAlbum",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    CoverImage = table.Column<byte[]>(nullable: false),
                    Details = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoAlbum", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Country = table.Column<string>(maxLength: 50, nullable: false),
                    City = table.Column<string>(maxLength: 50, nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    ContactUsID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Location_ContactUs_ContactUsID",
                        column: x => x.ContactUsID,
                        principalTable: "ContactUs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialMediaAccount",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    URL = table.Column<string>(maxLength: 255, nullable: false),
                    ContactUsID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaAccount", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SocialMediaAccount_ContactUs_ContactUsID",
                        column: x => x.ContactUsID,
                        principalTable: "ContactUs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackRequest",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Submitter = table.Column<string>(maxLength: 50, nullable: false),
                    MobileNumber = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Subject = table.Column<string>(maxLength: 50, nullable: true),
                    Details = table.Column<string>(maxLength: 255, nullable: true),
                    CountryID = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackRequest", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FeedbackRequest_Country_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Country",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeopleGroup-Announcement",
                columns: table => new
                {
                    PeopleGroupID = table.Column<int>(nullable: false),
                    AnnouncementID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleGroup-Announcement", x => new { x.PeopleGroupID, x.AnnouncementID });
                    table.ForeignKey(
                        name: "FK_PeopleGroup-Announcement_Announcement_AnnouncementID",
                        column: x => x.AnnouncementID,
                        principalTable: "Announcement",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeopleGroup-Announcement_PeopleGroup_PeopleGroupID",
                        column: x => x.PeopleGroupID,
                        principalTable: "PeopleGroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(name: "Password Hash", maxLength: 1000, nullable: false),
                    Salt = table.Column<byte[]>(nullable: false),
                    PassportNumber = table.Column<string>(maxLength: 50, nullable: false),
                    Gender = table.Column<string>(maxLength: 1, nullable: false),
                    SchoolName = table.Column<string>(maxLength: 50, nullable: false),
                    TeachingArea = table.Column<string>(maxLength: 50, nullable: false),
                    NationalityID = table.Column<int>(nullable: false),
                    PeopleGroupID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                    table.ForeignKey(
                        name: "FK_User_Nationality_NationalityID",
                        column: x => x.NationalityID,
                        principalTable: "Nationality",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_PeopleGroup_PeopleGroupID",
                        column: x => x.PeopleGroupID,
                        principalTable: "PeopleGroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoAlbum-Photo",
                columns: table => new
                {
                    PhotoID = table.Column<int>(nullable: false),
                    PhotoAlbumID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoAlbum-Photo", x => new { x.PhotoAlbumID, x.PhotoID });
                    table.ForeignKey(
                        name: "FK_PhotoAlbum-Photo_PhotoAlbum_PhotoAlbumID",
                        column: x => x.PhotoAlbumID,
                        principalTable: "PhotoAlbum",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoAlbum-Photo_Photo_PhotoID",
                        column: x => x.PhotoID,
                        principalTable: "Photo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoAlbum-Video",
                columns: table => new
                {
                    VideoID = table.Column<int>(nullable: false),
                    VideoAlbumID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoAlbum-Video", x => new { x.VideoAlbumID, x.VideoID });
                    table.ForeignKey(
                        name: "FK_VideoAlbum-Video_VideoAlbum_VideoAlbumID",
                        column: x => x.VideoAlbumID,
                        principalTable: "VideoAlbum",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoAlbum-Video_Video_VideoID",
                        column: x => x.VideoID,
                        principalTable: "Video",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    AllDayEvent = table.Column<bool>(nullable: false),
                    OrganisedBy = table.Column<string>(maxLength: 50, nullable: false),
                    ContactNumber = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Details = table.Column<string>(maxLength: 255, nullable: true),
                    Category = table.Column<string>(maxLength: 50, nullable: true),
                    URL = table.Column<string>(maxLength: 255, nullable: true),
                    LocationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Event_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnlineParticipationRequest",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    TrackingCode = table.Column<string>(maxLength: 50, nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    CountryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineParticipationRequest", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OnlineParticipationRequest_Country_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Country",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnlineParticipationRequest_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Expiry = table.Column<DateTime>(nullable: false),
                    TokenIsUsed = table.Column<bool>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeopleGroup-Event",
                columns: table => new
                {
                    PeopleGroupID = table.Column<int>(nullable: false),
                    EventID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleGroup-Event", x => new { x.PeopleGroupID, x.EventID });
                    table.ForeignKey(
                        name: "FK_PeopleGroup-Event_Event_EventID",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeopleGroup-Event_PeopleGroup_PeopleGroupID",
                        column: x => x.PeopleGroupID,
                        principalTable: "PeopleGroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Criteria",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    OnlineParticipationRequestID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criteria", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Criteria_OnlineParticipationRequest_OnlineParticipationRequestID",
                        column: x => x.OnlineParticipationRequestID,
                        principalTable: "OnlineParticipationRequest",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubCriteria",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Comments = table.Column<string>(maxLength: 255, nullable: true),
                    Document = table.Column<byte[]>(nullable: true),
                    CriteriaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCriteria", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SubCriteria_Criteria_CriteriaID",
                        column: x => x.CriteriaID,
                        principalTable: "Criteria",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_OnlineParticipationRequestID",
                table: "Criteria",
                column: "OnlineParticipationRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_LocationID",
                table: "Event",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackRequest_CountryID",
                table: "FeedbackRequest",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Location_ContactUsID",
                table: "Location",
                column: "ContactUsID");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineParticipationRequest_CountryID",
                table: "OnlineParticipationRequest",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineParticipationRequest_UserID",
                table: "OnlineParticipationRequest",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PeopleGroup-Announcement_AnnouncementID",
                table: "PeopleGroup-Announcement",
                column: "AnnouncementID");

            migrationBuilder.CreateIndex(
                name: "IX_PeopleGroup-Event_EventID",
                table: "PeopleGroup-Event",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoAlbum-Photo_PhotoID",
                table: "PhotoAlbum-Photo",
                column: "PhotoID");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaAccount_ContactUsID",
                table: "SocialMediaAccount",
                column: "ContactUsID");

            migrationBuilder.CreateIndex(
                name: "IX_SubCriteria_CriteriaID",
                table: "SubCriteria",
                column: "CriteriaID");

            migrationBuilder.CreateIndex(
                name: "IX_User_NationalityID",
                table: "User",
                column: "NationalityID");

            migrationBuilder.CreateIndex(
                name: "IX_User_PeopleGroupID",
                table: "User",
                column: "PeopleGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserID",
                table: "UserToken",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoAlbum-Video_VideoID",
                table: "VideoAlbum-Video",
                column: "VideoID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Award");

            migrationBuilder.DropTable(
                name: "AwardCriteria");

            migrationBuilder.DropTable(
                name: "AwardQuote");

            migrationBuilder.DropTable(
                name: "FeedbackReceiver");

            migrationBuilder.DropTable(
                name: "FeedbackRequest");

            migrationBuilder.DropTable(
                name: "Homebanner");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "PeopleGroup-Announcement");

            migrationBuilder.DropTable(
                name: "PeopleGroup-Event");

            migrationBuilder.DropTable(
                name: "PhotoAlbum-Photo");

            migrationBuilder.DropTable(
                name: "SocialMediaAccount");

            migrationBuilder.DropTable(
                name: "SubCriteria");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "VideoAlbum-Video");

            migrationBuilder.DropTable(
                name: "Announcement");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "PhotoAlbum");

            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropTable(
                name: "Criteria");

            migrationBuilder.DropTable(
                name: "VideoAlbum");

            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "OnlineParticipationRequest");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Nationality");

            migrationBuilder.DropTable(
                name: "PeopleGroup");
        }
    }
}
