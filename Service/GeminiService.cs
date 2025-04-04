using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace cook50_backend.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;
        private readonly ILogger<GeminiService> _logger;

        public GeminiService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<GeminiService> logger
        )
        {
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"];
            _logger = logger;
        }

        private List<Recipe> ParseRecipes(string text)
        {
            _logger.LogInformation($"Parsing recipes text");
            _logger.LogDebug($"Raw recipe text: {text}");

            List<Recipe> recipes = new List<Recipe>();

            var recipeBlocks = Regex.Split(
                text,
                @"(?:\r?\n){2,}(?=RECIPE #\d+:|[A-Z][a-z]+(?:\s+[A-Z][a-z]+)*(?:\s+[A-Za-z-]+)*\r?\n)"
            );

            foreach (
                var recipeBlock in recipeBlocks.Where(block => !string.IsNullOrWhiteSpace(block))
            )
            {
                var recipe = ParseSingleRecipe(recipeBlock.Trim());
                if (recipe != null)
                {
                    recipes.Add(recipe);
                }
            }

            _logger.LogInformation($"Successfully parsed {recipes.Count} recipes");

            if (recipes.Count == 0)
            {
                var singleRecipe = ParseSingleRecipe(text);
                if (singleRecipe != null)
                {
                    recipes.Add(singleRecipe);
                }
            }

            return recipes;
        }

        private Recipe? ParseSingleRecipe(string text)
        {
            var lines = text.Split('\n')
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .ToList();

            if (lines.Count == 0)
                return null;

            string title = "Untitled Recipe";
            List<string> ingredients = new();
            List<string> instructions = new();

            bool isIngredientSection = false;
            bool isInstructionSection = false;

            if (Regex.IsMatch(lines[0], @"^RECIPE #\d+:"))
            {
                title = Regex.Replace(lines[0], @"^RECIPE #\d+:\s*", "");
            }
            else
            {
                title = lines[0];
            }

            foreach (var line in lines.Skip(1))
            {
                if (line.StartsWith("##") || line.StartsWith("Title:"))
                {
                    title = line.Replace("##", "").Replace("Title:", "").Trim();
                }
                else if (
                    line.Equals("Ingredients:", StringComparison.OrdinalIgnoreCase)
                    || line.Equals("**Ingredients:**", StringComparison.OrdinalIgnoreCase)
                )
                {
                    isIngredientSection = true;
                    isInstructionSection = false;
                    continue;
                }
                else if (
                    line.Equals("Instructions:", StringComparison.OrdinalIgnoreCase)
                    || line.Equals("**Instructions:**", StringComparison.OrdinalIgnoreCase)
                    || line.Equals("Directions:", StringComparison.OrdinalIgnoreCase)
                    || line.Equals("**Directions:**", StringComparison.OrdinalIgnoreCase)
                    || line.Equals("Steps:", StringComparison.OrdinalIgnoreCase)
                    || line.Equals("**Steps:**", StringComparison.OrdinalIgnoreCase)
                )
                {
                    isIngredientSection = false;
                    isInstructionSection = true;
                    continue;
                }
                else if (isIngredientSection)
                {
                    string cleanIngredient = Regex.Replace(line, @"^[-*•]\s*", "");
                    ingredients.Add(cleanIngredient);
                }
                else if (isInstructionSection)
                {
                    string cleanInstruction = Regex.Replace(line, @"^\d+[\.\)]\s*", "");
                    instructions.Add(cleanInstruction);
                }
            }

            _logger.LogDebug(
                $"Parsed recipe: '{title}' with {ingredients.Count} ingredients and {instructions.Count} instructions"
            );

            return new Recipe
            {
                Title = title,
                Ingredients = ingredients,
                Instructions = instructions,
            };
        }

        public async Task<List<Recipe>> GetRecipesFromAI(List<string> ingredients)
        {
            int numRecipes = 3;

            var prompt =
                $@"Generate {numRecipes} different recipes using some or all of these ingredients: {string.Join(", ", ingredients)}. 

Format each recipe as follows:

RECIPE #1: Recipe Title

Ingredients:
[Ingredient list with measurements, one per line]

Instructions:
[Step-by-step numbered instructions]

RECIPE #2: [Recipe Title]

And so on. Make each recipe distinct and use different cooking methods if possible. Do not add any introductory section to your answer";

            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = prompt } } } },
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 2048,
                    topP = 0.95,
                },
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}"
            )
            {
                Content = jsonContent,
            };

            try
            {
                var response = await _httpClient.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Gemini API Response: {response.StatusCode}");
                }
                else
                {
                    _logger.LogError($"Gemini API Error: {response.StatusCode} - {responseBody}");
                    return CreateErrorRecipe("Error fetching recipes");
                }

                var jsonResponse = JsonDocument.Parse(responseBody);
                var recipeText = jsonResponse
                    .RootElement.GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                _logger.LogInformation($"Successfully retrieved recipes from Gemini");

                var recipes = ParseRecipes(recipeText ?? string.Empty);

                if (recipes.Count == 0)
                {
                    return CreateErrorRecipe("Could not parse recipes from AI response");
                }

                return recipes;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling Gemini API: {ex.Message}");
                return CreateErrorRecipe($"Error: {ex.Message}");
            }
        }

        private List<Recipe> CreateErrorRecipe(string errorMessage)
        {
            return new List<Recipe>
            {
                new Recipe
                {
                    Title = errorMessage,
                    Ingredients = new List<string>(),
                    Instructions = new List<string>(),
                },
            };
        }
    }

    public class RecipeResponse
    {
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
