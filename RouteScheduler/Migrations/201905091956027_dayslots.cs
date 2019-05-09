namespace RouteScheduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dayslots : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DaySlots",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PartOfDay = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DaySlots");
        }
    }
}
