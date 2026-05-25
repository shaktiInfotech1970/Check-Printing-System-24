namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uma2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Request", "AccountNo", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "Name", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Request", "Name", c => c.String(nullable: false, maxLength: 4000));
            AlterColumn("dbo.Request", "AccountNo", c => c.String(nullable: false, maxLength: 4000));
        }
    }
}
