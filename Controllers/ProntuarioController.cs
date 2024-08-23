using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PolyglotPersistence.Domain;
using PolyglotPersistence.Services;

namespace PolyglotPersistence.Controllers
{
    [ApiController]
    [Route("api/prontuario")]
    public class ProntuarioController : ControllerBase
    {
        private readonly ProntuarioService _prontuarioService;

        public ProntuarioController(ProntuarioService prontuarioService)
        {
            _prontuarioService = prontuarioService;
        }

        [HttpPost("{id}/registros")]
        public async Task<IActionResult> AdicionarRegistro(int prontuarioId, [FromBody] NovoRegistroDTO dto)
        {
            await _prontuarioService.AdicionarRegistro(prontuarioId, dto.Tipo, BsonDocument.Parse(dto.Conteudo));
            return Ok();
        }

        [HttpGet("{pacienteId}")]
        public async Task<IActionResult> ObterProntuarioDoPaciente(int pacienteId)
        {
            var prontuarioCompleto = await _prontuarioService.ObterRegistrosDoPaciente(pacienteId);
            if (prontuarioCompleto == null)
                return NotFound();
            return Ok(prontuarioCompleto);
        }
    }
}