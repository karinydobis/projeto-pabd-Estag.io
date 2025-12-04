using System.ComponentModel.DataAnnotations;

namespace ApiServico.Models.Dtos
{
    public class EstudanteDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required]
        public string Curso { get; set; }

        public string? Stacks { get; set; }
        public string? GitHub { get; set; }
        public string? Portfolio { get; set; }
    }
}
