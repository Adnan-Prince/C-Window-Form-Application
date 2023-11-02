namespace Search_n_View.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeduserdepartment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "MdId", "dbo.MainDepartments");
            DropIndex("dbo.AspNetUsers", new[] { "MdId" });
            CreateTable(
                "dbo.UserDepartments",
                c => new
                    {
                        UDpId = c.Int(nullable: false, identity: true),
                        UID = c.String(maxLength: 128),
                        MdID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UDpId)
                .ForeignKey("dbo.MainDepartments", t => t.MdID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UID)
                .Index(t => t.UID)
                .Index(t => t.MdID);
            
            AddColumn("dbo.Documents", "Email", c => c.String());
            DropColumn("dbo.AspNetUsers", "MdId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "MdId", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserDepartments", "UID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserDepartments", "MdID", "dbo.MainDepartments");
            DropIndex("dbo.UserDepartments", new[] { "MdID" });
            DropIndex("dbo.UserDepartments", new[] { "UID" });
            DropColumn("dbo.Documents", "Email");
            DropTable("dbo.UserDepartments");
            CreateIndex("dbo.AspNetUsers", "MdId");
            AddForeignKey("dbo.AspNetUsers", "MdId", "dbo.MainDepartments", "Id", cascadeDelete: true);
        }
    }
}
