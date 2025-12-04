using System.ComponentModel.DataAnnotations.Schema;

namespace ApiServico.Models
{
    [Table("empresa")]
    public class Empresa
    {
        [Column("id_emp")]
        public int Id { get; set; }

        [Column("razao_emp")]
        public string RazaoSocial { get; set; }

        [Column("cnpj_emp")]
        public string CNPJ { get; set; }

        [Column("area_emp")]
        public string? AreaAtuacao { get; set; }

        [Column("email_emp")]
        public string Email { get; set; }

        [Column("logo_emp")]
        public string? LogoUrl { get; set; }

        [Column("ativo_emp")]
        public bool Ativo { get; set; } = true;

        public ICollection<Vaga> Vagas { get; set; } = [];
    }
}
