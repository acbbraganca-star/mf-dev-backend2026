using System.ComponentModel.DataAnnotations;

namespace mf_dev_backend2026.Models;

public class Usuario
{
    [Key]
    [Required(ErrorMessage = "O código é obrigatório.")]
    [Display(Name = "Código")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]    
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "O perfil é obrigatório.")]
    public Perfil Perfil { get; set; }
}