namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ACNOoptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Request", "AccountNo", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Request", "AccountNo", c => c.String(nullable: false, maxLength: 4000));
        }
    }
}
