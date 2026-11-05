namespace CPS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddCustomerID : DbMigration
    {
        public override void Up()
        {
            // CustomerID column already exists.
            // ChequeSeries table already exists.
        }

        public override void Down()
        {
            // Intentionally left blank.
        }
    }
}