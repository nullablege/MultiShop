using System.ComponentModel.DataAnnotations;

namespace MultiShop.WebUI.Models.Order;

public sealed class CreateOrderAddressDto
{
    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    [MaxLength(100)]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon alanı zorunludur.")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
    [MaxLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ülke alanı zorunludur.")]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "İlçe alanı zorunludur.")]
    [MaxLength(100)]
    public string District { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şehir alanı zorunludur.")]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adres satırı zorunludur.")]
    [MaxLength(300)]
    public string Detail1 { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Detail2 { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Posta kodu zorunludur.")]
    [MaxLength(20)]
    public string ZipCode { get; set; } = string.Empty;
}
