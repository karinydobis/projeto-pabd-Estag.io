using System.ComponentModel.DataAnnotations;

namespace ApiServico.Models.Dtos
{
    public class EmpresaDto
    {
        [Required]
        public string RazaoSocial { get; set; }

        [Required]
        [Length(14, 18, ErrorMessage = "CNPJ inválido")]
        public string CNPJ { get; set; }

        public string? AreaAtuacao { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? LogoUrl { get; set; }
    }
}
