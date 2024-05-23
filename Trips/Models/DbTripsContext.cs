using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trips.Models;

public partial class DbTripsContext : DbContext
{
    public DbTripsContext()
    {
    }

    public DbTripsContext(DbContextOptions<DbTripsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AboutContant> AboutContants { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<HomeContant> HomeContants { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VisaCard> VisaCards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-I9DMUCI\\SQLEXPRESS;Initial Catalog=DB_Trips;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AboutContant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__about_co__3213E83F085B9E72");

            entity.ToTable("about_contant");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Pragraph)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("pragraph");
            entity.Property(e => e.Titel)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("titel");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC27387DD7AD");

            entity.ToTable("Booking");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TripId).HasColumnName("TripID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Trip).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TripId)
                .HasConstraintName("FK__Booking__TripID__74AE54BC");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Booking__UserID__75A278F5");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("category");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contact___3214EC27F9ABC5E9");

            entity.ToTable("Contact_Us");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MMessage)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("M_message");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__feedback__3214EC27B3B775C6");

            entity.ToTable("feedback");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Massage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__feedback__UserID__00200768");
        });

        modelBuilder.Entity<HomeContant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__home_con__3213E83F8F9CB605");

            entity.ToTable("home_contant");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Pragraph)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("pragraph");
            entity.Property(e => e.Titel)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("titel");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC2770847366");

            entity.ToTable("Payment");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.DatePay)
                .HasColumnType("date")
                .HasColumnName("date_Pay");

            entity.HasOne(d => d.Trip).WithMany(p => p.Payments)
                .HasForeignKey(d => d.TripId)
                .HasConstraintName("FK__Payment__TripId__7D439ABD");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Payment__UserId__7C4F7684");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC2705174794");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Trip__3214EC27C72E121D");

            entity.ToTable("Trip");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Destination)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.FinalDestination)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.StartDestination)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TDescription)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("T_Description");
            entity.Property(e => e.TripName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC2734C3FEB0");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__F3DBC572CB4F0721").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("imagepath");
            entity.Property(e => e.PhoneNum)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_num");
            entity.Property(e => e.UPassword)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("U_password");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__RoleId__6C190EBB");
        });

        modelBuilder.Entity<VisaCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Visa_Car__3214EC274036F5DD");

            entity.ToTable("Visa_Card");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("balance");
            entity.Property(e => e.CardNum)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("card_num");
            entity.Property(e => e.Cvv)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("cvv");
            entity.Property(e => e.ExpDate)
                .HasColumnType("date")
                .HasColumnName("exp_date");
            entity.Property(e => e.HolderName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Holder_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
