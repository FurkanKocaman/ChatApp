using ChatApp.Server.Domain.Users;

namespace ChatApp.Server.Domain.Tokens;
public sealed class Token
{
    public Token()
    {
        Id= Guid.CreateVersion7();
        Data = GenerateRandomToken();
        CreationDate = DateTimeOffset.Now;
    }
    public Guid Id { get; set; }
    public string Data { get; set; } = default!;
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public Guid CreatorId { get; set; }
    public AppUser? Creator { get; set; }
    public Guid ServerId { get; set; }
    public Domain.Servers.Server? Server { get; set; }
    public bool IsUsed { get; set; }
    public TokenType TokenType { get; set; } = TokenType.Invitation;
    public int MaxUsageCount { get; set; }
    public int CurrentUsageCount { get; set; }

    private static string GenerateRandomToken(int lenght = 32)
    {
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();

        var tokenData = new byte[lenght];
        rng.GetBytes(tokenData);
        return Convert.ToBase64String(tokenData)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }
}

public enum TokenType
{
    Invitation
}