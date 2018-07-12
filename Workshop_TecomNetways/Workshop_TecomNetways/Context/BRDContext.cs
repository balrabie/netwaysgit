using System.Data.Entity;
using Workshop_TecomNetways.Models;



namespace Workshop_TecomNetways.Context
{
    public class BRDContext : DbContext
    {
        public virtual DbSet<Homebanner> Homebanner { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<PeopleGroup> PeopleGroup { get; set; }
        public virtual DbSet<Award> Award { get; set; }
        public virtual DbSet<AwardCriteria> AwardCriteria { get; set; }
        public virtual DbSet<AwardQuote> AwardQuote { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<FeedbackRequest> FeedbackRequest { get; set; }
        public virtual DbSet<SocialMediaAccount> SocialMediaAccount { get; set; }
        public virtual DbSet<PhotoAlbum> PhotoAlbum { get; set; }
        public virtual DbSet<VideoAlbum> VideoAlbum { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<Video> Video { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Announcement> Announcement { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Nationality> Nationality { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<OnlineParticipationRequest> OnlineParticipationRequest { get; set; }
        public virtual DbSet<Criteria> Criteria { get; set; }
        public virtual DbSet<SubCriteria> SubCriteria { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureHomebanner(modelBuilder);

            ConfigureLocation(modelBuilder);

            ConfigurePeopleGroup(modelBuilder);

            ConfigureAward(modelBuilder);

            ConfigureAwardCriteria(modelBuilder);

            ConfigureAwardQuote(modelBuilder);

            ConfigureNews(modelBuilder);

            ConfigureEvent(modelBuilder);

            ConfigureAnnouncement(modelBuilder);

            ConfigureContactUs(modelBuilder);

            ConfigureFeedbackRequest(modelBuilder);

            ConfigureSocialMediaAccount(modelBuilder);

            ConfigurePhoto(modelBuilder);

            ConfigureVideo(modelBuilder);

            ConfigureCountry(modelBuilder);

            ConfigureNationality(modelBuilder);

            ConfigureAddress(modelBuilder);

            ConfigureUser(modelBuilder);

            ConfigureFeedbackReceiver(modelBuilder);

            ConfigureCriteria(modelBuilder);

            ConfigureSubCriteria(modelBuilder);

            ConfigureOnlineParticipationRequest(modelBuilder);
        }

        private static void ConfigureOnlineParticipationRequest(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OnlineParticipationRequest>().ToTable("OnlineParticipationRequest");
            modelBuilder.Entity<OnlineParticipationRequest>().Property(e => e.Title).HasMaxLength(50);
            modelBuilder.Entity<OnlineParticipationRequest>().Property(e => e.TrackingCode)
                .HasMaxLength(50).IsRequired();
        }

        private static void ConfigureSubCriteria(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubCriteria>().ToTable("SubCriteria");
            modelBuilder.Entity<SubCriteria>().Property(e => e.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<SubCriteria>().Property(e => e.Comments).HasMaxLength(255);
            modelBuilder.Entity<SubCriteria>().Property(e => e.CriteriaID).IsRequired();
        }

        private static void ConfigureCriteria(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Criteria>().ToTable("Criteria");
            modelBuilder.Entity<Criteria>().Property(e => e.Title).HasMaxLength(50).IsRequired();
        }

        private static void ConfigureFeedbackReceiver(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedbackReceiver>().ToTable("FeedbackReceiver");
            modelBuilder.Entity<FeedbackReceiver>().Property(e => e.Email).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<FeedbackReceiver>().Property(e => e.IsActive).IsRequired();
        }

        private static void ConfigureUser(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().Property(e => e.Email).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(e => e.Password).HasMaxLength(1000).IsRequired()
                .HasColumnName("Password Hash");
            modelBuilder.Entity<User>().Property(e => e.Salt).IsRequired();
            modelBuilder.Entity<User>().Property(e => e.PassportNumber).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(e => e.Gender).HasMaxLength(1).IsRequired();
            modelBuilder.Entity<User>().Property(e => e.SchoolName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(e => e.TeachingArea).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(e => e.NationalityID).IsRequired();
            modelBuilder.Entity<User>().Property(e => e.PeopleGroupID).IsRequired();
        }

        private static void ConfigureAddress(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Address>().Property(e => e.Country).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Address>().Property(e => e.City).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Address>().Property(e => e.Street).HasMaxLength(50).IsRequired();
        }

        private static void ConfigureNationality(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nationality>().ToTable("Nationality");
            modelBuilder.Entity<Nationality>().Property(e => e.Name).HasMaxLength(50).IsRequired();
        }

        private static void ConfigureCountry(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<Country>().Property(e => e.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Country>().Property(e => e.CountryCode).HasMaxLength(20).IsRequired();
        }

        private static void ConfigureVideo(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VideoAlbum>().ToTable("VideoAlbum");
            modelBuilder.Entity<VideoAlbum>().Property(e => e.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<VideoAlbum>().Property(e => e.Details).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<VideoAlbum>().Property(e => e.CoverImage).IsRequired();

            modelBuilder.Entity<Video>().ToTable("Video");
            modelBuilder.Entity<Video>().Property(e => e.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Video>().Property(e => e.URL).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<Video>().Property(e => e.Description).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<Video>().Property(e => e.PostingDate).IsRequired();
        }

        private static void ConfigurePhoto(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhotoAlbum>().ToTable("PhotoAlbum");
            modelBuilder.Entity<PhotoAlbum>().Property(e => e.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<PhotoAlbum>().Property(e => e.Details).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<PhotoAlbum>().Property(e => e.CoverImage).IsRequired();

            modelBuilder.Entity<Photo>().ToTable("Photo");
            modelBuilder.Entity<Photo>().Property(e => e.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Photo>().Property(e => e.Image).IsRequired();
            modelBuilder.Entity<Photo>().Property(e => e.Description).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<Photo>().Property(e => e.PostingDate).IsRequired();
        }

        private static void ConfigureContactUs(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactUs>().ToTable("ContactUs");
            modelBuilder.Entity<ContactUs>().Property(e => e.Email).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<ContactUs>().Property(e => e.WorkHours).HasMaxLength(50).IsRequired();
        }

        private static void ConfigureSocialMediaAccount(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SocialMediaAccount>().ToTable("SocialMediaAccount");
            modelBuilder.Entity<SocialMediaAccount>().Property(e => e.URL).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<SocialMediaAccount>().Property(e => e.ContactUsID).IsRequired();
        }

        private static void ConfigureFeedbackRequest(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedbackRequest>().ToTable("FeedbackRequest");
            modelBuilder.Entity<FeedbackRequest>().Property(e => e.Submitter).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<FeedbackRequest>().Property(e => e.MobileNumber).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<FeedbackRequest>().Property(e => e.Email).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<FeedbackRequest>().Property(e => e.Subject).HasMaxLength(50);
            modelBuilder.Entity<FeedbackRequest>().Property(e => e.Details).HasMaxLength(255);
            modelBuilder.Entity<FeedbackRequest>().Property(e => e.CountryID).IsRequired();
        }

        private static void ConfigureAnnouncement(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>().ToTable("Announcement");
            modelBuilder.Entity<Announcement>().Property(e => e.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Announcement>().Property(e => e.Details).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<Announcement>().Property(e => e.StartTime).IsRequired();
            modelBuilder.Entity<Announcement>().Property(e => e.EndTime).IsRequired();
            modelBuilder.Entity<Announcement>().Property(e => e.Image).IsRequired();
            modelBuilder.Entity<Announcement>().Property(e => e.Summary).HasMaxLength(255);
        }

        private static void ConfigureEvent(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<Event>().Property(e => e.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Event>().Property(e => e.Details).HasMaxLength(255);
            modelBuilder.Entity<Event>().Property(e => e.StartTime).IsRequired();
            modelBuilder.Entity<Event>().Property(e => e.EndTime).IsRequired();
            modelBuilder.Entity<Event>().Property(e => e.OrganisedBy).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Event>().Property(e => e.LocationID).IsRequired();
            modelBuilder.Entity<Event>().Property(e => e.EndTime).IsRequired();
            modelBuilder.Entity<Event>().Property(e => e.Email).HasMaxLength(50);
            modelBuilder.Entity<Event>().Property(e => e.URL).HasMaxLength(255);
            modelBuilder.Entity<Event>().Property(e => e.Category).HasMaxLength(50);
        }

        private static void ConfigureNews(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>().ToTable("News");
            modelBuilder.Entity<News>().Property(e => e.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<News>().Property(e => e.Details).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<News>().Property(e => e.StartTime).IsRequired();
            modelBuilder.Entity<News>().Property(e => e.EndTime).IsRequired();
        }

        private static void ConfigureAwardQuote(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AwardQuote>().ToTable("AwardQuote");
            modelBuilder.Entity<AwardQuote>().Property(e => e.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<AwardQuote>().Property(e => e.Image).IsRequired();
            modelBuilder.Entity<AwardQuote>().Property(e => e.Description).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<AwardQuote>().Property(e => e.ContactName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<AwardQuote>().Property(e => e.JobTitle).HasMaxLength(50).IsRequired();
        }

        private static void ConfigureAwardCriteria(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AwardCriteria>().ToTable("AwardCriteria");
            modelBuilder.Entity<AwardCriteria>().Property(e => e.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<AwardCriteria>().Property(e => e.Logo).IsRequired();
            modelBuilder.Entity<AwardCriteria>().Property(e => e.Weight).IsRequired();
        }

        private static void ConfigureAward(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Award>().ToTable("Award");
            modelBuilder.Entity<Award>().Property(e => e.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Award>().Property(e => e.Logo).IsRequired();
            modelBuilder.Entity<Award>().Property(e => e.Number).IsRequired();
        }

        private static void ConfigurePeopleGroup(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeopleGroup>().ToTable("PeopleGroup");
            modelBuilder.Entity<PeopleGroup>().Property(e => e.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<PeopleGroup>().Property(e => e.Title).HasMaxLength(50);
        }

        private static void ConfigureLocation(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<Location>().Property(e => e.Country).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Location>().Property(e => e.City).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Location>().Property(e => e.Longitude).IsRequired();
            modelBuilder.Entity<Location>().Property(e => e.Latitude).IsRequired();
            modelBuilder.Entity<Location>().Property(e => e.ContactUsID).IsRequired();
        }

        private static void ConfigureHomebanner(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Homebanner>().ToTable("Homebanner");
            modelBuilder.Entity<Homebanner>().Property(e => e.Title).HasMaxLength(50);
            modelBuilder.Entity<Homebanner>().Property(e => e.Image).IsRequired();
            modelBuilder.Entity<Homebanner>().Property(e => e.StartTime).IsRequired();
            modelBuilder.Entity<Homebanner>().Property(e => e.EndTime).IsRequired();
            modelBuilder.Entity<Homebanner>().Property(e => e.URL).HasMaxLength(255);
        }
    }
}