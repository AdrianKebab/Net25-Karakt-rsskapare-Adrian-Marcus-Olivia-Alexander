using OpenAI;
using System.ClientModel;
using DotNetEnv;
using OpenAI.Chat;

#pragma warning disable OPENAI001

Env.Load();

const string deploymentName = "gpt-5.4-mini";
var endpoint = Environment.GetEnvironmentVariable("ENDPOINT") ?? throw new Exception("fel endpoint");
var apiKey = Environment.GetEnvironmentVariable("KEY") ?? throw new Exception("fel key");

var client = new ChatClient(
    deploymentName,
    new ApiKeyCredential(apiKey),
    new OpenAIClientOptions
    {
        Endpoint = new Uri(endpoint)
    });

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost(
    "/api/character",
    async (CharacterRequest request) =>
{

    if (string.IsNullOrWhiteSpace(request.Description))
    {
        return Results.BadRequest(new { erro = "Ingen Text angiven." });
    }

var completion = await client.CompleteChatAsync(
[
    new SystemChatMessage(
        """
        Du är en kreativ karaktärsskapare för rollspel.

        Skapa en komplett och sammanhängande rollspelskaraktär
        utifrån användarens beskrivning. Fyll själv i rimliga
        detaljer som användaren inte har angett.

        Regler:
        - Skriv allt innehåll på svenska.
        - Ge karaktären 3 till 6 personlighetsdrag.
        - Ge karaktären 3 till 8 utrustningsföremål.
        - Ge karaktären 3 till 6 färdigheter.
        - Bakgrundshistorien ska vara 2 till 4 korta stycken.
        - Alla properties måste alltid finnas med.
        - Svara endast med giltig JSON.
        - Använd inte Markdown eller kodblock.
        - Skriv ingen text före eller efter JSON-objektet.

        Svaret ska följa exakt denna struktur:

        {
          "name": "Karaktärens namn",
          "species": "Art eller folkslag",
          "profession": "Klass, yrke eller roll",
          "personalityTraits": [
            "Personlighetsdrag"
          ],
          "equipment": [
            "Utrustningsföremål"
          ],
          "skills": [
            "Färdighet"
          ],
          "backstory": "Karaktärens bakgrundshistoria"
        }
        """
    ),
    new UserChatMessage(request.Description)
]);

    string refinedDescription = completion.Value.Content[0].Text;

    return Results.Ok(new CharacterResponse(refinedDescription));


});

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();

public record CharacterRequest(string Description);

public record CharacterResponse(string RefinedDescription);