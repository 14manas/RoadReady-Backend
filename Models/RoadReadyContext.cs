using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RoadReady.Models
{
    public partial class RoadReadyContext : DbContext
    {
        public RoadReadyContext()
        {
        }

        public RoadReadyContext(DbContextOptions<RoadReadyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; } = null!;
        public virtual DbSet<CarDetail> CarDetails { get; set; } = null!;
        public virtual DbSet<CarImage> CarImages { get; set; } = null!;
        public virtual DbSet<CarReview> CarReviews { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<PaymentDetail> PaymentDetails { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;
        public virtual DbSet<State> States { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Usertype> Usertypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=26520541EA40500;Database=RoadReady;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.CarId).HasColumnName("car_id");

                entity.Property(e => e.AgentId).HasColumnName("agent_id");

                entity.Property(e => e.Available).HasColumnName("available");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.RatePerHour)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("rate_per_hour");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.AgentId)
                    .HasConstraintName("FK__Cars__agent_id__52593CB8");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Cars__location_i__534D60F1");
            });

            modelBuilder.Entity<CarDetail>(entity =>
            {
                entity.ToTable("Car_Details");

                entity.Property(e => e.CarDetailId).HasColumnName("car_detail_id");

                entity.Property(e => e.CarId).HasColumnName("car_id");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("color");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Make)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("make");

                entity.Property(e => e.Model)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("model");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.CarDetails)
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("FK__Car_Detai__car_i__4E88ABD4");
            });

            modelBuilder.Entity<CarImage>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__Car_Imag__DC9AC955B66D0B8B");

                entity.ToTable("Car_Images");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.CarId).HasColumnName("car_id");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("image_url");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.CarImages)
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("FK__Car_Image__car_i__4F7CD00D");
            });

            modelBuilder.Entity<CarReview>(entity =>
            {
                entity.HasKey(e => e.ReviewId)
                    .HasName("PK__Car_Revi__60883D9010EF0B34");

                entity.ToTable("Car_Reviews");

                entity.Property(e => e.ReviewId).HasColumnName("review_id");

                entity.Property(e => e.CarId).HasColumnName("car_id");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.ReviewText)
                    .HasColumnType("text")
                    .HasColumnName("review_text");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("timestamp");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.CarReviews)
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("FK__Car_Revie__car_i__5070F446");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CarReviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Car_Revie__user___5165187F");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.HasIndex(e => e.Cityname, "UQ__City__9AE3A3043ADD7CA9")
                    .IsUnique();

                entity.Property(e => e.Cityname)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("FK__City__StateId__5441852A");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.LocationName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("location_name");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.Cityid)
                    .HasConstraintName("FK__Locations__Cityi__5535A963");
            });

            modelBuilder.Entity<PaymentDetail>(entity =>
            {
                entity.HasKey(e => e.PaymentId)
                    .HasName("PK__Payment___ED1FC9EA04BC8D89");

                entity.ToTable("Payment_Details");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("payment_date");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("payment_method");

                entity.Property(e => e.ReservationId).HasColumnName("reservation_id");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.PaymentDetails)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK__Payment_D__reser__5629CD9C");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.Property(e => e.ReservationId).HasColumnName("reservation_id");

                entity.Property(e => e.CarId).HasColumnName("car_id");

                entity.Property(e => e.ReservationDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("reservation_date_time");

                entity.Property(e => e.ReturnDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("return_date_time");

                entity.Property(e => e.StatusName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("status_name");

                entity.Property(e => e.TotalCost)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("total_cost");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("FK__Reservati__car_i__571DF1D5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Reservati__user___5812160E");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("state");

                entity.Property(e => e.Statename)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username, "UQ__Users__536C85E4DCE24BA1")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Users__A9D10534C4B402D6")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Usertype)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Usertypeid)
                    .HasConstraintName("FK__Users__Usertypei__59063A47");
            });

            modelBuilder.Entity<Usertype>(entity =>
            {
                entity.ToTable("usertype");

                entity.HasIndex(e => e.Usertypename, "UQ__usertype__5737ED8045F4AFC9")
                    .IsUnique();

                entity.Property(e => e.Usertypename)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
