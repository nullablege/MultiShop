using System.ComponentModel.DataAnnotations;

namespace MultiShop.WebUI.Models.Catalog.ContactDTOs;

public sealed class CreateContactDto
{
    [Required(ErrorMessage = "Ad soyad alanı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Ad soyad en fazla 100 karakter olabilir.")]
    [Display(Name = "Ad Soyad")]
    public string NameSurname { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
    [MaxLength(256, ErrorMessage = "E-posta en fazla 256 karakter olabilir.")]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Konu alanı zorunludur.")]
    [MaxLength(200, ErrorMessage = "Konu en fazla 200 karakter olabilir.")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mesaj alanı zorunludur.")]
    [StringLength(4000, MinimumLength = 10, ErrorMessage = "Mesaj 10-4000 karakter arasında olmalıdır.")]
    public string Message { get; set; } = string.Empty;
}
