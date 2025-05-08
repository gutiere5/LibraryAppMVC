namespace LibraryAppMVC.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        // Navigation property
        public string Role { get; set; } // e.g., "Admin", "Librarian", "Member"
        public ICollection<Book> BorrowedBooks { get; set; }
    }
}
