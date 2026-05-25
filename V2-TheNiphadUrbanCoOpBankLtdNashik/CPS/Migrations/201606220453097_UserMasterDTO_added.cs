namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserMasterDTO_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserMaster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        UserId = c.String(nullable: false, maxLength: 20),
                        Password = c.String(nullable: false, maxLength: 50),
                        IsLocked = c.Boolean(nullable: false),
                        LockDate = c.DateTime(),
                        LockReason = c.String(maxLength: 4000),
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
            DropTable("dbo.UserMaster");
        }
    }
}
