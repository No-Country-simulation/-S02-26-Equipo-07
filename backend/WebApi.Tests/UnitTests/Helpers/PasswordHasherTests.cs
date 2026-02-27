using WebApi.Helpers;
using Xunit;
using FluentAssertions;

namespace WebApi.Tests.UnitTests.Helpers;

public class PasswordHasherTests
{
    [Fact]
    public void HashPassword_ConPasswordPlano_RetornaHash()
    {
        var password = "MySecurePassword123";

        var hash = PasswordHasher.HashPassword(password);

        hash.Should().NotBeNullOrEmpty();
        hash.Should().NotBe(password);
        hash.Length.Should().BeGreaterThan(20);
    }

    [Fact]
    public void VerifyPassword_ConPasswordCorrecto_RetornaTrue()
    {
        var password = "MySecurePassword123";
        var hash = PasswordHasher.HashPassword(password);

        var result = PasswordHasher.VerifyPassword(password, hash);

        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_ConPasswordIncorrecto_RetornaFalse()
    {
        var correctPassword = "MySecurePassword123";
        var wrongPassword = "WrongPassword";
        var hash = PasswordHasher.HashPassword(correctPassword);

        var result = PasswordHasher.VerifyPassword(wrongPassword, hash);

        result.Should().BeFalse();
    }

    [Fact]
    public void HashPassword_DiferentesLlamadas_RetornaHashesDiferentes()
    {
        var password = "MySecurePassword123";

        var hash1 = PasswordHasher.HashPassword(password);
        var hash2 = PasswordHasher.HashPassword(password);

        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void HashPassword_ConPasswordVacio_RetornaHash()
    {
        var password = "";

        var hash = PasswordHasher.HashPassword(password);

        hash.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void VerifyPassword_ConPasswordVacio_RetornaTrue()
    {
        var password = "";
        var hash = PasswordHasher.HashPassword(password);

        var result = PasswordHasher.VerifyPassword(password, hash);

        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_ConPasswordVacioYHashDiferente_RetornaFalse()
    {
        var password = "";
        var hash = PasswordHasher.HashPassword("otherpassword");

        var result = PasswordHasher.VerifyPassword(password, hash);

        result.Should().BeFalse();
    }
}
