using System.ComponentModel.DataAnnotations.Schema;

namespace ApiServico.Models
{
    [Table("candidatura")]
    public class Candidatura
    {
        [Column("id_cand")]
        public int Id { get; set; }

        [Column("id_est_fk")]
        public int EstudanteId { get; set; }

        public Estudante? Estudante { get; set; }

        [Column("id_vag_fk")]
        public int VagaId { get; set; }

        public Vaga? Vaga { get; set; }

        [Column("data_cand")]
        public DateTime DataCandidatura { get; set; } = DateTime.UtcNow;
    }
}
