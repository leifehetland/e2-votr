namespace Votr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVoteModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        VoteId = c.Int(nullable: false, identity: true),
                        Choice_OptionId = c.Int(),
                        Poll_PollId = c.Int(),
                        Voter_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.VoteId)
                .ForeignKey("dbo.Options", t => t.Choice_OptionId)
                .ForeignKey("dbo.Polls", t => t.Poll_PollId)
                .ForeignKey("dbo.AspNetUsers", t => t.Voter_Id)
                .Index(t => t.Choice_OptionId)
                .Index(t => t.Poll_PollId)
                .Index(t => t.Voter_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "Voter_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Votes", "Poll_PollId", "dbo.Polls");
            DropForeignKey("dbo.Votes", "Choice_OptionId", "dbo.Options");
            DropIndex("dbo.Votes", new[] { "Voter_Id" });
            DropIndex("dbo.Votes", new[] { "Poll_PollId" });
            DropIndex("dbo.Votes", new[] { "Choice_OptionId" });
            DropTable("dbo.Votes");
        }
    }
}
