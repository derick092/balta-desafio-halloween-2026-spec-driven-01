using PasswordGenerator.Api;

namespace PasswordGenerator.Tests;

public sealed class StrongPasswordGeneratorTests
{
    [Fact]
    public void Generate_ReturnsPasswordWithRequiredComposition()
    {
        var generator = new StrongPasswordGenerator();

        for (var attempt = 0; attempt < 100; attempt++)
        {
            var password = generator.Generate();

            Assert.True(password.Length >= 16);
            Assert.Contains(password, char.IsUpper);
            Assert.Contains(password, char.IsLower);
            Assert.Contains(password, char.IsDigit);
            Assert.Contains(password, character => "!@#$%^&*()-_=+[]{};:,.?".Contains(character));
            Assert.DoesNotContain(password, char.IsWhiteSpace);
            Assert.All(password, character => Assert.True(char.IsAscii(character)));
        }
    }
}