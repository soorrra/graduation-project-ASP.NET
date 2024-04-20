using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NuGet.Protocol.Plugins;
using Petsitter.Data;

namespace Petsitter.Models
{
    public partial class PetsitterContext : DbContext
    {

        public PetsitterContext(DbContextOptions<PetsitterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Availability> Availabilities { get; set; } = null!;
        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<BookingPet> BookingPets { get; set; } = null!;
        public virtual DbSet<Pet> Pets { get; set; } = null!;
        public virtual DbSet<PetType> PetTypes { get; set; } = null!;
        public virtual DbSet<ServiceType> ServiceTypes { get; set; } = null!;
        public virtual DbSet<Sitter> Sitters { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserType> UserTypes { get; set; } = null!;
        public virtual DbSet<IPN> IPNs { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Chat> Chats { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= DESKTOP-R1OH460\\SQLEXPRESS03;Database=Petsitter;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Availability>(entity =>
            {
                entity.ToTable("Availability");

                entity.Property(e => e.AvailabilityId).HasColumnName("availabilityID");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("endDate");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.BookingId).HasColumnName("bookingID");

                entity.Property(e => e.Complaint)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("complaint");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("endDate");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.Review)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("review");

                entity.Property(e => e.SitterId).HasColumnName("sitterID");

                entity.Property(e => e.SpecialRequests)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("specialRequests");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Sitter)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.SitterId)
                    .HasConstraintName("FK__Booking__userID__3B75D760");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Booking__userID__3C69FB99");
            });

            modelBuilder.Entity<BookingPet>(entity =>
            {
                entity.HasKey(e => new { e.BookingId, e.PetId });

                entity.ToTable("BookingPet");

                entity.Property(e => e.BookingId).HasColumnName("bookingID");

                entity.Property(e => e.PetId).HasColumnName("petID");

                entity.HasOne(d => d.Booking)
                    .WithMany()
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__BookingPe__booki__3E52440B");

                entity.HasOne(d => d.Pet)
                    .WithMany()
                    .HasForeignKey(d => d.PetId)
                    .HasConstraintName("FK__BookingPe__petID__3F466844");
            });

            modelBuilder.Entity<Pet>(entity =>
            {
                entity.ToTable("Pet");

                entity.Property(e => e.PetId).HasColumnName("petID");

                entity.Property(e => e.BirthYear).HasColumnName("birthYear");

                entity.Property(e => e.Instructions)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("instructions");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PetSize)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("petSize");

                entity.Property(e => e.PetType)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("petType");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("sex")
                    .IsFixedLength();

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.PetTypeNavigation)
                    .WithMany(p => p.Pets)
                    .HasForeignKey(d => d.PetType)
                    .HasConstraintName("FK__Pet__petType__32E0915F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Pets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Pet__userID__31EC6D26");
            });

            modelBuilder.Entity<PetType>(entity =>
            {
                entity.HasKey(e => e.PetType1)
                    .HasName("PK__PetType__3408B3AE78D2F573");

                entity.ToTable("PetType");

                entity.Property(e => e.PetType1)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("petType");
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.HasKey(e => e.ServiceType1)
                    .HasName("PK_ServiceType");

                entity.ToTable("ServiceType");


                entity.Property(e => e.ServiceType1)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("serviceName");
            });


            modelBuilder.Entity<Sitter>(entity =>
            {
                entity.ToTable("Sitter");

                entity.Property(e => e.SitterId).HasColumnName("sitterID");

                entity.Property(e => e.ProfileBio)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("profileBio");

                entity.Property(e => e.RatePerPetPerDay)
                    .HasColumnType("money")
                    .HasColumnName("ratePerPetPerDay");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sitters)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Sitter__userID__29572725");

                entity.HasMany(d => d.Availabilities)
                    .WithMany(p => p.Sitters)
                    .UsingEntity<Dictionary<string, object>>(
                        "SitterAvailability",
                        l => l.HasOne<Availability>().WithMany().HasForeignKey("AvailabilityId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__SitterAva__avail__38996AB5"),
                        r => r.HasOne<Sitter>().WithMany().HasForeignKey("SitterId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__SitterAva__sitte__37A5467C"),
                        j =>
                        {
                            j.HasKey("SitterId", "AvailabilityId").HasName("PK__SitterAv__2595E50B9A99093F");

                            j.ToTable("SitterAvailability");

                            j.IndexerProperty<int>("SitterId").HasColumnName("sitterID");

                            j.IndexerProperty<int>("AvailabilityId").HasColumnName("availabilityID");
                        });

                entity.HasMany(d => d.PetTypes)
                    .WithMany(p => p.Sitters)
                    .UsingEntity<Dictionary<string, object>>(
                        "SitterPetType",
                        l => l.HasOne<PetType>().WithMany().HasForeignKey("PetType").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__SitterPet__petTy__2F10007B"),
                        r => r.HasOne<Sitter>().WithMany().HasForeignKey("SitterId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__SitterPet__sitte__2E1BDC42"),
                        j =>
                        {
                            j.HasKey("SitterId", "PetType").HasName("PK__SitterPe__9D2BD2361D4E7ABE");

                            j.ToTable("SitterPetType");

                            j.IndexerProperty<int>("SitterId").HasColumnName("sitterID");

                            j.IndexerProperty<string>("PetType").HasMaxLength(25).IsUnicode(false).HasColumnName("petType");
                        });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber")
                    .IsFixedLength();

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("postalCode")
                    .IsFixedLength();

                entity.Property(e => e.StreetAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("streetAddress");

                entity.Property(e => e.UserType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userType");

                entity.HasOne(d => d.UserTypeNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserType)
                    .HasConstraintName("FK__User__userType__267ABA7A");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.HasKey(e => e.UserType1)
                    .HasName("PK__UserType__73837898450D54D6");

                entity.ToTable("UserType");

                entity.Property(e => e.UserType1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userType");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Messages");

                entity.Property(e => e.messageID).HasColumnName("messageID");
                entity.Property(e => e.chatID).HasColumnName("chatID");
                entity.Property(e => e.fromUserID).HasColumnName("fromUserID");
                entity.Property(e => e.toUserID).HasColumnName("toUserID");
                entity.Property(e => e.messageText).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Chat)
                    .WithMany()
                    .HasForeignKey(d => d.chatID)
                    .HasConstraintName("FK_Messages_Chats");

                entity.HasOne(d => d.FromUser)
                    .WithMany()
                    .HasForeignKey(d => d.fromUserID)
                    .HasConstraintName("FK_Messages_FromUsers");

                entity.HasOne(d => d.ToUser)
                    .WithMany()
                    .HasForeignKey(d => d.toUserID)
                    .HasConstraintName("FK_Messages_ToUsers");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("Chats");

                entity.Property(e => e.chatID).HasColumnName("chatID");
                entity.Property(e => e.user1ID).HasColumnName("user1ID");
                entity.Property(e => e.user2ID).HasColumnName("user2ID");

                entity.HasOne(d => d.User1)
                    .WithMany()
                    .HasForeignKey(d => d.user1ID)
                    .HasConstraintName("FK_Chats_User1");

                entity.HasOne(d => d.User2)
                    .WithMany()
                    .HasForeignKey(d => d.user2ID)
                    .HasConstraintName("FK_Chats_User2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
