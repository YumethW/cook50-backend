using cook50_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace cook50_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly GeminiService _geminiService;

        public RecipeController(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        [HttpPost("get-recipes")]
        public async Task<IActionResult> GetRecipes([FromBody] IngredientRequest request)
        {
            var normalizedIngredients = request
                .Ingredients.Select(i => i.ToLower().Trim())
                .ToList();

            var suggestedRecipes = await _geminiService.GetRecipesFromAI(normalizedIngredients);

            if (!suggestedRecipes.Any())
            {
                return Ok(
                    new
                    {
                        recipes = new List<Recipe>
                        {
                            new Recipe
                            {
                                Title = "No recipes found",
                                Ingredients = new List<string>(),
                                Instructions = new List<string>(),
                            },
                        },
                    }
                );
            }

            return Ok(new { recipes = suggestedRecipes });
        }
    }

    public class IngredientRequest
    {
        public required List<string> Ingredients { get; set; }
    }
}

public class Recipe
{
    public required string Title { get; set; }
    public required List<string> Ingredients { get; set; }
    public required List<string> Instructions { get; set; }
}
