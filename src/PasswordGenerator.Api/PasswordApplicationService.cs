using Microsoft.EntityFrameworkCore;

namespace PasswordGenerator.Api;

public sealed class PasswordApplicationService(
    PasswordDbContext database,
    StrongPasswordGenerator passwordGenerator)
{
    public async Task<Guid> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var entry = new PasswordEntry
        {
            Id = Guid.NewGuid(),
            Value = passwordGenerator.Generate()
        };

        database.Passwords.Add(entry);
        await database.SaveChangesAsync(cancellationToken);

        return entry.Id;
    }

    public Task<string?> FindAsync(Guid id, CancellationToken cancellationToken = default) =>
        database.Passwords
            .Where(password => password.Id == id)
            .Select(password => password.Value)
            .SingleOrDefaultAsync(cancellationToken);
}