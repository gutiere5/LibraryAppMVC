namespace LibraryAppMVC.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Author
    {
        public int AuthorId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(1000)]
        public string Biography { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
