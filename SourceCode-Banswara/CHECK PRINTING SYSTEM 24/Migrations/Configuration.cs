namespace CPS.Migrations
{
    using CPS.Business;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CPS.CPSDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CPS.CPSDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            SeedUser(context);
            SeedPermission(context);
            SeedRequestLayout(context);
            SeedChequeLayout(context);

            context.BankMaster.AddOrUpdate(new BankMasterDTO { Id = 1, Code = "815", Name = "BANSWARA CENTRAL CO - OP. BANK LTD.", AddressLine1 = "AddressLine1", City = "City", State = "State", Country = "Country", PostalCode = "000000", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            context.AccountType.AddOrUpdate(new AccountTypeDTO { Id = 1, Code = 10, Name = "Savings A/C No.", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.AccountType.AddOrUpdate(new AccountTypeDTO { Id = 2, Code = 11, Name = "Current A/C No.", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.AccountType.AddOrUpdate(new AccountTypeDTO { Id = 3, Code = 12, Name = "Pay Order A/C No.", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.AccountType.AddOrUpdate(new AccountTypeDTO { Id = 4, Code = 13, Name = "CC A/C No.", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            SeedBranchMaster(context);
            SeedChequeBookSeries(context);

        }

        private void SeedBranchMaster(CPSDbContext context)
        {
            context.BranchMaster.AddOrUpdate(new BranchMasterDTO { Id = 1, Code = "002", Name = "COLLEGE ROAD", ShortName = "BR1", IFSC = "RSCB0013009", MICR = "327815002", AddressLine1 = "NEAR RAJTALAB POLICE CHOWKI", AddressLine2 = "COLLEGE ROAD", City = "BANSWARA", PostalCode = "327001", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.BranchMaster.AddOrUpdate(new BranchMasterDTO { Id = 2, Code = "004", Name = "CITY BRANCH", ShortName = "BR2", IFSC = "RSCB0013001", MICR = "327815004", AddressLine1 = "SADAR BAZAR", AddressLine2 = "AAZAD CHOWK", City = "BANSWARA", PostalCode = "327001", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.BranchMaster.AddOrUpdate(new BranchMasterDTO { Id = 3, Code = "003", Name = "NAI AABADI", ShortName = "BR3", IFSC = "RSCB0013008", MICR = "327815003", AddressLine1 = "NEAR DAKSHIN KALIKA MANDIR", AddressLine2 = "NAI AABADI", City = "BANSWARA", PostalCode = "327001", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.BranchMaster.AddOrUpdate(new BranchMasterDTO { Id = 4, Code = "026", Name = "PARTAPUR", ShortName = "BR4", IFSC = "RSCB0013002", MICR = "327815026", AddressLine1 = "NEAR GOVT. SCHOOL STADIUM, SADAR BAZAR", AddressLine2 = "PARTAPUR", City = "BANSWARA", PostalCode = "327024", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.BranchMaster.AddOrUpdate(new BranchMasterDTO { Id = 5, Code = "027", Name = "KUSHALGARH", ShortName = "BR5", IFSC = "RSCB0013003", MICR = "327815027", AddressLine1 = "NEAR SHAHID BHAGAT SINGH BUS STAND", AddressLine2 = "KUSHALGARH", City = "BANSWARA", PostalCode = "327801", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.BranchMaster.AddOrUpdate(new BranchMasterDTO { Id = 6, Code = "028", Name = "BAGIDORA", ShortName = "BR6", IFSC = "RSCB0013004", MICR = "327815028", AddressLine1 = "NEAR BUS STAND , MASJID STREET", AddressLine2 = "BAGIDORA", City = "BANSWARA", PostalCode = "327601", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.BranchMaster.AddOrUpdate(new BranchMasterDTO { Id = 7, Code = "029", Name = "GHATOL", ShortName = "BR7", IFSC = "RSCB0013005", MICR = "327815029", AddressLine1 = "NEAR POST OFFICE CHOURAHA", AddressLine2 = "GHATOL", City = "BANSWARA", PostalCode = "327023", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.BranchMaster.AddOrUpdate(new BranchMasterDTO { Id = 8, Code = "030", Name = "SAJJANGARH", ShortName = "BR8", IFSC = "RSCB0013006", MICR = "327815030", AddressLine1 = "NEAR GOVT. AYURVEDIC HOSPITAL, KUSHALGARH ROAD", AddressLine2 = "SAJJANGARH", City = "BANSWARA", PostalCode = "327602", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.BranchMaster.AddOrUpdate(new BranchMasterDTO { Id = 9, Code = "031", Name = "ANANDPURI", ShortName = "BR9", IFSC = "RSCB0013007", MICR = "327815031", AddressLine1 = "OPP. PANCHAYAT SAMITI", AddressLine2 = "ANANDPURI", City = "BANSWARA", PostalCode = "327031", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.BranchMaster.AddOrUpdate(new BranchMasterDTO { Id = 10, Code = "051", Name = "GANGARTALAI", ShortName = "BR10", IFSC = "RSCB0013010", MICR = "327815051", AddressLine1 = "NEAR AMBIKA CHOWK, JAMBURI ROAD, MAIN BAZAR", AddressLine2 = "GANGARTALAI", City = "BANSWARA", PostalCode = "327601", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.BranchMaster.AddOrUpdate(new BranchMasterDTO { Id = 11, Code = "052", Name = "TALWARA", ShortName = "BR11", IFSC = "RSCB0013011", MICR = "327815052", AddressLine1 = "OPP. PANCHAYAT SAMITI ,DUNGARPUR ROAD", AddressLine2 = "TALWARA", City = "BANSWARA", PostalCode = "327025", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
        }

        private void SeedChequeBookSeries(CPSDbContext context)
        {
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 1, BranchId = 1, AccountTypeId = 1, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 2, BranchId = 1, AccountTypeId = 2, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 3, BranchId = 1, AccountTypeId = 3, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 4, BranchId = 1, AccountTypeId = 4, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 5, BranchId = 2, AccountTypeId = 1, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 6, BranchId = 2, AccountTypeId = 2, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 7, BranchId = 2, AccountTypeId = 3, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 8, BranchId = 2, AccountTypeId = 4, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 9, BranchId = 3, AccountTypeId = 1, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 10, BranchId = 3, AccountTypeId = 2, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 11, BranchId = 3, AccountTypeId = 3, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 12, BranchId = 3, AccountTypeId = 4, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 13, BranchId = 4, AccountTypeId = 1, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 14, BranchId = 4, AccountTypeId = 2, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 15, BranchId = 4, AccountTypeId = 3, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 16, BranchId = 4, AccountTypeId = 4, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 17, BranchId = 5, AccountTypeId = 1, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 18, BranchId = 5, AccountTypeId = 2, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 19, BranchId = 5, AccountTypeId = 3, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 20, BranchId = 5, AccountTypeId = 4, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 21, BranchId = 6, AccountTypeId = 1, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 22, BranchId = 6, AccountTypeId = 2, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 23, BranchId = 6, AccountTypeId = 3, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 24, BranchId = 6, AccountTypeId = 4, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 25, BranchId = 7, AccountTypeId = 1, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 26, BranchId = 7, AccountTypeId = 2, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 27, BranchId = 7, AccountTypeId = 3, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 28, BranchId = 7, AccountTypeId = 4, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 29, BranchId = 8, AccountTypeId = 1, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 30, BranchId = 8, AccountTypeId = 2, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 31, BranchId = 8, AccountTypeId = 3, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 32, BranchId = 8, AccountTypeId = 4, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 33, BranchId = 9, AccountTypeId = 1, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 34, BranchId = 9, AccountTypeId = 2, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 35, BranchId = 9, AccountTypeId = 3, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 36, BranchId = 9, AccountTypeId = 4, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 37, BranchId = 10, AccountTypeId = 1, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 38, BranchId = 10, AccountTypeId = 2, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 39, BranchId = 10, AccountTypeId = 3, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 40, BranchId = 10, AccountTypeId = 4, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 41, BranchId = 11, AccountTypeId = 1, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 42, BranchId = 11, AccountTypeId = 2, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 43, BranchId = 11, AccountTypeId = 3, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.ChequeBookSeries.AddOrUpdate(new ChequeBookSeriesDTO { Id = 44, BranchId = 11, AccountTypeId = 4, StartChequeNumber = 1, EndChequeNumber = 999999, LastChequeNumber = 0, AvailableCheques = 999999, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
        }

        private void SeedUser(CPSDbContext context)
        {
            var superadmin = new UserMasterDTO { Id = 1, Name = "superadmin", UserId = "superadmin", Password = "$up3r@Dmin", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now, IsLocked = false };
            var admin = new UserMasterDTO { Id = 2, Name = "admin", UserId = "admin", Password = "admin", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now, IsLocked = false };
            context.Users.AddOrUpdate(superadmin);
            context.Users.AddOrUpdate(admin);
        }

        private void SeedPermission(CPSDbContext context)
        {
            context.Permission.AddOrUpdate(new PermissionDTO { Id = 1, UserId = 1, Page = Common.Page.AccountType, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 2, UserId = 1, Page = Common.Page.BankMaster, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 3, UserId = 1, Page = Common.Page.BranchMaster, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 4, UserId = 1, Page = Common.Page.ChequeBookSeries, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 5, UserId = 1, Page = Common.Page.DataExport, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 6, UserId = 1, Page = Common.Page.DataImport, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 7, UserId = 1, Page = Common.Page.PrintChequeBook, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 8, UserId = 1, Page = Common.Page.RePrintChequeBook, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 9, UserId = 1, Page = Common.Page.RePrintRequest, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 10, UserId = 1, Page = Common.Page.RePrintSinglePage, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 11, UserId = 1, Page = Common.Page.RequestDataEntry, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 12, UserId = 1, Page = Common.Page.DaywiseChequePrint, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 13, UserId = 1, Page = Common.Page.PendingChequeRequest, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 14, UserId = 1, Page = Common.Page.PrintedCheque, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 15, UserId = 1, Page = Common.Page.PrintedChequePrintFile, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 16, UserId = 1, Page = Common.Page.PrintedChequeSeries, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 17, UserId = 1, Page = Common.Page.ReprintedCheque, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 18, UserId = 1, Page = Common.Page.ReprintedChequeSinglePage, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 19, UserId = 1, Page = Common.Page.TotalPrintCheque, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 20, UserId = 1, Page = Common.Page.TotalReprintCheque, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 21, UserId = 1, Page = Common.Page.Permission, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 22, UserId = 1, Page = Common.Page.UserMaster, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 23, UserId = 1, Page = Common.Page.Preferences, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 24, UserId = 1, Page = Common.Page.DatabaseBackup, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 25, UserId = 1, Page = Common.Page.RequestLayoutPreference, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 26, UserId = 1, Page = Common.Page.ChequeLayoutPreference, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },

                                           new PermissionDTO { Id = 27, UserId = 2, Page = Common.Page.AccountType, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 28, UserId = 2, Page = Common.Page.BankMaster, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 29, UserId = 2, Page = Common.Page.BranchMaster, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 30, UserId = 2, Page = Common.Page.ChequeBookSeries, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 31, UserId = 2, Page = Common.Page.DataExport, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 32, UserId = 2, Page = Common.Page.DataImport, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 33, UserId = 2, Page = Common.Page.PrintChequeBook, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 34, UserId = 2, Page = Common.Page.RePrintChequeBook, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 35, UserId = 2, Page = Common.Page.RePrintRequest, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 36, UserId = 2, Page = Common.Page.RePrintSinglePage, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 37, UserId = 2, Page = Common.Page.RequestDataEntry, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 38, UserId = 2, Page = Common.Page.DaywiseChequePrint, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 39, UserId = 2, Page = Common.Page.PendingChequeRequest, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 40, UserId = 2, Page = Common.Page.PrintedCheque, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 41, UserId = 2, Page = Common.Page.PrintedChequePrintFile, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 42, UserId = 2, Page = Common.Page.PrintedChequeSeries, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 43, UserId = 2, Page = Common.Page.ReprintedCheque, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 44, UserId = 2, Page = Common.Page.ReprintedChequeSinglePage, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 45, UserId = 2, Page = Common.Page.TotalPrintCheque, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 46, UserId = 2, Page = Common.Page.TotalReprintCheque, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 47, UserId = 2, Page = Common.Page.Permission, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 48, UserId = 2, Page = Common.Page.UserMaster, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 49, UserId = 2, Page = Common.Page.Preferences, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 50, UserId = 2, Page = Common.Page.DatabaseBackup, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 51, UserId = 2, Page = Common.Page.RequestLayoutPreference, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 52, UserId = 2, Page = Common.Page.ChequeLayoutPreference, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
        }

        private void SeedRequestLayout(CPSDbContext context)
        {
            context.RequestLayout.AddOrUpdate(x => x.Id, new RequestLayout
            {
                Id = 1,

                branchAddress1Visble = false,
                branchAddress1X = 0.0f,
                branchAddress1Y = 0.0f,

                branchAddress2Visble = false,
                branchAddress2X = 0.0f,
                branchAddress2Y = 0.0f,

                chequeFrom1Visble = true,
                chequeFrom1X = 3.7f,
                chequeFrom1Y = 1.8f,

                chequeTo1Visble = true,
                chequeTo1X = 8.5f,
                chequeTo1Y = 1.8f,

                chequeFrom2Visble = true,
                chequeFrom2X = 18f,
                chequeFrom2Y = 1.8f,

                chequeTo2Visble = true,
                chequeTo2X = 23f,
                chequeTo2Y = 1.8f,

                nameAddress1Visble = true,
                nameAddress1X = 1f,
                nameAddress1Y = 3.2f,

                nameAddress2Visble = true,
                nameAddress2X = 15f,
                nameAddress2Y = 3.2f,

                accountNo1Visble = true,
                accountNo1X = 1f,
                accountNo1Y = 6.5f,

                accountNo2Visble = true,
                accountNo2X = 15f,
                accountNo2Y = 6.5f,

                barcode1Visble = true,
                barcode1X = 1.1f,
                barcode1Y = 7.3f,

                barcode2Visble = false,
                barcode2X = 1.1f,
                barcode2Y = 7.8f,

                audiText1Visble = true,
                audiText1X = 0.2f,
                audiText1Y = 3f,

                audiText2Visble = true,
                audiText2X = 14f,
                audiText2Y = 3f
            });
        }

        private void SeedChequeLayout(CPSDbContext context)
        {
            context.ChequeLayout.AddOrUpdate(x => x.Id, new ChequeLayout
            {
                Id = 1,

                branchAddressVisble = true,
                branchAddressX = 2.7f,
                branchAddressY = 0.5f,

                ifscVisble = true,
                ifscX = 2.7f,
                ifscY = 1f,

                orderOrBarerVisble = true,
                orderOrBarerX = 24f,
                orderOrBarerY = 2f,

                accountNoVisble = true,
                accountNoX = 1.4f,
                accountNoY = 5.5f,

                stampVisble = true,
                stampX = 10.6f,
                stampY = 6f,

                micrVisble = true,
                micrX = 6.45f,
                micrY = 10.8f,

                barcodeVisble = true,
                barcodeX = 26.8f,
                barcodeY = 2.2f,

                audiTextVisble = true,
                audiTextX = 0.2f,
                audiTextY = 2f,

                accountPayeeVisble = true,
                accountPayeeX = 0f,
                accountPayeeY = 0.2f,
            });
        }

    }
}
