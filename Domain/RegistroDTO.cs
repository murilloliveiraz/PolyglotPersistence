namespace PolyglotPersistence.Domain
{
    public class RegistroDTO
    {
        public string Id { get; set; }
        public int ProntuarioId { get; set; }
        public string Tipo { get; set; }
        public DateTime Data { get; set; }
        public object Conteudo { get; set; }
    }
}
