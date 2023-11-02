namespace Search_n_View.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbnew : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CanView = c.Boolean(nullable: false),
                        CanAddNote = c.Boolean(nullable: false),
                        CanPrint = c.Boolean(nullable: false),
                        CanDownload = c.Boolean(nullable: false),
                        CanUpload = c.Boolean(nullable: false),
                        CanEmail = c.Boolean(nullable: false),
                        AgShortName = c.String(),
                        AddedBy = c.String(),
                        AddedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ActiveDrives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BasePath = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        AddedBy = c.String(),
                        AddedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocPath = c.String(),
                        ClaimNo = c.String(),
                        ItID = c.Int(nullable: false),
                        DtId = c.Int(nullable: false),
                        UId = c.String(maxLength: 128),
                        Extension = c.String(),
                        IsActive = c.Boolean(),
                        BaseDrivePath = c.String(),
                        Date = c.DateTime(),
                        FromDate = c.DateTime(),
                        ToDate = c.DateTime(),
                        AddedBy = c.String(),
                        AddedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentTypes", t => t.DtId, cascadeDelete: true)
                .ForeignKey("dbo.ItemTypes", t => t.ItID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UId)
                .Index(t => t.ItID)
                .Index(t => t.DtId)
                .Index(t => t.UId);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ItID = c.Int(),
                        AddedBy = c.String(),
                        AddedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItemTypes", t => t.ItID)
                .Index(t => t.ItID);
            
            CreateTable(
                "dbo.ItemTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MdId = c.Int(nullable: false),
                        AddedBy = c.String(),
                        AddedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MainDepartments", t => t.MdId, cascadeDelete: true)
                .Index(t => t.MdId);
            
            CreateTable(
                "dbo.MainDepartments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AddedBy = c.String(),
                        AddedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        CNIC = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        AcId = c.Int(nullable: false),
                        IsMaker = c.Boolean(),
                        IsChecker = c.Boolean(),
                        MdId = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessGroups", t => t.AcId, cascadeDelete: true)
                .ForeignKey("dbo.MainDepartments", t => t.MdId, cascadeDelete: true)
                .Index(t => t.AcId)
                .Index(t => t.MdId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.DocumentHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Viewed = c.Boolean(nullable: false),
                        AddedNote = c.Boolean(nullable: false),
                        Printed = c.Boolean(nullable: false),
                        Downloaded = c.Boolean(nullable: false),
                        Uploaded = c.Boolean(nullable: false),
                        Emailed = c.Boolean(nullable: false),
                        EmailRecipients = c.String(),
                        EmailBccRecipients = c.String(),
                        EmailSubject = c.String(),
                        EmailBody = c.String(),
                        UserNote = c.String(),
                        UId = c.String(maxLength: 128),
                        DId = c.Int(nullable: false),
                        AddedBy = c.String(),
                        AddedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Documents", t => t.DId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UId)
                .Index(t => t.UId)
                .Index(t => t.DId);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Recipients = c.String(),
                        AddedBy = c.String(),
                        AddedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ErrorLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassName = c.String(),
                        FunctionName = c.String(),
                        Message = c.String(),
                        Source = c.String(),
                        Stacktrace = c.String(),
                        AddedBy = c.String(),
                        AddedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Note = c.String(),
                        UId = c.String(maxLength: 128),
                        DId = c.Int(nullable: false),
                        AddedBy = c.String(),
                        AddedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Documents", t => t.DId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UId)
                .Index(t => t.UId)
                .Index(t => t.DId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserItemTypes",
                c => new
                    {
                        UItId = c.Int(nullable: false, identity: true),
                        UID = c.String(maxLength: 128),
                        ItID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UItId)
                .ForeignKey("dbo.ItemTypes", t => t.ItID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UID)
                .Index(t => t.UID)
                .Index(t => t.ItID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserItemTypes", "UID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserItemTypes", "ItID", "dbo.ItemTypes");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Notes", "UId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notes", "DId", "dbo.Documents");
            DropForeignKey("dbo.DocumentHistories", "UId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentHistories", "DId", "dbo.Documents");
            DropForeignKey("dbo.Documents", "UId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "MdId", "dbo.MainDepartments");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "AcId", "dbo.AccessGroups");
            DropForeignKey("dbo.Documents", "ItID", "dbo.ItemTypes");
            DropForeignKey("dbo.Documents", "DtId", "dbo.DocumentTypes");
            DropForeignKey("dbo.DocumentTypes", "ItID", "dbo.ItemTypes");
            DropForeignKey("dbo.ItemTypes", "MdId", "dbo.MainDepartments");
            DropIndex("dbo.UserItemTypes", new[] { "ItID" });
            DropIndex("dbo.UserItemTypes", new[] { "UID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Notes", new[] { "DId" });
            DropIndex("dbo.Notes", new[] { "UId" });
            DropIndex("dbo.DocumentHistories", new[] { "DId" });
            DropIndex("dbo.DocumentHistories", new[] { "UId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "MdId" });
            DropIndex("dbo.AspNetUsers", new[] { "AcId" });
            DropIndex("dbo.ItemTypes", new[] { "MdId" });
            DropIndex("dbo.DocumentTypes", new[] { "ItID" });
            DropIndex("dbo.Documents", new[] { "UId" });
            DropIndex("dbo.Documents", new[] { "DtId" });
            DropIndex("dbo.Documents", new[] { "ItID" });
            DropTable("dbo.UserItemTypes");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Notes");
            DropTable("dbo.ErrorLogs");
            DropTable("dbo.Emails");
            DropTable("dbo.DocumentHistories");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.MainDepartments");
            DropTable("dbo.ItemTypes");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.Documents");
            DropTable("dbo.ActiveDrives");
            DropTable("dbo.AccessGroups");
        }
    }
}
