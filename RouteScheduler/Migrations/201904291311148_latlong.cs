namespace RouteScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class latlong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessOwners", "Latitude", c => c.Double(nullable: true));
            AddColumn("dbo.BusinessOwners", "Longitude", c => c.Double(nullable: true));
            AddColumn("dbo.Customers", "Latitude", c => c.Double(nullable: true));
            AddColumn("dbo.Customers", "Longitude", c => c.Double(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Longitude");
            DropColumn("dbo.Customers", "Latitude");
            DropColumn("dbo.BusinessOwners", "Longitude");
            DropColumn("dbo.BusinessOwners", "Latitude");
        }
    }
}
