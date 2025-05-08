namespace LibraryAppMVC.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Book
    {
        public int BookId { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(20)]
        public string ISBN { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public int? BorrowerId { get; set; }

        // Navigation properties
        public Author Author { get; set; }
        public Publisher Publisher { get; set; }
        public User Borrower { get; set; }
        public bool IsAvailable => BorrowerId == null;
    }
}
