namespace CPS
{
    using CPS.Business;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class CPSDbContext : DbContext
    {
        // Your context has been configured to use a 'DbEntity' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'CPS.DbEntity' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DbEntity' 
        // connection string in the application configuration file.
        public CPSDbContext(): base("name=CPSdbConnection")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<UserMasterDTO> Users { get; set; }
        public virtual DbSet<BankMasterDTO> BankMaster { get; set; }
        public virtual DbSet<BranchMasterDTO> BranchMaster { get; set; }
        public virtual DbSet<AccountTypeDTO> AccountType { get; set; }
        public virtual DbSet<ChequeBookSeriesDTO> ChequeBookSeries { get; set; }
        public virtual DbSet<PrinterPreference> PrinterPreference { get; set; }
        public virtual DbSet<RequestDTO> Request { get; set; }
        public virtual DbSet<Counter> Counter { get; set; }
        public virtual DbSet<PrintHistoryDTO> PrintHistory { get; set; }
        public virtual DbSet<PermissionDTO> Permission { get; set; }
        public virtual DbSet<DatabaseBackup> DatabaseBackup { get; set; }
        public virtual DbSet<ChequeLayout> ChequeLayout { get; set; }
        public virtual DbSet<RequestLayout> RequestLayout { get; set; }

        public virtual DbSet<ChequeSeries> ChequeSeries { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}