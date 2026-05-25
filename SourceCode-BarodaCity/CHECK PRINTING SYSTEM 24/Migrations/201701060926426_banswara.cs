namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class banswara : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Request", "brsid");
            DropColumn("dbo.Request", "AccountNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "AccountNo", c => c.String(nullable: false, maxLength: 4000));
            AddColumn("dbo.Request", "brsid", c => c.String(maxLength: 4000));
        }
    }
}
