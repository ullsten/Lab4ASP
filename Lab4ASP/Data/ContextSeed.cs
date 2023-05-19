using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;
using Lab4ASP.Enums;
using Lab4ASP.Models;
using Lab4ASP.Models.JunctionTables;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            context.Database.Migrate();

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

            if (!context.Users.Any())
            {
                var users = new Users[]
                {
                    new Users { FirstName ="Oskar", LastName = "Ullsten", PhoneNumber="0730913046", Email="ullzten@gmail.com"},
                    new Users { FirstName ="Gong", LastName = "Jonsson", PhoneNumber="0706744671", Email="gong@gmail.com"},
                    new Users { FirstName ="Alessia", LastName = "Ullsten", PhoneNumber="0703154556", Email="alessia@gmail.com"},
                    new Users { FirstName ="August", LastName = "Ullsten", PhoneNumber="073091455", Email="august@gmail.com"},
                    new Users { FirstName ="Müsli", LastName = "Ullsten", PhoneNumber="0703414559", Email="Müsli@gmail.com"},
                    new Users { FirstName ="Louie", LastName = "Willington", PhoneNumber="0703414559", Email="willington@gmail.com"},
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            if (!context.Addresses.Any())
            {
                var addresses = new Address[]
{
                    new Address { Street = "Byskillnadsvägen 14", City = "Köpmanholmen", PostalCode = "89340", FK_UserId = 1 },
                    new Address { Street = "456 Elm Avenue", City = "Los Angeles", PostalCode = "90001", FK_UserId = 2},
                    new Address { Street = "789 Oak Drive", City = "Chicago", PostalCode = "60601", FK_UserId = 3 },
                    new Address { Street = "321 Pine Street", City = "San Francisco", PostalCode = "94101", FK_UserId = 4 },
                    new Address { Street = "987 Maple Lane", City = "Seattle", PostalCode = "98101", FK_UserId = 5 },
                    new Address { Street = "654 Cedar Road", City = "Miami", PostalCode = "33101", FK_UserId = 6 },
                };

                context.Addresses.AddRange(addresses);
                context.SaveChanges();
            }

            if (!context.Authors.Any())
            {
                var authors = new Author[]
                {
                    new Author { AuthorName = "F. Scott Fitzgerald" },
                    new Author { AuthorName = "Harper Lee" },
                    new Author { AuthorName = "J.R.R. Tolkien" },
                    new Author { AuthorName = "Jane Austen" },
                    new Author { AuthorName = "George Orwell" },
                    new Author { AuthorName = "J.D. Salinger" },
                    new Author { AuthorName = "Herman Melville" },
                    new Author { AuthorName = "Douglas Adams" },
                    new Author { AuthorName = "Suzanne Collins" },
                    new Author { AuthorName = "Margaret Mitchell" }
                };

                context.Authors.AddRange(authors);
                context.SaveChanges();
            }

            if (!context.Books.Any())
            {
                var books = new Book[]
                {
                    new Book { BookTitle = "The Great Gatsby", BookDescription = "A classic novel about the Roaring Twenties", PublishedYear = 1925, Quantity = 2, FK_BookTypeId = 2 },
                    new Book { BookTitle = "To Kill a Mockingbird", BookDescription = "A story about racism and injustice in the American South", PublishedYear = 1960, Quantity = 2, FK_BookTypeId = 5 },
                    new Book { BookTitle = "The Lord of the Rings", BookDescription = "A fantasy epic about a quest to destroy an evil ring", PublishedYear = 1954, Quantity = 2, FK_BookTypeId = 4 },
                    new Book { BookTitle = "Pride and Prejudice", BookDescription = "A romantic comedy about the Bennet family", PublishedYear = 1813, Quantity = 2, FK_BookTypeId = 3 },
                    new Book { BookTitle = "1984", BookDescription = "A dystopian novel about a totalitarian government", PublishedYear = 1949, Quantity = 2, FK_BookTypeId = 4 },
                    new Book { BookTitle = "The Catcher in the Rye", BookDescription = "A coming-of-age story about a teenage boy in New York City", PublishedYear = 1951, Quantity = 2, FK_BookTypeId = 2 },
                    new Book { BookTitle = "Moby-Dick", BookDescription = "A novel about Captain Ahab's obsession with a white whale", PublishedYear = 1851, Quantity = 2, FK_BookTypeId = 5 },
                    new Book { BookTitle = "The Hitchhiker's Guide to the Galaxy", BookDescription = "A humorous science fiction book about the end of the world", PublishedYear = 1979, Quantity = 2, FK_BookTypeId = 4 },
                    new Book { BookTitle = "The Hunger Games", BookDescription = "A dystopian novel about a teenage girl who must compete in a fight to the death", PublishedYear = 2008, Quantity = 2, FK_BookTypeId = 4 },
                    new Book { BookTitle = "Gone with the Wind", BookDescription = "A historical romance set in the American South during the Civil War", PublishedYear = 1936, Quantity = 2, FK_BookTypeId = 3 }
                };

                context.Books.AddRange(books);
                context.SaveChanges();
            }

            if (!context.BooksAuthors.Any())
            {
                var bookAuthors = new BookAuthor[]
                {
                    new BookAuthor { AuthorId =1, FK_BookId=1},
                    new BookAuthor { AuthorId =2, FK_BookId=2},
                    new BookAuthor { AuthorId =3, FK_BookId=3},
                    new BookAuthor { AuthorId =4, FK_BookId=4},
                    new BookAuthor { AuthorId =5, FK_BookId=5},
                    new BookAuthor { AuthorId =6, FK_BookId=6},
                    new BookAuthor { AuthorId =7, FK_BookId=7},
                    new BookAuthor { AuthorId =8, FK_BookId=8},
                    new BookAuthor { AuthorId =9, FK_BookId=9},
                    new BookAuthor { AuthorId =10, FK_BookId=10},
                };

                context.BooksAuthors.AddRange(bookAuthors);
                context.SaveChanges();
            }
        }

    }
}


