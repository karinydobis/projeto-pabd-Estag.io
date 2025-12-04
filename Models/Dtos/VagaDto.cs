using System.ComponentModel.DataAnnotations;

namespace ApiServico.Models.Dtos
{
    public class VagaDto
    {
        [Required]
        public int EmpresaId { get; set; }

        [Required]
        [Length(5, 150)]
        public string Titulo { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public string Requisitos { get; set; }

        [Required]
        public string Modalidade { get; set; }

        public string? Beneficios { get; set; }
    }
}
