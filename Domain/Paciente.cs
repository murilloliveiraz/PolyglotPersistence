namespace PolyglotPersistence.Domain
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Prontuario Prontuario { get; set; }
    }
}