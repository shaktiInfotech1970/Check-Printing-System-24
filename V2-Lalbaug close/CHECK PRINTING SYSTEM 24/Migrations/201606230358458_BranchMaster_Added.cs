namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BranchMaster_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BranchMaster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 4000),
                        Name = c.String(nullable: false, maxLength: 100),
                        ShortName = c.String(nullable: false, maxLength: 50),
                        IFSC = c.String(nullable: false, maxLength: 4000),
                        MICR = c.String(nullable: false, maxLength: 4000),
                        AddressLine1 = c.String(nullable: false, maxLength: 100),
                        AddressLine2 = c.String(maxLength: 100),
                        AddressLine3 = c.String(maxLength: 100),
                        City = c.String(nullable: false, maxLength: 100),
                        PostalCode = c.String(nullable: false, maxLength: 4000),
                        Telephone1 = c.String(maxLength: 20),
                        Telephone2 = c.String(maxLength: 20),
                        Mobile = c.String(maxLength: 20),
                        Email = c.String(maxLength: 4000),
                        Fax = c.String(maxLength: 20),
                        ImportPath = c.String(maxLength: 100),
                        ExportPath = c.String(maxLength: 100),
                        CreatedBy = c.String(nullable: false, maxLength: 4000),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.String(nullable: false, maxLength: 4000),
                        UpdatedOn = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BranchMaster");
        }
    }
}
