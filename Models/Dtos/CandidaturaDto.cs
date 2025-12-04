using System.ComponentModel.DataAnnotations;

namespace ApiServico.Models.Dtos
{
    public class CandidaturaDto
    {
        [Required]
        public int EstudanteId { get; set; }

        [Required]
        public int VagaId { get; set; }
    }
}
