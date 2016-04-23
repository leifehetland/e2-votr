namespace Votr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        OptionId = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Poll_PollId = c.Int(),
                    })
                .PrimaryKey(t => t.OptionId)
                .ForeignKey("dbo.Polls", t => t.Poll_PollId)
                .Index(t => t.Poll_PollId);
            
            CreateTable(
                "dbo.Polls",
                c => new
                    {
                        PollId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.PollId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Options", "Poll_PollId", "dbo.Polls");
            DropIndex("dbo.Options", new[] { "Poll_PollId" });
            DropTable("dbo.Polls");
            DropTable("dbo.Options");
        }
    }
}
