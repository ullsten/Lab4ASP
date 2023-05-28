using System.Security.Principal;
using Lab4ASP.Models;
using Lab4ASP.Models.JunctionTables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab4ASP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //public DbSet<Users> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookType> BookTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<BookAuthor> BooksAuthors { get; set; }
        public DbSet<LoanHistory> LoanHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.HasDefaultSchema("Identity");

            //builder.Entity<Users>().ToTable("Users");
            builder.Entity<Author>().ToTable("Authors");
            builder.Entity<Book>().ToTable("Books");
            builder.Entity<Address>().ToTable("Addresses");
            builder.Entity<BookAuthor>().ToTable("BooksAuthors");
            builder.Entity<LoanHistory>().ToTable("LoanHistories");


            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "IdentityUser");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "IdentityRole");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("IdentityUserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("IdentityUserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("IdentityUserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("IdentityRoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("IdentityUserTokens");
            });
        }
    }
}