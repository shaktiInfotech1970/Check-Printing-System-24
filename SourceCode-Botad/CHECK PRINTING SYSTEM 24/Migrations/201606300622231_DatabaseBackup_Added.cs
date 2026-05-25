namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseBackup_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DatabaseBackup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(maxLength: 4000),
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
            DropTable("dbo.DatabaseBackup");
        }
    }
}
