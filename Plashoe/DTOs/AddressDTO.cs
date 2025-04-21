// DTOs/AddressDTO.cs
namespace Plashoe.DTOs
{
    public class AddressDTO
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool IsDefault { get; set; }
    }
}
