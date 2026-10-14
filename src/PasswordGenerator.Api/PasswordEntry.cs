namespace PasswordGenerator.Api;

public sealed class PasswordEntry
{
    public Guid Id { get; set; }

    public required string Value { get; set; }
}