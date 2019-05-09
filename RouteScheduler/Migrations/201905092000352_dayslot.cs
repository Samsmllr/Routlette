namespace RouteScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dayslot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceRequesteds", "Id", c => c.Int(nullable: false));
            CreateIndex("dbo.ServiceRequesteds", "Id");
            AddForeignKey("dbo.ServiceRequesteds", "Id", "dbo.DaySlots", "id", cascadeDelete: true);
            DropColumn("dbo.ServiceRequesteds", "PreferredTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceRequesteds", "PreferredTime", c => c.String());
            DropForeignKey("dbo.ServiceRequesteds", "Id", "dbo.DaySlots");
            DropIndex("dbo.ServiceRequesteds", new[] { "Id" });
            DropColumn("dbo.ServiceRequesteds", "Id");
        }
    }
}
