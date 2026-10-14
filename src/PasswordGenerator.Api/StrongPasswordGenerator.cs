using System.Security.Cryptography;

namespace PasswordGenerator.Api;

public sealed class StrongPasswordGenerator
{
    private const int PasswordLength = 20;
    private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";
    private const string SpecialCharacters = "!@#$%^&*()-_=+[]{};:,.?";
    private const string AllCharacters = UppercaseLetters + LowercaseLetters + Digits + SpecialCharacters;

    public string Generate()
    {
        var characters = new char[PasswordLength];
        characters[0] = Pick(UppercaseLetters);
        characters[1] = Pick(LowercaseLetters);
        characters[2] = Pick(Digits);
        characters[3] = Pick(SpecialCharacters);

        for (var index = 4; index < characters.Length; index++)
        {
            characters[index] = Pick(AllCharacters);
        }

        for (var index = characters.Length - 1; index > 0; index--)
        {
            var swapIndex = RandomNumberGenerator.GetInt32(index + 1);
            (characters[index], characters[swapIndex]) = (characters[swapIndex], characters[index]);
        }

        return new string(characters);
    }

    private static char Pick(string characters) => characters[RandomNumberGenerator.GetInt32(characters.Length)];
}