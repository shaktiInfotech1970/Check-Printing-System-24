namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChequeLayout_Change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChequeLayout", "custom1Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom1X", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom1Y", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom1Text", c => c.String(maxLength: 4000));
            AddColumn("dbo.ChequeLayout", "custom2Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom2X", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom2Y", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom2Text", c => c.String(maxLength: 4000));
            AddColumn("dbo.ChequeLayout", "custom3Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom3X", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom3Y", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom3Text", c => c.String(maxLength: 4000));
            AddColumn("dbo.ChequeLayout", "custom4Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom4X", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom4Y", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom4Text", c => c.String(maxLength: 4000));
            AddColumn("dbo.ChequeLayout", "custom5Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom5X", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom5Y", c => c.Single(nullable: false));
            AddColumn("dbo.ChequeLayout", "custom5Text", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChequeLayout", "custom5Text");
            DropColumn("dbo.ChequeLayout", "custom5Y");
            DropColumn("dbo.ChequeLayout", "custom5X");
            DropColumn("dbo.ChequeLayout", "custom5Visible");
            DropColumn("dbo.ChequeLayout", "custom4Text");
            DropColumn("dbo.ChequeLayout", "custom4Y");
            DropColumn("dbo.ChequeLayout", "custom4X");
            DropColumn("dbo.ChequeLayout", "custom4Visible");
            DropColumn("dbo.ChequeLayout", "custom3Text");
            DropColumn("dbo.ChequeLayout", "custom3Y");
            DropColumn("dbo.ChequeLayout", "custom3X");
            DropColumn("dbo.ChequeLayout", "custom3Visible");
            DropColumn("dbo.ChequeLayout", "custom2Text");
            DropColumn("dbo.ChequeLayout", "custom2Y");
            DropColumn("dbo.ChequeLayout", "custom2X");
            DropColumn("dbo.ChequeLayout", "custom2Visible");
            DropColumn("dbo.ChequeLayout", "custom1Text");
            DropColumn("dbo.ChequeLayout", "custom1Y");
            DropColumn("dbo.ChequeLayout", "custom1X");
            DropColumn("dbo.ChequeLayout", "custom1Visible");
        }
    }
}
