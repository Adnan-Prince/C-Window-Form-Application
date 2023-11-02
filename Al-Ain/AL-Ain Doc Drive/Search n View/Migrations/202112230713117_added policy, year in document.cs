namespace Search_n_View.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedpolicyyearindocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "PolicyNo", c => c.String());
            AddColumn("dbo.Documents", "Year", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "Year");
            DropColumn("dbo.Documents", "PolicyNo");
        }
    }
}
