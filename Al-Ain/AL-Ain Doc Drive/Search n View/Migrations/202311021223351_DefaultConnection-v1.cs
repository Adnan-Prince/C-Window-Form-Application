namespace Search_n_View.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultConnectionv1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AlAinDocuments", "MdId", c => c.Int());
            CreateIndex("dbo.AlAinDocuments", "MdId");
            AddForeignKey("dbo.AlAinDocuments", "MdId", "dbo.MainDepartments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AlAinDocuments", "MdId", "dbo.MainDepartments");
            DropIndex("dbo.AlAinDocuments", new[] { "MdId" });
            DropColumn("dbo.AlAinDocuments", "MdId");
        }
    }
}
