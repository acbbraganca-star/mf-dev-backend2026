using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mf_dev_backend2026.Models
{
    [Table("Veículo")]
    public class Veículo
    {
        [Key]
        public  int Id { get; set; }

        [Required (ErrorMessage = "Obrigatório informar o nome")]
        public string Nome { get; set; }

        [Required (ErrorMessage = "Obrigatório informar a placa")]
        public  string Placa { get; set; }

        [Display(Name = "Ano de Fabricação")]
        [Required(ErrorMessage = "Obrigatório informar o ano de fabricação")]
        public int? AnoFabricacao { get; set; }

        [Display(Name = "Ano do Modelo")]
        [Required(ErrorMessage = "Obrigatório informar o ano do modelo")]
        public int? AnoModelo { get; set; }

    }
}
