namespace LibraryAppMVC.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Publisher
    {
        public int PublisherId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        // Navigation property
        public ICollection<Book> Books { get; set; }
    }
}
