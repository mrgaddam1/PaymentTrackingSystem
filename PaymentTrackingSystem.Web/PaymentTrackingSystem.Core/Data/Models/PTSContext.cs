using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PTSContext : DbContext
{
    public PTSContext()
    {
    }

    public PTSContext(DbContextOptions<PTSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientAddress> ClientAddresses { get; set; }

    public virtual DbSet<ClientInterestPayment> ClientInterestPayments { get; set; }

    public virtual DbSet<ClientPayment> ClientPayments { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Lender> Lenders { get; set; }

    public virtual DbSet<LenderAddress> LenderAddresses { get; set; }

    public virtual DbSet<LendingDocument> LendingDocuments { get; set; }

    public virtual DbSet<LendingInterest> LendingInterests { get; set; }

    public virtual DbSet<PaymentDueDate> PaymentDueDates { get; set; }

    public virtual DbSet<PaymentMode> PaymentModes { get; set; }

    public virtual DbSet<PaymentMonth> PaymentMonths { get; set; }

    public virtual DbSet<Profession> Professions { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<PropertyAddress> PropertyAddresses { get; set; }

    public virtual DbSet<PropertyType> PropertyTypes { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<TenantAddress> TenantAddresses { get; set; }

    public virtual DbSet<TenantAgreement> TenantAgreements { get; set; }

    public virtual DbSet<TenantPreviousAddress> TenantPreviousAddresses { get; set; }

    public virtual DbSet<TenantProfession> TenantProfessions { get; set; }

    public virtual DbSet<TenantRent> TenantRents { get; set; }

    public virtual DbSet<TenantType> TenantTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PaymentTrackingSystemDB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Client");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.EmailId).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(150);
            entity.Property(e => e.LastName).HasMaxLength(150);
            entity.Property(e => e.MobileNumber).HasMaxLength(15);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ClientAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId);

            entity.ToTable("ClientAddress");

            entity.Property(e => e.AddressLine1).HasMaxLength(150);
            entity.Property(e => e.AddressLine2).HasMaxLength(150);
            entity.Property(e => e.City).HasMaxLength(150);
            entity.Property(e => e.Postcode).HasMaxLength(15);
        });

        modelBuilder.Entity<ClientInterestPayment>(entity =>
        {
            entity.HasKey(e => e.InterestId);

            entity.ToTable("ClientInterestPayment");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.InterestAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InterestFirstCutOffDate).HasColumnType("datetime");
            entity.Property(e => e.InterestPaidDate).HasColumnType("datetime");
            entity.Property(e => e.InterestPaidMonth).HasMaxLength(100);
            entity.Property(e => e.InterestSecondCutOffDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ClientPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId);

            entity.ToTable("ClientPayment");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountTransferedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.InterestAmountCutOffDate).HasColumnType("datetime");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.CountryName).HasMaxLength(150);
        });

        modelBuilder.Entity<Lender>(entity =>
        {
            entity.HasKey(e => e.LenderId).HasName("PK_Lenders");

            entity.ToTable("Lender");

            entity.Property(e => e.EmailId)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Lenders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lenders_User");
        });

        modelBuilder.Entity<LenderAddress>(entity =>
        {
            entity.ToTable("LenderAddress");

            entity.Property(e => e.AddressLine1).HasMaxLength(150);
            entity.Property(e => e.AddressLine2).HasMaxLength(150);
            entity.Property(e => e.Postcode).HasMaxLength(10);

            entity.HasOne(d => d.Country).WithMany(p => p.LenderAddresses)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LenderAddress_Country");
        });

        modelBuilder.Entity<LendingDocument>(entity =>
        {
            entity.ToTable("LendingDocument");

            entity.Property(e => e.DocumentExtension)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DocumentName)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Lender).WithMany(p => p.LendingDocuments)
                .HasForeignKey(d => d.LenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LendingDocument_Lender");

            entity.HasOne(d => d.User).WithMany(p => p.LendingDocuments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LendingDocument_User");
        });

        modelBuilder.Entity<LendingInterest>(entity =>
        {
            entity.ToTable("LendingInterest");

            entity.Property(e => e.ActualInterestAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExpectedInterestAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Lender).WithMany(p => p.LendingInterests)
                .HasForeignKey(d => d.LenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LendingInterest_Lender");

            entity.HasOne(d => d.PaymentMode).WithMany(p => p.LendingInterests)
                .HasForeignKey(d => d.PaymentModeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LendingInterest_PaymentMode");

            entity.HasOne(d => d.User).WithMany(p => p.LendingInterests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LendingInterest_User");
        });

        modelBuilder.Entity<PaymentDueDate>(entity =>
        {
            entity.HasKey(e => e.DueId);

            entity.ToTable("PaymentDueDate");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.MonthEndDate).HasColumnType("datetime");
            entity.Property(e => e.MonthName).HasMaxLength(150);
            entity.Property(e => e.MonthStartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<PaymentMode>(entity =>
        {
            entity.ToTable("PaymentMode");

            entity.Property(e => e.PaymentModeDescription)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PaymentMonth>(entity =>
        {
            entity.HasKey(e => e.MonthId);

            entity.ToTable("PaymentMonth");

            entity.Property(e => e.MonthId).ValueGeneratedNever();
            entity.Property(e => e.MonthName).HasMaxLength(150);
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.HasKey(e => e.ProfessionId).HasName("PK_TesProfession");

            entity.ToTable("Profession");

            entity.Property(e => e.ProfessionDescription).HasMaxLength(250);
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.ToTable("Property");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeleteDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.OwnerMobileNumber).HasMaxLength(25);
            entity.Property(e => e.PropertOwnerName).HasMaxLength(250);
            entity.Property(e => e.PropertyName).HasMaxLength(250);
        });

        modelBuilder.Entity<PropertyAddress>(entity =>
        {
            entity.ToTable("PropertyAddress");

            entity.Property(e => e.AddressLine1).HasMaxLength(250);
            entity.Property(e => e.AddressLine2).HasMaxLength(250);
            entity.Property(e => e.Postcode).HasMaxLength(15);
        });

        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.ToTable("PropertyType");

            entity.Property(e => e.PropertyTypeName).HasMaxLength(250);
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.ToTable("Tenant");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.EmailId).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(250);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(250);
            entity.Property(e => e.MobileNumber).HasMaxLength(25);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TenantAddress>(entity =>
        {
            entity.HasKey(e => e.TenantCurrentAddressId);

            entity.ToTable("TenantAddress");

            entity.Property(e => e.AddressLine1).HasMaxLength(150);
            entity.Property(e => e.AddressLine2).HasMaxLength(150);
            entity.Property(e => e.AddressLine3).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Postcode).HasMaxLength(10);
        });

        modelBuilder.Entity<TenantAgreement>(entity =>
        {
            entity.ToTable("TenantAgreement");

            entity.Property(e => e.AgreementFileType).HasMaxLength(100);
        });

        modelBuilder.Entity<TenantPreviousAddress>(entity =>
        {
            entity.ToTable("TenantPreviousAddress");

            entity.Property(e => e.AddressLine1).HasMaxLength(150);
            entity.Property(e => e.AddressLine2).HasMaxLength(150);
            entity.Property(e => e.AddressLine3).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Postcode).HasMaxLength(10);
        });

        modelBuilder.Entity<TenantProfession>(entity =>
        {
            entity.ToTable("TenantProfession");

            entity.Property(e => e.CompanyName).HasMaxLength(250);
        });

        modelBuilder.Entity<TenantRent>(entity =>
        {
            entity.HasKey(e => e.RentId);

            entity.ToTable("TenantRent");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedDatet).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.TenantEndDate).HasColumnType("datetime");
            entity.Property(e => e.TenantStartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TenantType>(entity =>
        {
            entity.ToTable("TenantType");

            entity.Property(e => e.TenantTypeDescription).HasMaxLength(250);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmailId).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(150);
            entity.Property(e => e.LastName).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
