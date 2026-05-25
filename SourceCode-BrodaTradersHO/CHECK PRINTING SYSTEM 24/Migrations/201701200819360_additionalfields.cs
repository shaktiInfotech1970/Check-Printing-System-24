namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additionalfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "additional_f1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f2", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f3", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f4", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f5", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f6", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f7", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "opmode", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f8", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f9", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "name_1", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "name_1");
            DropColumn("dbo.Request", "additional_f9");
            DropColumn("dbo.Request", "additional_f8");
            DropColumn("dbo.Request", "opmode");
            DropColumn("dbo.Request", "additional_f7");
            DropColumn("dbo.Request", "additional_f6");
            DropColumn("dbo.Request", "additional_f5");
            DropColumn("dbo.Request", "additional_f4");
            DropColumn("dbo.Request", "additional_f3");
            DropColumn("dbo.Request", "additional_f2");
            DropColumn("dbo.Request", "additional_f1");
        }
    }
}
