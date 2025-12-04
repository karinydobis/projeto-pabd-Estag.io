using System.ComponentModel.DataAnnotations.Schema;

namespace ApiServico.Models
{
    [Table("vaga")]
    public class Vaga
    {
        [Column("id_vag")]
        public int Id { get; set; }

        [Column("id_emp_fk")]
        public int EmpresaId { get; set; }

        public Empresa? Empresa { get; set; }

        [Column("titulo_vag")]
        public string Titulo { get; set; }

        [Column("descricao_vag")]
        public string Descricao { get; set; }

        [Column("requisitos_vag")]
        public string Requisitos { get; set; }

        [Column("modalidade_vag")]
        public string Modalidade { get; set; }

        [Column("beneficios_vag")]
        public string? Beneficios { get; set; }

        [Column("data_vag")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public ICollection<Candidatura> Candidaturas { get; set; } = [];
    }
}
