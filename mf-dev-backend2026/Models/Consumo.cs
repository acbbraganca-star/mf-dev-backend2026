using mf_dev_backend2026.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mf_dev_backend2026.Models;

[Table("Consumos")]
public class Consumo
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data é obrigatória.")]
    [DataType(DataType.Date)]
    public DateTime Data { get; set; }

    [Required(ErrorMessage = "O local é obrigatório.")]
    public string Local { get; set; } = string.Empty;

    [Required(ErrorMessage = "O valor é obrigatório.")]
    [Column(TypeName = "decimal(18,2)")]          
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "A quilometragem é obrigatória.")]
    public int Quilometragem { get; set; }

    [Display(Name = "Tipo de Combustível")]
    public TipoCombustivel TipoCombustivel { get; set; }


    [Required(ErrorMessage = "Obrigatório informar o veículo.")]
    [Display(Name = "Veículo")]
    public int VeiculoId { get; set; }

   
    [ForeignKey(nameof(VeiculoId))]
    public Veículo? Veiculo { get; set; }

    
}
