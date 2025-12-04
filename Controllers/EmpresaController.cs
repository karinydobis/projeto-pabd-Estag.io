using ApiServico.DataContexts;
using ApiServico.Models.Dtos;
using ApiServico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiServico.Controllers
{
    [Route("/empresas")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpresaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarTodos(
            [FromQuery] string? search
        )
        {
            var query = _context.Empresas.AsQueryable();

            if (search is not null)
            {
                query = query.Where(x => x.RazaoSocial.Contains(search));
            }

            var empresas = await query.ToListAsync();
            return Ok(empresas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var empresa = await _context.Empresas
                .Include(e => e.Vagas)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (empresa is null)
                return NotFound();

            return Ok(empresa);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] EmpresaDto dto)
        {
            var empresa = new Empresa()
            {
                RazaoSocial = dto.RazaoSocial,
                CNPJ = dto.CNPJ,
                AreaAtuacao = dto.AreaAtuacao,
                Email = dto.Email,
                LogoUrl = dto.LogoUrl
            };

            await _context.Empresas.AddAsync(empresa);
            await _context.SaveChangesAsync();

            return Created("", empresa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] EmpresaDto dto)
        {
            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa is null)
                return NotFound();

            empresa.RazaoSocial = dto.RazaoSocial;
            empresa.CNPJ = dto.CNPJ;
            empresa.AreaAtuacao = dto.AreaAtuacao;
            empresa.Email = dto.Email;
            empresa.LogoUrl = dto.LogoUrl;

            _context.Empresas.Update(empresa);
            await _context.SaveChangesAsync();

            return Ok(empresa);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var empresa = await _context.Empresas
                .Include(e => e.Vagas)
                .ThenInclude(v => v.Candidaturas)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (empresa == null)
                return NotFound("Empresa não encontrada.");

            var candidaturas = empresa.Vagas.SelectMany(v => v.Candidaturas).ToList();

            if (candidaturas.Any())
                _context.Candidaturas.RemoveRange(candidaturas);

            
            if (empresa.Vagas.Any())
                _context.Vagas.RemoveRange(empresa.Vagas);

           
            _context.Empresas.Remove(empresa);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Empresa, vagas e candidaturas removidas com sucesso."
            });
        }

    }
}
