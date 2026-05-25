namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FAIZ : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "BankBranch", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f2", c => c.String(maxLength: 4000));
            DropColumn("dbo.Request", "brsid");
            DropColumn("dbo.Request", "Address4");
            DropColumn("dbo.Request", "Address5");
            DropColumn("dbo.Request", "telr");
            DropColumn("dbo.Request", "telo");
            DropColumn("dbo.Request", "mob");
            DropColumn("dbo.Request", "prcode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "prcode", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "mob", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "telo", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "telr", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Address5", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Address4", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "brsid", c => c.String(maxLength: 4000));
            DropColumn("dbo.Request", "additional_f2");
            DropColumn("dbo.Request", "additional_f1");
            DropColumn("dbo.Request", "BankBranch");
        }
    }
}
