using System.ComponentModel.DataAnnotations;

namespace Plashoe.Model
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public bool IsBlocked { get; set; }
        public string? Role { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual ICollection<Address>? Addresses { get; set; }
        public virtual List<Wishlist>? Wishlist { get; set; }
        public virtual List<Order>? Orders { get; set; }
    }
}
