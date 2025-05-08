namespace LibraryAppMVC.Data
{
    using LibraryAppMVC.Models;
    using Microsoft.EntityFrameworkCore;

    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Fluent API to configure relationships

            modelBuilder
                .Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            modelBuilder
                .Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId);

            modelBuilder
                .Entity<Book>()
                .HasOne(b => b.Borrower)
                .WithMany(u => u.BorrowedBooks)
                .HasForeignKey(b => b.BorrowerId)
                .OnDelete(DeleteBehavior.SetNull); // optional: null out borrower if user is deleted
        }
    }
}
