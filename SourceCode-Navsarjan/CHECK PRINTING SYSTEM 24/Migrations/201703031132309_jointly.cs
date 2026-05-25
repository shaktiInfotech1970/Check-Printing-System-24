namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jointly : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "Jointly", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "Jointly");
        }
    }
}
