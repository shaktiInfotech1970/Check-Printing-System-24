namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BarodaCity1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "AccountNo", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "AccountNo");
        }
    }
}
