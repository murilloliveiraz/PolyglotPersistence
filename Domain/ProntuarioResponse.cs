namespace PolyglotPersistence.Domain
{
    public class ProntuarioResponse
    {
        public Prontuario Prontuario { get; set; }
        public ICollection<Registro> Registros { get; set; }
    }
}