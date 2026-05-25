namespace CPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Layout_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChequeLayout",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        branchAddressVisble = c.Boolean(nullable: false),
                        branchAddressX = c.Single(nullable: false),
                        branchAddressY = c.Single(nullable: false),
                        ifscVisble = c.Boolean(nullable: false),
                        ifscX = c.Single(nullable: false),
                        ifscY = c.Single(nullable: false),
                        orderOrBarerVisble = c.Boolean(nullable: false),
                        orderOrBarerX = c.Single(nullable: false),
                        orderOrBarerY = c.Single(nullable: false),
                        accountNoVisble = c.Boolean(nullable: false),
                        accountNoX = c.Single(nullable: false),
                        accountNoY = c.Single(nullable: false),
                        stampVisble = c.Boolean(nullable: false),
                        stampX = c.Single(nullable: false),
                        stampY = c.Single(nullable: false),
                        micrVisble = c.Boolean(nullable: false),
                        micrX = c.Single(nullable: false),
                        micrY = c.Single(nullable: false),
                        barcodeVisble = c.Boolean(nullable: false),
                        barcodeX = c.Single(nullable: false),
                        barcodeY = c.Single(nullable: false),
                        audiTextVisble = c.Boolean(nullable: false),
                        audiTextX = c.Single(nullable: false),
                        audiTextY = c.Single(nullable: false),
                        accountPayeeVisble = c.Boolean(nullable: false),
                        accountPayeeX = c.Single(nullable: false),
                        accountPayeeY = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequestLayout",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        branchAddress1Visble = c.Boolean(nullable: false),
                        branchAddress1X = c.Single(nullable: false),
                        branchAddress1Y = c.Single(nullable: false),
                        branchAddress2Visble = c.Boolean(nullable: false),
                        branchAddress2X = c.Single(nullable: false),
                        branchAddress2Y = c.Single(nullable: false),
                        chequeFrom1Visble = c.Boolean(nullable: false),
                        chequeFrom1X = c.Single(nullable: false),
                        chequeFrom1Y = c.Single(nullable: false),
                        chequeTo1Visble = c.Boolean(nullable: false),
                        chequeTo1X = c.Single(nullable: false),
                        chequeTo1Y = c.Single(nullable: false),
                        chequeFrom2Visble = c.Boolean(nullable: false),
                        chequeFrom2X = c.Single(nullable: false),
                        chequeFrom2Y = c.Single(nullable: false),
                        chequeTo2Visble = c.Boolean(nullable: false),
                        chequeTo2X = c.Single(nullable: false),
                        chequeTo2Y = c.Single(nullable: false),
                        nameAddress1Visble = c.Boolean(nullable: false),
                        nameAddress1X = c.Single(nullable: false),
                        nameAddress1Y = c.Single(nullable: false),
                        nameAddress2Visble = c.Boolean(nullable: false),
                        nameAddress2X = c.Single(nullable: false),
                        nameAddress2Y = c.Single(nullable: false),
                        accountNo1Visble = c.Boolean(nullable: false),
                        accountNo1X = c.Single(nullable: false),
                        accountNo1Y = c.Single(nullable: false),
                        accountNo2Visble = c.Boolean(nullable: false),
                        accountNo2X = c.Single(nullable: false),
                        accountNo2Y = c.Single(nullable: false),
                        barcode1Visble = c.Boolean(nullable: false),
                        barcode1X = c.Single(nullable: false),
                        barcode1Y = c.Single(nullable: false),
                        barcode2Visble = c.Boolean(nullable: false),
                        barcode2X = c.Single(nullable: false),
                        barcode2Y = c.Single(nullable: false),
                        audiText1Visble = c.Boolean(nullable: false),
                        audiText1X = c.Single(nullable: false),
                        audiText1Y = c.Single(nullable: false),
                        audiText2Visble = c.Boolean(nullable: false),
                        audiText2X = c.Single(nullable: false),
                        audiText2Y = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Request", "AtPar", c => c.String(nullable: false, maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Request", "AtPar");
            DropTable("dbo.RequestLayout");
            DropTable("dbo.ChequeLayout");
        }
    }
}
