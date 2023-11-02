using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Search_n_View.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkId=317594 to learn more.
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<AccessGroup> AccessGroup { get; set; }
        public DbSet<ActiveDrive> ActiveDrive { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<DocumentHistory> DocumentHistory { get; set; }
        public DbSet<DocumentType> DocumentType { get; set; }

        public DbSet<EmailAttachment> EmailAttachment { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<ItemType> ItemType { get; set; }
        public DbSet<MainDepartment> MainDepartment { get; set; }
        public DbSet<UserItemType> UserItemType { get; set; }

        public DbSet<UserDepartment> UserDepartment { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<AlAinDocument> AlAinDocument { get; set; }


    }
}