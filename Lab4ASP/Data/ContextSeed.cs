using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;
using Lab4ASP.Enums;
using Lab4ASP.Models;
using Microsoft.AspNetCore.Identity;

namespace Lab4ASP.Data
{

    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Basic.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager )
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "alessia@gmail.com",
                FirstName = "Alessia",
                LastName = "Ullsten",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
                }

            }
        }
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.BookTypes.Any())
            {
                var bookTypes = new BookType[]
                {
           new BookType {BookTypeName = "Biography"},
           new BookType {BookTypeName = "Novel"},
           new BookType {BookTypeName = "Mystery"},
           new BookType {BookTypeName = "Romance"},
           new BookType {BookTypeName = "Science fiction"},
           new BookType {BookTypeName = "History"},
           new BookType {BookTypeName = "Cook book"},
           new BookType {BookTypeName = "Philosophy"}
                };
                context.BookTypes.AddRange(bookTypes);
                context.SaveChanges();
            }

            if (!context.Books.Any())
            {
                var books = new Book[]
                {
            new Book { BookTitle = "The Great Gatsby", BookDescription = "A classic novel about the Roaring Twenties", PublishedYear = 1925, FK_BookTypeId = 2 },
            new Book { BookTitle = "To Kill a Mockingbird", BookDescription = "A story about racism and injustice in the American South", PublishedYear = 1960, FK_BookTypeId = 5 },
            new Book { BookTitle = "The Lord of the Rings", BookDescription = "A fantasy epic about a quest to destroy an evil ring", PublishedYear = 1954, FK_BookTypeId = 4 },
            new Book { BookTitle = "Pride and Prejudice", BookDescription = "A romantic comedy about the Bennet family", PublishedYear = 1813, FK_BookTypeId = 3 },
            new Book { BookTitle = "1984", BookDescription = "A dystopian novel about a totalitarian government", PublishedYear = 1949, FK_BookTypeId = 4 },
            new Book { BookTitle = "The Catcher in the Rye", BookDescription = "A coming-of-age story about a teenage boy in New York City", PublishedYear = 1951, FK_BookTypeId = 2 },
            new Book { BookTitle = "Moby-Dick", BookDescription = "A novel about Captain Ahab's obsession with a white whale", PublishedYear = 1851, FK_BookTypeId = 5 },
            new Book { BookTitle = "The Hitchhiker's Guide to the Galaxy", BookDescription = "A humorous science fiction book about the end of the world", PublishedYear = 1979, FK_BookTypeId = 4 },
            new Book { BookTitle = "The Hunger Games", BookDescription = "A dystopian novel about a teenage girl who must compete in a fight to the death", PublishedYear = 2008, FK_BookTypeId = 4 },
            new Book { BookTitle = "Gone with the Wind", BookDescription = "A historical romance set in the American South during the Civil War", PublishedYear = 1936, FK_BookTypeId = 3 }
                };

                context.Books.AddRange(books);
                context.SaveChanges();
            }

        }

    }
}


