using System.ComponentModel.DataAnnotations.Schema;

namespace ApiServico.Models
{
    
        [Table("estudante")]
        public class Estudante
        {
            [Column("id_est")]
            public int Id { get; set; }

            [Column("nome_est")]
            public string Nome { get; set; }

            [Column("email_est")]
            public string Email { get; set; }

            [Column("curso_est")]
            public string Curso { get; set; }

            [Column("stacks_est")]
            public string? Stacks { get; set; }

            [Column("github_est")]
            public string? GitHub { get; set; }

            [Column("portfolio_est")]
            public string? Portfolio { get; set; }

            [Column("ativo_est")]
            public bool Ativo { get; set; } = true;

            public ICollection<Candidatura> Candidaturas { get; set; } = [];
        }
    }

