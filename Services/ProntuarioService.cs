using Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using PolyglotPersistence.Context;
using PolyglotPersistence.Domain;

namespace PolyglotPersistence.Services
{
    public class ProntuarioService
    {
        private readonly ApplicationContext _context;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<Registro> _registros;

        public ProntuarioService(ApplicationContext context, IMongoClient mongoClient, IOptions<MongoDatabaseConfig> mongoConfig)
        {
            _context = context;
            _mongoClient = mongoClient;
            _mongoDatabase = _mongoClient.GetDatabase(mongoConfig.Value.DatabaseName);
            _registros = _mongoDatabase.GetCollection<Registro>("Registros");
        }

        public async Task<Prontuario> CriarProntuario(int pacienteId)
        {
            var prontuario = new Prontuario
            {
                PacienteId = pacienteId,
                DataDeCriacao = DateTime.UtcNow
            };

            _context.Prontuarios.Add(prontuario);
            await _context.SaveChangesAsync();

            var conteudo = new NovoRegistroDTO
            {
                Tipo = "Cria��o",
                Conteudo = new
                {
                    Observa��o = "Prontu�rio criado automaticamente ao registrar o paciente.",
                }
            };

            await AdicionarRegistro(prontuario.Id, conteudo);
            return prontuario;
        }

        public async Task AdicionarRegistro(int prontuarioId, NovoRegistroDTO dto)
        {
            var registro = new Registro
            {
                ProntuarioId = prontuarioId,
                Tipo = dto.Tipo,
                Data = DateTime.UtcNow,
                Conteudo = dto.Conteudo.ToBsonDocument(),
            };

            await _registros.InsertOneAsync(registro);
        }

        public async Task<ICollection<RegistroDTO>> ObterRegistrosDoPaciente(int pacienteId)
        {
            var paciente = await _context.Pacientes
                .Include(p => p.Prontuario)
                .FirstOrDefaultAsync(p => p.Id == pacienteId);

            if (paciente?.Prontuario == null)
                return new List<RegistroDTO>();

            var registros = await _registros.Find(r => r.ProntuarioId == paciente.Prontuario.Id).ToListAsync();

            return registros.Select(r => new RegistroDTO
            {
                Id = r.Id.ToString(),
                ProntuarioId = r.ProntuarioId,
                Tipo = r.Tipo,
                Data = r.Data,
                Conteudo = BsonTypeMapper.MapToDotNetValue(r.Conteudo)
            }).ToList();
        }
    }
}