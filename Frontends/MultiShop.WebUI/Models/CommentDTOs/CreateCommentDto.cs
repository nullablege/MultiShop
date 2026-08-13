using System.ComponentModel.DataAnnotations;

namespace MultiShop.WebUI.Models.CommentDTOs
{
    public class CreateCommentDto
    {
        [Required]
        [StringLength(64)]
        public string ProductId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adınız soyadınız zorunludur.")]
        [StringLength(100)]
        public string NameSurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yorumunuz zorunludur.")]
        [MinLength(2)]
        [StringLength(2000)]
        public string CommentDetail { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Ürün puanınızı seçiniz.")]
        public int Rating { get; set; }
    }
}
