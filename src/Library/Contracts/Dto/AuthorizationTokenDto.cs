using MongoDB.Bson.Serialization.Attributes;

namespace Library.Contracts.Dto;

public class AuthorizationTokenDto
{
    [BsonId]
    public required Guid UserId { get; init; }

    public required string Token { get; init; }
    public required DateTime? ExpirationTime { get; init; }
}