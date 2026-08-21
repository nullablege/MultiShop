using System.ComponentModel.DataAnnotations;

namespace MultiShop.WebUI.Models.CargoDTOs;

public sealed class UpdateCargoCompanyDto
{
    [Range(1, int.MaxValue)]
    public int CargoCompanyId { get; set; }

    [Required(ErrorMessage = "Kargo şirketi adı zorunludur.")]
    [StringLength(100, ErrorMessage = "Kargo şirketi adı en fazla 100 karakter olabilir.")]
    public string CargoCompanyName { get; set; } = string.Empty;
}
