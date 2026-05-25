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
            //SeedRequestLayout(context);
            //SeedChequeLayout(context);



            context.BankMaster.AddOrUpdate(new BankMasterDTO { Id = 1, Code = "815", Name = "BANKNAME", AddressLine1 = "BANKADDRESS", City = "CITY", State = "STATE", Country = "India", PostalCode = "000000", WebAddress = "", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });

            context.AccountType.AddOrUpdate(new AccountTypeDTO { Id = 1, Code = 10, Name = "Savings A/C No.", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.AccountType.AddOrUpdate(new AccountTypeDTO { Id = 2, Code = 11, Name = "Current A/C No.", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.AccountType.AddOrUpdate(new AccountTypeDTO { Id = 3, Code = 12, Name = "Pay Order A/C No.", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
            context.AccountType.AddOrUpdate(new AccountTypeDTO { Id = 4, Code = 13, Name = "CC A/C No.", CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
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
                                           new PermissionDTO { Id = 52, UserId = 2, Page = Common.Page.ChequeLayoutPreference, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 53, UserId = 1, Page = Common.Page.SearchReport, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 54, UserId = 2, Page = Common.Page.SearchReport, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now },
                                           new PermissionDTO { Id = 55, UserId = 1, Page = Common.Page.RemoveRequestData, Permission = 7, CreatedBy = "default", CreatedOn = DateTime.Now, UpdatedBy = "default", UpdatedOn = DateTime.Now });
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

                custom1Visible = false,
                custom1X = 0f,
                custom1Y = 0f,
                custom1Text = string.Empty,

                custom2Visible = false,
                custom2X = 0f,
                custom2Y = 0f,
                custom2Text = string.Empty,


                custom3Visible = false,
                custom3X = 0f,
                custom3Y = 0f,
                custom3Text = string.Empty,

                custom4Visible = false,
                custom4X = 0f,
                custom4Y = 0f,
                custom4Text = string.Empty,

                custom5Visible = false,
                custom5X = 0f,
                custom5Y = 0f,
                custom5Text = string.Empty
            });
        }

    }
}
