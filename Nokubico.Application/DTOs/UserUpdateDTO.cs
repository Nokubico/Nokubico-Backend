using System.ComponentModel.DataAnnotations;

namespace Nokubico.Application.DTOs;

public class UserUpdateDTO
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Name { get; set; } = string.Empty;

    public string? Bio { get; set; }
    public string? Location { get; set; }
    public string? Profession { get; set; }
    public string? Image { get; set; }
}
