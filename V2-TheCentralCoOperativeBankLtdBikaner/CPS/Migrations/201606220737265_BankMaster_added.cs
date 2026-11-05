namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BankMaster_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankMaster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 4000),
                        Name = c.String(nullable: false, maxLength: 100),
                        AddressLine1 = c.String(nullable: false, maxLength: 100),
                        AddressLine2 = c.String(maxLength: 100),
                        AddressLine3 = c.String(maxLength: 100),
                        City = c.String(nullable: false, maxLength: 100),
                        State = c.String(nullable: false, maxLength: 100),
                        Country = c.String(nullable: false, maxLength: 100),
                        PostalCode = c.String(nullable: false, maxLength: 4000),
                        Phone = c.String(maxLength: 20),
                        Mobile = c.String(maxLength: 20),
                        Email = c.String(maxLength: 4000),
                        Fax = c.String(maxLength: 20),
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
            DropTable("dbo.BankMaster");
        }
    }
}
