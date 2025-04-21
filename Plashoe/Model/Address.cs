using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Plashoe.Model
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string AddressLine1 { get; set; }

        [StringLength(200)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string State { get; set; }

        [Required]
        [StringLength(20)]
        public string ZipCode { get; set; }

        public bool IsDefault { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
