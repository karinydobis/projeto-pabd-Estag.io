using ApiServico.DataContexts;
using ApiServico.Models.Dtos;
using ApiServico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiServico.Controllers
{
    [Route("/vagas")]
    [ApiController]
    public class VagaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VagaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarTodos(
            [FromQuery] string? search,
            [FromQuery] string? empresa
        )
        {
            var query = _context.Vagas
                .Include(v => v.Empresa)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(v => v.Titulo.Contains(search));
            }

            if (!string.IsNullOrEmpty(empresa))
            {
                query = query.Where(v => v.Empresa.RazaoSocial.Contains(empresa));
            }

            var vagas = await query
                .Select(v => new
                {
                    Empresa = new
                    {
                        v.Empresa.RazaoSocial,
                        v.Empresa.Email,
                        v.Empresa.AreaAtuacao
                    },
                    Id = v.Id,
                    Titulo = v.Titulo,
                    Descricao = v.Descricao,
                    Requisitos = v.Requisitos,
                    Modalidade = v.Modalidade,
                    Beneficios = v.Beneficios,
                    DataCriacao = v.DataCriacao
                })
                .ToListAsync();

            return Ok(vagas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var vaga = await _context.Vagas
                .Include(v => v.Empresa)
                .Where(v => v.Id == id)
                .Select(v => new
                {
                    Empresa = new
                    {
                        v.Empresa.RazaoSocial,
                        v.Empresa.Email,
                        v.Empresa.AreaAtuacao
                    },
                    Titulo = v.Titulo,
                    Descricao = v.Descricao,
                    Requisitos = v.Requisitos,
                    Modalidade = v.Modalidade,
                    Beneficios = v.Beneficios,
                    DataCriacao = v.DataCriacao
                })
                .FirstOrDefaultAsync();

            if (vaga is null)
                return NotFound();

            return Ok(vaga);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] VagaDto dto)
        {
            var empresa = await _context.Empresas.FindAsync(dto.EmpresaId);

            if (empresa is null)
                return NotFound("Empresa não encontrada.");

            var vaga = new Vaga()
            {
                EmpresaId = dto.EmpresaId,
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Requisitos = dto.Requisitos,
                Modalidade = dto.Modalidade,
                Beneficios = dto.Beneficios
            };

            await _context.Vagas.AddAsync(vaga);
            await _context.SaveChangesAsync();

            return Created("", vaga);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] VagaDto dto)
        {
            var vaga = await _context.Vagas
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vaga == null)
            {
                return NotFound("Vaga não encontrada.");
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(e => e.Id == dto.EmpresaId);

            if (empresa == null)
            {
                return NotFound("Empresa informada não encontrada.");
            }

            vaga.Titulo = dto.Titulo;
            vaga.Descricao = dto.Descricao;
            vaga.Requisitos = dto.Requisitos;
            vaga.Modalidade = dto.Modalidade;
            vaga.Beneficios = dto.Beneficios;
            vaga.EmpresaId = dto.EmpresaId;

            _context.Vagas.Update(vaga);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                vaga.Titulo,
                vaga.Descricao,
                vaga.Requisitos,
                vaga.Modalidade,
                vaga.Beneficios,
                Empresa = empresa.RazaoSocial
            });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var vaga = await _context.Vagas
                .Include(v => v.Candidaturas)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vaga == null)
                return NotFound("Vaga não encontrada.");

            // Remove as candidaturas ligadas a essa vaga
            if (vaga.Candidaturas.Any())
            {
                _context.Candidaturas.RemoveRange(vaga.Candidaturas);
            }

            _context.Vagas.Remove(vaga);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Vaga e suas candidaturas foram removidas com sucesso."
            });
        }
    }
}
