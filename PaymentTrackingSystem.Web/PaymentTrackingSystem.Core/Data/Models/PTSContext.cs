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

    public virtual DbSet<BillType> BillTypes { get; set; }

    public virtual DbSet<City> Citys { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientAddress> ClientAddresses { get; set; }

    public virtual DbSet<ClientInterestPayment> ClientInterestPayments { get; set; }

    public virtual DbSet<ClientPayment> ClientPayments { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Furniture> Furnitures { get; set; }

    public virtual DbSet<ImageCategory> ImageCategories { get; set; }

    public virtual DbSet<Investor> Investors { get; set; }

    public virtual DbSet<Lender> Lenders { get; set; }

    public virtual DbSet<LenderAddress> LenderAddresses { get; set; }

    public virtual DbSet<LendingAmountDetail> LendingAmountDetails { get; set; }

    public virtual DbSet<LendingDocument> LendingDocuments { get; set; }

    public virtual DbSet<LendingDueDateDescription> LendingDueDateDescriptions { get; set; }

    public virtual DbSet<LendingInterestRate> LendingInterestRates { get; set; }

    public virtual DbSet<PaymentDueDate> PaymentDueDates { get; set; }

    public virtual DbSet<PaymentMode> PaymentModes { get; set; }

    public virtual DbSet<PaymentMonth> PaymentMonths { get; set; }

    public virtual DbSet<Profession> Professions { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<PropertyAddress> PropertyAddresses { get; set; }

    public virtual DbSet<PropertyAmenity> PropertyAmenities { get; set; }

    public virtual DbSet<PropertyAmenityCategory> PropertyAmenityCategories { get; set; }

    public virtual DbSet<PropertyBillType> PropertyBillTypes { get; set; }

    public virtual DbSet<PropertyDetail> PropertyDetails { get; set; }

    public virtual DbSet<PropertyFinance> PropertyFinances { get; set; }

    public virtual DbSet<PropertyFurniture> PropertyFurnitures { get; set; }

    public virtual DbSet<PropertyImage> PropertyImages { get; set; }

    public virtual DbSet<PropertyStatus> PropertyStatuses { get; set; }

    public virtual DbSet<PropertyType> PropertyTypes { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<TenantAddress> TenantAddresses { get; set; }

    public virtual DbSet<TenantAgreement> TenantAgreements { get; set; }

    public virtual DbSet<TenantMonthlyPaymentDetail> TenantMonthlyPaymentDetails { get; set; }

    public virtual DbSet<TenantPreviousAddress> TenantPreviousAddresses { get; set; }

    public virtual DbSet<TenantProfession> TenantProfessions { get; set; }

    public virtual DbSet<TenantPropertyAssigned> TenantPropertyAssigneds { get; set; }

    public virtual DbSet<TenantType> TenantTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PaymentTrackingSystemDB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BillType>(entity =>
        {
            entity.ToTable("Bill_Type");

            entity.Property(e => e.BillTypeId).HasColumnName("Bill_Type_Id");
            entity.Property(e => e.BillTypeDescription)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Bill_Type_Description");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.Property(e => e.CityName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

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

        modelBuilder.Entity<District>(entity =>
        {
            entity.ToTable("District");

            entity.Property(e => e.DistrictName).HasMaxLength(250);
        });

        modelBuilder.Entity<Furniture>(entity =>
        {
            entity.ToTable("Furniture");

            entity.Property(e => e.FurnnitureName).HasMaxLength(250);
        });

        modelBuilder.Entity<ImageCategory>(entity =>
        {
            entity.ToTable("Image_Category");

            entity.Property(e => e.ImageCategoryId).HasColumnName("Image_Category_Id");
            entity.Property(e => e.ImageCategoryDescription)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Image_Category_Description");
        });

        modelBuilder.Entity<Investor>(entity =>
        {
            entity.Property(e => e.CompensatedAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Compensated_Amount");
            entity.Property(e => e.CompensatedBy)
                .HasMaxLength(150)
                .HasColumnName("Compensated_By");
            entity.Property(e => e.CompensationDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("Created_Date");
            entity.Property(e => e.DeletedDate)
                .HasColumnType("datetime")
                .HasColumnName("Deleted_Date");
            entity.Property(e => e.EmailId)
                .HasMaxLength(250)
                .HasColumnName("Email_Id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(150)
                .HasColumnName("First_Name");
            entity.Property(e => e.HaveYouCompensated).HasColumnName("HaveYou_Compensated");
            entity.Property(e => e.InvestAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Invest_Amount");
            entity.Property(e => e.InvestedDate)
                .HasColumnType("datetime")
                .HasColumnName("Invested_Date");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.LastName)
                .HasMaxLength(150)
                .HasColumnName("Last_Name");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(15)
                .HasColumnName("Mobile_Number");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("Modified_Date");
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

        modelBuilder.Entity<LendingAmountDetail>(entity =>
        {
            entity.HasKey(e => e.LendingInterestId).HasName("PK_LendingInterest");

            entity.ToTable("LendingAmountDetail");

            entity.Property(e => e.ActualInterestAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExpectedInterestAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LendingAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.LendingInterestRate).WithMany(p => p.LendingAmountDetails)
                .HasForeignKey(d => d.LendingInterestRateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LendingInterest_LendingInterestRates");

            entity.HasOne(d => d.PaymentMode).WithMany(p => p.LendingAmountDetails)
                .HasForeignKey(d => d.PaymentModeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LendingInterest_PaymentMode");

            entity.HasOne(d => d.User).WithMany(p => p.LendingAmountDetails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LendingInterest_User");
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

            entity.HasOne(d => d.User).WithMany(p => p.LendingDocuments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LendingDocument_User");
        });

        modelBuilder.Entity<LendingDueDateDescription>(entity =>
        {
            entity.HasKey(e => e.LendingDueDateId);

            entity.ToTable("LendingDueDateDescription");

            entity.Property(e => e.LendingDueDateDescritpion).HasMaxLength(250);
        });

        modelBuilder.Entity<LendingInterestRate>(entity =>
        {
            entity.Property(e => e.InterestRate).HasColumnType("decimal(18, 2)");
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

            entity.Property(e => e.PropertyId).HasColumnName("Property_Id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("Created_Date");
            entity.Property(e => e.DeleteDate)
                .HasColumnType("datetime")
                .HasColumnName("Delete_Date");
            entity.Property(e => e.HasElectricityBill).HasColumnName("Has_Electricity_Bill");
            entity.Property(e => e.HasFurniture).HasColumnName("Has_Furniture");
            entity.Property(e => e.HasParking).HasColumnName("Has_Parking");
            entity.Property(e => e.HasWaterBill).HasColumnName("Has_Water_Bill");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("Modified_Date");
            entity.Property(e => e.OwnerMobileNumber)
                .HasMaxLength(25)
                .HasColumnName("Owner_Mobile_Number");
            entity.Property(e => e.PropertOwnerName)
                .HasMaxLength(250)
                .HasColumnName("Propert_Owner_Name");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .HasColumnName("Property_Name");
            entity.Property(e => e.PropertyReference)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Property_Reference");
            entity.Property(e => e.PropertyStatusId).HasColumnName("Property_Status_Id");
            entity.Property(e => e.PropertyTypeId).HasColumnName("Property_Type_Id");
            entity.Property(e => e.UserId).HasColumnName("User_Id");
        });

        modelBuilder.Entity<PropertyAddress>(entity =>
        {
            entity.ToTable("Property_Address");

            entity.Property(e => e.PropertyAddressId).HasColumnName("Property_Address_Id");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(250)
                .HasColumnName("Address_Line1");
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(250)
                .HasColumnName("Address_Line2");
            entity.Property(e => e.CountryId).HasColumnName("Country_Id");
            entity.Property(e => e.DistrictId).HasColumnName("District_Id");
            entity.Property(e => e.Postcode).HasMaxLength(15);
            entity.Property(e => e.PropertyId).HasColumnName("Property_Id");
            entity.Property(e => e.StateId).HasColumnName("State_Id");

            entity.HasOne(d => d.Country).WithMany(p => p.PropertyAddresses)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_Property_Address_Country");

            entity.HasOne(d => d.District).WithMany(p => p.PropertyAddresses)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK_Property_Address_District");

            entity.HasOne(d => d.State).WithMany(p => p.PropertyAddresses)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_Property_Address_State");
        });

        modelBuilder.Entity<PropertyAmenity>(entity =>
        {
            entity.HasKey(e => e.PropertyAmenityId).HasName("PK_Amenity");

            entity.ToTable("Property_Amenity");

            entity.HasIndex(e => e.AmenityName, "UQ__Property__8A4FC32E705C616C").IsUnique();

            entity.Property(e => e.PropertyAmenityId).HasColumnName("Property_Amenity_Id");
            entity.Property(e => e.AmenityDescription)
                .HasMaxLength(500)
                .HasColumnName("Amenity_Description");
            entity.Property(e => e.AmenityName)
                .HasMaxLength(100)
                .HasColumnName("Amenity_Name");
            entity.Property(e => e.CreatedDate)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PropertyAmenityCategoryId).HasColumnName("Property_Amenity_Category_Id");
        });

        modelBuilder.Entity<PropertyAmenityCategory>(entity =>
        {
            entity.ToTable("Property_Amenity_Category");

            entity.Property(e => e.PropertyAmenityCategoryId).HasColumnName("Property_Amenity_Category_Id");
            entity.Property(e => e.CategoryDescription)
                .HasMaxLength(250)
                .HasColumnName("Category_Description");
        });

        modelBuilder.Entity<PropertyBillType>(entity =>
        {
            entity.ToTable("Property_Bill_Type");

            entity.Property(e => e.PropertyBillTypeId).HasColumnName("Property_Bill_Type_Id");
            entity.Property(e => e.BillTypeId).HasColumnName("Bill_Type_Id");
        });

        modelBuilder.Entity<PropertyDetail>(entity =>
        {
            entity.ToTable("Property_Detail");

            entity.Property(e => e.PropertyDetailId).HasColumnName("Property_Detail_Id");
            entity.Property(e => e.AreaInSquareFeet)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Area_In_Square_Feet");
            entity.Property(e => e.FloorNumber).HasColumnName("Floor_Number");
            entity.Property(e => e.IsItFurnished).HasColumnName("Is_It_Furnished");
            entity.Property(e => e.NoOfBathRooms).HasColumnName("No_Of_Bath_Rooms");
            entity.Property(e => e.NoOfBedRooms).HasColumnName("No_Of_Bed_Rooms");
            entity.Property(e => e.PropertyId).HasColumnName("Property_Id");
            entity.Property(e => e.PropertyTypeId).HasColumnName("Property_Type_Id");
            entity.Property(e => e.TotalFloors).HasColumnName("Total_Floors");
            entity.Property(e => e.YearBuilt).HasColumnName("Year_Built");

            entity.HasOne(d => d.Property).WithMany(p => p.PropertyDetails)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Property_Detail_Property");

            entity.HasOne(d => d.PropertyType).WithMany(p => p.PropertyDetails)
                .HasForeignKey(d => d.PropertyTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Property_Detail_PropertyType");
        });

        modelBuilder.Entity<PropertyFinance>(entity =>
        {
            entity.HasKey(e => e.PropertyFinancialId);

            entity.ToTable("Property_Finance");

            entity.Property(e => e.PropertyFinancialId).HasColumnName("PropertyFinancial_Id");
            entity.Property(e => e.DepositAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Deposit_Amount");
            entity.Property(e => e.HasMaintananceBill).HasColumnName("Has_Maintanance_Bill");
            entity.Property(e => e.IsThereAnyDeposit).HasColumnName("Is_There_Any_Deposit");
            entity.Property(e => e.MaintananceBillDescription)
                .HasMaxLength(1000)
                .HasColumnName("Maintanance_Bill_Description");
            entity.Property(e => e.NumberOfMonthsForDeposit).HasColumnName("Number_Of_Months_For_Deposit");
            entity.Property(e => e.PropertyAmenityId).HasColumnName("Property_Amenity_Id");
            entity.Property(e => e.PropertyId).HasColumnName("Property_Id");
            entity.Property(e => e.RentAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Rent_Amount");

            entity.HasOne(d => d.PropertyAmenity).WithMany(p => p.PropertyFinances)
                .HasForeignKey(d => d.PropertyAmenityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Property_Finance_Property_Amenity");

            entity.HasOne(d => d.Property).WithMany(p => p.PropertyFinances)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Property_Finance_Property");
        });

        modelBuilder.Entity<PropertyImage>(entity =>
        {
            entity.HasKey(e => e.PropertyImageId).HasName("PK_PropertyImage");

            entity.ToTable("Property_Image");

            entity.HasIndex(e => e.BlobStorageUrl, "UC_PropertyImage_BlobUrl").IsUnique();

            entity.Property(e => e.BlobContainerName).HasMaxLength(100);
            entity.Property(e => e.BlobStoragePath).HasMaxLength(300);
            entity.Property(e => e.BlobStorageUrl).HasMaxLength(500);
            entity.Property(e => e.DeleteDate).HasPrecision(0);
            entity.Property(e => e.ImageCategoryId).HasColumnName("Image_Category_Id");
            entity.Property(e => e.ImageData).HasColumnName("Image_Data");
            entity.Property(e => e.ImageDescription)
                .HasMaxLength(500)
                .HasColumnName("Image_Description");
            entity.Property(e => e.ImageName)
                .HasMaxLength(250)
                .HasColumnName("Image_Name");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsPublic).HasDefaultValue(true);
            entity.Property(e => e.MimeType).HasMaxLength(50);
            entity.Property(e => e.PropertyId).HasColumnName("Property_Id");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UploadedDate)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Property).WithMany(p => p.PropertyImages)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PropertyImage_Property");
        });

        modelBuilder.Entity<PropertyStatus>(entity =>
        {
            entity.ToTable("Property_Status");

            entity.Property(e => e.PropertyStatusId).HasColumnName("Property_Status_Id");
            entity.Property(e => e.PropertyStatusDescription)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Property_Status_Description");
        });

        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.ToTable("PropertyType");

            entity.Property(e => e.PropertyTypeName).HasMaxLength(250);
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.ToTable("State");

            entity.Property(e => e.Statement).HasMaxLength(250);
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

            entity.Property(e => e.AgreementFileName).HasMaxLength(250);
            entity.Property(e => e.AgreementFileType).HasMaxLength(100);
        });

        modelBuilder.Entity<TenantMonthlyPaymentDetail>(entity =>
        {
            entity.HasKey(e => e.TenantPropertyId).HasName("PK_TenantPropertyRentDetail");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Rent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate)
                .HasMaxLength(10)
                .IsFixedLength();
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

        modelBuilder.Entity<TenantPropertyAssigned>(entity =>
        {
            entity.HasKey(e => e.RentId).HasName("PK_TenantRent");

            entity.ToTable("TenantPropertyAssigned");

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
