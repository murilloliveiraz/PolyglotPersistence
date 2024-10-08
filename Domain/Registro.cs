using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PolyglotPersistence.Domain
{
    public class Registro
    {
    [BsonId]
    public ObjectId Id { get; set; }
    public int ProntuarioId { get; set; }
    public string Tipo { get; set; }
    public DateTime Data { get; set; }
    public BsonDocument Conteudo { get; set; }
    }
}