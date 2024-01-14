using System.ComponentModel.DataAnnotations;

namespace ContactManager.Models
{
    public class Contacts
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ContactResponse
    {
        public IEnumerable<Contacts> Contacts { get; set; }
        public int? TotalPages { get; set; }
        public int? TotalRecords { get; set; }
    }
}
