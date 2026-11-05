namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Request", "brsid", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Address4", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "Address5", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "telr", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "telo", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "mob", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "prcode", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f1", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f2", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f3", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f4", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f5", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f6", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f7", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f8", c => c.String(maxLength: 4000));
            AddColumn("dbo.Request", "additional_f9", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "RequestNo", c => c.Long(nullable: false));
            AlterColumn("dbo.Request", "SerialNo", c => c.Long(nullable: false));
            AlterColumn("dbo.Request", "AccountNoFull", c => c.String(nullable: false, maxLength: 4000));
            DropColumn("dbo.Request", "ECSAccountCode");
            DropColumn("dbo.Request", "MICRCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Request", "MICRCode", c => c.String(nullable: false, maxLength: 4000));
            AddColumn("dbo.Request", "ECSAccountCode", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "AccountNoFull", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Request", "SerialNo", c => c.Int(nullable: false));
            AlterColumn("dbo.Request", "RequestNo", c => c.Int(nullable: false));
            DropColumn("dbo.Request", "additional_f9");
            DropColumn("dbo.Request", "additional_f8");
            DropColumn("dbo.Request", "additional_f7");
            DropColumn("dbo.Request", "additional_f6");
            DropColumn("dbo.Request", "additional_f5");
            DropColumn("dbo.Request", "additional_f4");
            DropColumn("dbo.Request", "additional_f3");
            DropColumn("dbo.Request", "additional_f2");
            DropColumn("dbo.Request", "additional_f1");
            DropColumn("dbo.Request", "prcode");
            DropColumn("dbo.Request", "mob");
            DropColumn("dbo.Request", "telo");
            DropColumn("dbo.Request", "telr");
            DropColumn("dbo.Request", "Address5");
            DropColumn("dbo.Request", "Address4");
            DropColumn("dbo.Request", "brsid");
        }
    }
}
