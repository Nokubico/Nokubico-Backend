using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Nokubico.Application.DTOs;
using Nokubico.Application.Mapping;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Enums;

// Run with: dotnet run --project tests/Nokubico.Application.SecurityChecks
var checks = new (string Name, Action Run)[]
{
    ("User output exposes only the approved contract", () =>
    {
        var user = new User("user@example.com", "Nome", UserRole.Admin);
        user.SetBio("Bio");
        user.SetImage("/profile.png");
        user.SetLocation("Luanda");
        user.SetProfession("Dev");
        user.VerifyEmail();
        var account = new Account("user@example.com", "email", user);
        account.SetPassword("SECRET_HASH");
        user.AddAccount(account);

        var dto = UserMapper.ToDto(user);
        using var json = JsonDocument.Parse(JsonSerializer.Serialize(dto));
        AssertFields(json, "Id", "Name", "Email", "Image", "Bio", "Location",
            "Profession", "Role", "EmailVerified", "CreatedAt");
        Assert(!json.RootElement.ToString().Contains("SECRET_HASH"), "Credential leaked.");
        Assert(dto.Id == user.Id && dto.CreatedAt == user.CreatedAt && dto.Name == user.Name
            && dto.Email == user.Email && dto.Image == user.Image && dto.Bio == user.Bio
            && dto.Location == user.Location && dto.Profession == user.Profession
            && dto.Role == UserRole.Admin && dto.EmailVerified, "Public profile lost data.");
    }),
    ("Profile update cannot bind or change identity and privileges", () =>
    {
        const string payload = """
            {"Name":"Novo","Bio":"Bio","Location":"Luanda","Profession":"Dev",
             "Image":"/new.png","Email":"attacker@example.com","Role":1,
             "EmailVerified":false,"Password":"attacker","Accounts":[],"Sessions":[]}
            """;
        var dto = JsonSerializer.Deserialize<UserUpdateDTO>(payload)!;
        var user = new User("original@example.com", "Antes", UserRole.User);
        user.VerifyEmail();
        var id = user.Id;
        var createdAt = user.CreatedAt;
        var accounts = user.Accounts;
        var sessions = user.Sessions;
        var result = UserMapper.Apply(dto, user);
        Assert(ReferenceEquals(result, user), "Update replaced the entity.");
        Assert(user.Email == "original@example.com" && user.Role == UserRole.User
            && user.EmailVerified && user.Id == id && user.CreatedAt == createdAt
            && ReferenceEquals(accounts, user.Accounts) && ReferenceEquals(sessions, user.Sessions),
            "Update changed a protected field.");
        Assert(user.Name == "Novo" && user.Bio == "Bio" && user.Location == "Luanda"
            && user.Profession == "Dev" && user.Image == "/new.png", "Profile update incomplete.");
        using var json = JsonDocument.Parse(JsonSerializer.Serialize(dto));
        AssertFields(json, "Name", "Bio", "Location", "Profession", "Image");
        UserMapper.Apply(new UserUpdateDTO { Name = "Novo" }, user);
        Assert(user.Bio == null && user.Location == null && user.Profession == null
            && user.Image == null, "Full replacement must clear omitted optional fields.");
    }),
    ("Registration cannot select an administrator role or verified email", () =>
    {
        var dto = JsonSerializer.Deserialize<RegisterDTO>("""
            {"Name":"Nome","Email":"user@example.com","Password":"SECRET",
             "Profession":"Dev","Role":1,"EmailVerified":true}
            """)!;
        var user = UserMapper.ToEntity(dto);
        Assert(user.Role == UserRole.User && !user.EmailVerified && user.Accounts.Count == 0
            && user.Sessions.Count == 0, "Registration set privileged or credential state.");
        Assert(user.Name == dto.Name && user.Email == dto.Email && user.Profession == "Dev",
            "Registration lost allowed fields.");
        user.SetRole(UserRole.Admin);
        user.VerifyEmail();
        var originalEmail = user.Email;
        dto.Email = "different@example.com";
        dto.Name = "Novo";
        UserMapper.Apply(dto, user);
        Assert(user.Email == originalEmail && user.Role == UserRole.Admin && user.EmailVerified
            && user.Name == "Novo", "Registration application changed protected state.");
    }),
    ("Invalid registration and login inputs return Portuguese validation errors", () =>
    {
        AssertErrors(new RegisterDTO { Name = " ", Email = "invalid", Password = "12345" },
            "O nome é obrigatório.", "Email inválido.", "A senha deve ter pelo menos 6 caracteres.");
        AssertErrors(new LoginDTO { Email = "invalid", Password = "12345" },
            "Email inválido.", "A senha deve ter pelo menos 6 caracteres.");
        AssertErrors(new RegisterDTO(), "O nome é obrigatório.", "O email é obrigatório.",
            "A senha é obrigatória.");
        AssertErrors(new LoginDTO(), "O email é obrigatório.", "A senha é obrigatória.");
        AssertErrors(new UserUpdateDTO { Name = " " }, "O nome é obrigatório.");
    }),
    ("Valid inputs accept six-character passwords and optional profile fields", () =>
    {
        foreach (var dto in new object[]
        {
            new RegisterDTO { Name = "Nome", Email = "user@example.com", Password = "123456" },
            new LoginDTO { Email = "user@example.com", Password = "123456" },
            new UserUpdateDTO { Name = "Nome" }
        })
            Assert(Validate(dto).Count == 0, "Valid DTO rejected.");
    }),
    ("Authentication response exposes only token, email and role", () =>
    {
        using var json = JsonDocument.Parse(JsonSerializer.Serialize(new UserTokenDTO
        {
            Token = "login-token", Email = "user@example.com", Role = UserRole.User
        }));
        AssertFields(json, "Token", "Email", "Role");
    })
};

var failures = 0;
foreach (var (name, run) in checks)
{
    try { run(); Console.WriteLine($"PASS: {name}"); }
    catch (Exception error) { failures++; Console.Error.WriteLine($"FAIL: {name}: {error.Message}"); }
}
Console.WriteLine($"{checks.Length - failures}/{checks.Length} security checks passed.");
return failures == 0 ? 0 : 1;

static List<ValidationResult> Validate(object dto)
{
    var results = new List<ValidationResult>();
    Validator.TryValidateObject(dto, new ValidationContext(dto), results, validateAllProperties: true);
    return results;
}

static void AssertErrors(object dto, params string[] expected)
{
    var actual = Validate(dto).Select(result => result.ErrorMessage).ToHashSet();
    Assert(actual.SetEquals(expected), $"Unexpected validation messages: {string.Join("; ", actual)}");
}

static void AssertFields(JsonDocument json, params string[] expected)
{
    var actual = json.RootElement.EnumerateObject().Select(property => property.Name).ToHashSet();
    Assert(actual.SetEquals(expected), $"Unexpected serialized fields: {string.Join(", ", actual)}");
}

static void Assert(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}
