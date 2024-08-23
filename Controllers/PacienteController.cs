using Context;
using Microsoft.AspNetCore.Mvc;
using PolyglotPersistence.Domain;
using PolyglotPersistence.Services;

namespace PolyglotPersistence.Controllers
{
    [ApiController]
    [Route("api/pacientes")]
    public class PacienteController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ProntuarioService _prontuarioService;
        public PacienteController(ApplicationContext context, ProntuarioService prontuarioService)
        {
            _context = context;
            _prontuarioService = prontuarioService;
        }

        [HttpPost]
        public async Task<Paciente> CreatePaciente(CreatePacienteModel model)
        {
            
            var paciente = new Paciente
            {
                Nome = model.Nome,
            };
            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();
            var prontuario = await _prontuarioService.CriarProntuario(paciente.Id);            
            return paciente;
        }

    }
}