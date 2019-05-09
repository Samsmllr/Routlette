namespace RouteScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ToDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServiceRequesteds", "PreferredDayOne", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ServiceRequesteds", "PreferredDayTwo", c => c.DateTime(nullable: true));
            AlterColumn("dbo.ServiceRequesteds", "PreferredDayThree", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceRequesteds", "PreferredDayThree", c => c.String());
            AlterColumn("dbo.ServiceRequesteds", "PreferredDayTwo", c => c.String());
            AlterColumn("dbo.ServiceRequesteds", "PreferredDayOne", c => c.String());
        }
    }
}
