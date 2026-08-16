
using System.ComponentModel.DataAnnotations;

namespace MultiShop.Catalog.DTOs.ContactDTOs
{
    public class CreateContactDto
    {
        [Required]
        [MaxLength(100)]
        public string NameSurname { get; set; } = string.Empty;
        [EmailAddress]
        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        public string Subject { get; set; } = string.Empty;
        [Required]
        [Length(10, 4000)]
        public string Message { get; set; } = string.Empty;
    }
}
