namespace RouteScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedToServiceRequested : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceRequesteds", "PreferredDayOne", c => c.String());
            AddColumn("dbo.ServiceRequesteds", "PreferredDayTwo", c => c.String());
            AddColumn("dbo.ServiceRequesteds", "PreferredDayThree", c => c.String());
            AddColumn("dbo.ServiceRequesteds", "PreferredTime", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceRequesteds", "PreferredTime");
            DropColumn("dbo.ServiceRequesteds", "PreferredDayThree");
            DropColumn("dbo.ServiceRequesteds", "PreferredDayTwo");
            DropColumn("dbo.ServiceRequesteds", "PreferredDayOne");
        }
    }
}
