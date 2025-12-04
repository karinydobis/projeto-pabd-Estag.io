using ApiServico.DataContexts;
using Microsoft.EntityFrameworkCore;
using ApiServico.Models.Dtos;
using ApiServico.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiServico.Controllers
{
    [Route("/candidaturas")]
    [ApiController]
    public class CandidaturaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CandidaturaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarTodas()
        {
            var candidaturas = await _context.Candidaturas
                .Include(c => c.Estudante)
                .Include(c => c.Vaga)
                .ThenInclude(v => v.Empresa)
                .Select(c => new
                {
                    CandidaturaId = c.Id,
                    Data = c.DataCandidatura,

                    Estudante = new
                    {
                        c.Estudante.Nome,
                        c.Estudante.Email,
                        c.Estudante.Curso
                    },

                    Vaga = new
                    {
                        c.Vaga.Titulo,
                        c.Vaga.Modalidade
                    },

                    Empresa = new
                    {
                        c.Vaga.Empresa.RazaoSocial
                    }
                })
                .ToListAsync();

            return Ok(candidaturas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var candidatura = await _context.Candidaturas
                .Include(c => c.Estudante)
                .Include(c => c.Vaga)
                    .ThenInclude(v => v.Empresa)
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    Estudante = new
                    {
                        c.Estudante.Nome,
                        c.Estudante.Email,
                        c.Estudante.Curso
                    },

                    Vaga = new
                    {
                        c.Vaga.Titulo,
                        Empresa = c.Vaga.Empresa.RazaoSocial
                    },

                    c.DataCandidatura
                })
                .FirstOrDefaultAsync();

            if (candidatura == null)
                return NotFound("Candidatura não encontrada.");

            return Ok(candidatura);
        }


        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CandidaturaDto dto)
        {
            var estudante = await _context.Estudantes.FindAsync(dto.EstudanteId);
            if (estudante == null)
                return NotFound("Estudante não encontrado.");

            var vaga = await _context.Vagas
                .Include(v => v.Empresa)
                .FirstOrDefaultAsync(v => v.Id == dto.VagaId);

            if (vaga == null)
                return NotFound("Vaga não encontrada.");

            if (vaga.Empresa == null)
                return BadRequest("A vaga não está vinculada a nenhuma empresa.");

            var existe = await _context.Candidaturas
                .AnyAsync(c => c.EstudanteId == dto.EstudanteId && c.VagaId == dto.VagaId);

            if (existe)
                return BadRequest("O estudante já se candidatou para esta vaga.");

            var candidatura = new Candidatura
            {
                EstudanteId = dto.EstudanteId,
                VagaId = dto.VagaId,
                DataCandidatura = DateTime.Now
            };

            await _context.Candidaturas.AddAsync(candidatura);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Candidatura criada com sucesso!",
                estudante = estudante.Nome,
                vaga = vaga.Titulo,
                empresa = vaga.Empresa.RazaoSocial
            });
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CandidaturaDto dto)
        {
            var candidatura = await _context.Candidaturas
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidatura == null)
            {
                return NotFound("Candidatura não encontrada.");
            }

            var estudante = await _context.Estudantes
                .FirstOrDefaultAsync(e => e.Id == dto.EstudanteId);

            if (estudante == null)
            {
                return NotFound("Estudante não encontrado.");
            }

            var vaga = await _context.Vagas
                .FirstOrDefaultAsync(v => v.Id == dto.VagaId);

            if (vaga == null)
            {
                return NotFound("Vaga não encontrada.");
            }

            // Verificar duplicidade (caso altere o vínculo)
            var duplicada = await _context.Candidaturas
                .FirstOrDefaultAsync(c =>
                    c.Id != id &&
                    c.EstudanteId == dto.EstudanteId &&
                    c.VagaId == dto.VagaId
                );

            if (duplicada != null)
            {
                return BadRequest("Já existe uma candidatura desse estudante para essa vaga.");
            }

            candidatura.EstudanteId = dto.EstudanteId;
            candidatura.VagaId = dto.VagaId;
            candidatura.DataCandidatura = DateTime.Now;

            _context.Candidaturas.Update(candidatura);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Estudante = estudante.Nome,
                Vaga = vaga.Titulo,
                candidatura.DataCandidatura
            });
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var candidatura = await _context.Candidaturas.FindAsync(id);

            if (candidatura is null)
                return NotFound();

            _context.Candidaturas.Remove(candidatura);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

