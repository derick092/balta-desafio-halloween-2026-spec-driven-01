using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordGenerator.Api;

namespace PasswordGenerator.Tests;

public sealed class PasswordEndpointsTests
{
    [Fact]
    public async Task PasswordEndpoints_FulfillGenerationAndLookupContracts()
    {
        var databasePath = Path.Combine(Path.GetTempPath(), $"passwords-{Guid.NewGuid():N}.db");

        try
        {
            Guid firstId;
            Guid secondId;
            string generatedPassword;

            using (var factory = CreateFactory(databasePath))
            using (var client = factory.CreateClient())
            {
                var firstGeneration = await client.PostAsync("/passwords", content: null);
                var secondGeneration = await client.PostAsync("/passwords", content: null);

                Assert.Equal(HttpStatusCode.Created, firstGeneration.StatusCode);
                Assert.Equal(HttpStatusCode.Created, secondGeneration.StatusCode);

                firstId = Assert.IsType<Guid>(await firstGeneration.Content.ReadFromJsonAsync<Guid>());
                secondId = Assert.IsType<Guid>(await secondGeneration.Content.ReadFromJsonAsync<Guid>());
                Assert.NotEqual(firstId, secondId);

                var firstLookup = await client.GetAsync($"/passwords/{firstId}");
                Assert.Equal(HttpStatusCode.OK, firstLookup.StatusCode);
                generatedPassword = Assert.IsType<string>(await firstLookup.Content.ReadFromJsonAsync<string>());

                var duplicateValueId = Guid.NewGuid();
                var anotherDuplicateValueId = Guid.NewGuid();
                await using (var scope = factory.Services.CreateAsyncScope())
                {
                    var database = scope.ServiceProvider.GetRequiredService<PasswordDbContext>();
                    database.Passwords.AddRange(
                        new PasswordEntry { Id = duplicateValueId, Value = generatedPassword },
                        new PasswordEntry { Id = anotherDuplicateValueId, Value = generatedPassword });
                    await database.SaveChangesAsync();
                }

                var duplicateValueLookup = await client.GetAsync($"/passwords/{duplicateValueId}");
                var anotherDuplicateValueLookup = await client.GetAsync($"/passwords/{anotherDuplicateValueId}");
                Assert.Equal(generatedPassword, await duplicateValueLookup.Content.ReadFromJsonAsync<string>());
                Assert.Equal(generatedPassword, await anotherDuplicateValueLookup.Content.ReadFromJsonAsync<string>());

                var malformedId = await client.GetAsync("/passwords/not-a-guid");
                Assert.Equal(HttpStatusCode.BadRequest, malformedId.StatusCode);
                Assert.Equal("application/problem+json", malformedId.Content.Headers.ContentType?.MediaType);

                var missingId = await client.GetAsync($"/passwords/{Guid.NewGuid()}");
                Assert.Equal(HttpStatusCode.NotFound, missingId.StatusCode);
                Assert.Equal("application/problem+json", missingId.Content.Headers.ContentType?.MediaType);
            }

            using (var factory = CreateFactory(databasePath))
            using (var client = factory.CreateClient())
            {
                var lookupAfterRestart = await client.GetAsync($"/passwords/{firstId}");

                Assert.Equal(HttpStatusCode.OK, lookupAfterRestart.StatusCode);
                Assert.Equal(generatedPassword, await lookupAfterRestart.Content.ReadFromJsonAsync<string>());
            }
        }
        finally
        {
            if (File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }
        }
    }

    private static WebApplicationFactory<Program> CreateFactory(string databasePath) =>
        new WebApplicationFactory<Program>().WithWebHostBuilder(webHostBuilder =>
            webHostBuilder.ConfigureAppConfiguration((_, configuration) =>
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Passwords"] = $"Data Source={databasePath}"
                })));
}