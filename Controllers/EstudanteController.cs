using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiServico.DataContexts;
using ApiServico.Models;
using ApiServico.Models.Dtos;

namespace ApiServico.Controllers
{
    [Route("/estudantes")]
    [ApiController]
    public class EstudanteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstudanteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarTodos(
            [FromQuery] string? search
        )
        {
            var query = _context.Estudantes.AsQueryable();

            if (search is not null)
            {
                query = query.Where(x => x.Nome.Contains(search));
            }

            var estudantes = await query.ToListAsync();
            return Ok(estudantes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var estudante = await _context.Estudantes
                .Include(e => e.Candidaturas)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (estudante is null)
                return NotFound();

            return Ok(estudante);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] EstudanteDto dto)
        {
            var estudante = new Estudante()
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Curso = dto.Curso,
                GitHub = dto.GitHub,
                Portfolio = dto.Portfolio,
                Stacks = dto.Stacks
            };

            await _context.Estudantes.AddAsync(estudante);
            await _context.SaveChangesAsync();

            return Created("", estudante);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] EstudanteDto dto)
        {
            var estudante = await _context.Estudantes.FirstOrDefaultAsync(e => e.Id == id);

            if (estudante == null)
                return NotFound("Estudante não encontrado.");

            // Verifica se já existe outro estudante com esse e-mail
            var emailEmUso = await _context.Estudantes
                .AnyAsync(e => e.Email == dto.Email && e.Id != id);

            if (emailEmUso)
                return BadRequest("Este e-mail já está sendo usado por outro estudante.");

            estudante.Nome = dto.Nome;
            estudante.Email = dto.Email;
            estudante.Curso = dto.Curso;
            estudante.Stacks = dto.Stacks;
            estudante.GitHub = dto.GitHub;
            estudante.Portfolio = dto.Portfolio;

            _context.Estudantes.Update(estudante);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Estudante atualizado com sucesso!",
                estudante = estudante.Nome
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var estudante = await _context.Estudantes
                .Include(e => e.Candidaturas)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (estudante == null)
                return NotFound("Estudante não encontrado.");

           
            if (estudante.Candidaturas.Any())
            {
                _context.Candidaturas.RemoveRange(estudante.Candidaturas);
            }

           
            _context.Estudantes.Remove(estudante);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Estudante e suas candidaturas foram removidos com sucesso."
            });
        }

    }
}
