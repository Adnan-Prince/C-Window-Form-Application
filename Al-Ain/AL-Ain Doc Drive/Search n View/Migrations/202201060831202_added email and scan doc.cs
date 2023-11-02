namespace Search_n_View.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedemailandscandoc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocPath = c.String(),
                        AttachmentName = c.String(),
                        Header = c.String(),
                        Recipients = c.String(),
                        Body = c.String(),
                        AddedBy = c.String(),
                        AddedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Documents", "DEId", c => c.Int());
            AddColumn("dbo.Documents", "ScannedFilePath", c => c.String());
            AddColumn("dbo.DocumentHistories", "Scanned", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Documents", "DEId");
            AddForeignKey("dbo.Documents", "DEId", "dbo.EmailAttachments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "DEId", "dbo.EmailAttachments");
            DropIndex("dbo.Documents", new[] { "DEId" });
            DropColumn("dbo.DocumentHistories", "Scanned");
            DropColumn("dbo.Documents", "ScannedFilePath");
            DropColumn("dbo.Documents", "DEId");
            DropTable("dbo.EmailAttachments");
        }
    }
}
