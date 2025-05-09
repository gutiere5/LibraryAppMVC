namespace LibraryAppMVC.Models
{
    using LibraryAppMVC.Data;
    using Microsoft.EntityFrameworkCore;

    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (
                var context = new LibraryContext(
                    serviceProvider.GetRequiredService<DbContextOptions<LibraryContext>>()
                )
            )
            {
                // Return if data already exists
                if (
                    context.Books.Any()
                    || context.Authors.Any()
                    || context.Publishers.Any()
                    || context.Users.Any()
                )
                {
                    return;
                }

                // Seed Authors
                var author1 = new Author
                {
                    FullName = "Robert C. Martin",
                    Biography =
                        "Robert C. Martin, also known as Uncle Bob, is a software engineer and author.",
                };

                var author2 = new Author
                {
                    FullName = "Andy Hunt",
                    Biography =
                        "Andy Hunt is a programmer turned author, known for 'The Pragmatic Programmer'.",
                };

                context.Authors.AddRange(author1, author2);

                // Seed Publishers
                var publisher1 = new Publisher
                {
                    Name = "Prentice Hall",
                    Address = "221 River Street, Hoboken, NJ",
                };

                var publisher2 = new Publisher
                {
                    Name = "Addison-Wesley",
                    Address = "75 Arlington Street, Boston, MA",
                };

                context.Publishers.AddRange(publisher1, publisher2);

                var user1 = new User
                {
                    UserName = "jdoe",
                    Password = "Password123",
                    Email = "jdoe@example.com",
                    Role = "Member",
                };

                var user2 = new User
                {
                    UserName = "admin",
                    Password = "Admin123",
                    Email = "admin@example.com",
                    Role = "Admin",
                };

                context.Users.AddRange(user1, user2);

                context.SaveChanges();

                var book1 = new Book
                {
                    Title = "Clean Code",
                    ISBN = "9780132350884",
                    AuthorId = author1.AuthorId,
                    PublisherId = publisher1.PublisherId,
                    BorrowerId = user1.UserId,
                };

                var book2 = new Book
                {
                    Title = "The Pragmatic Programmer",
                    ISBN = "9780201616224",
                    AuthorId = author2.AuthorId,
                    PublisherId = publisher2.PublisherId,
                    BorrowerId = null,
                };
                var book3 = new Book
                {
                    Title = "Refactoring: Improving the Design of Existing Code",
                    ISBN = "9780201485677",
                    AuthorId = author1.AuthorId,
                    PublisherId = publisher1.PublisherId,
                    BorrowerId = null,
                };

                var book4 = new Book
                {
                    Title = "Test-Driven Development: By Example",
                    ISBN = "9780321146533",
                    AuthorId = author2.AuthorId,
                    PublisherId = publisher2.PublisherId,
                    BorrowerId = null,
                };

                context.Books.AddRange(book1, book2, book3, book4);

                context.SaveChanges();
            }
        }
    }
}
