using Receptek.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Receptek.Services
{
    public class IngredientService
    {
        private readonly IIngredientRepository ingredientRepo;

        public IngredientService(IIngredientRepository ingredientRepo)
        {
            this.ingredientRepo = ingredientRepo;
        }

        public void Add(Ingredient ingredient) => ingredientRepo.Add(ingredient);

        public bool Remove(Ingredient ingredient) => ingredientRepo.Remove(ingredient);

        public Ingredient? FindByName(string ingredientName) => ingredientRepo.FindByName(ingredientName);

        public int GetId(Ingredient ingredient) => ingredientRepo.GetId(ingredient);

        public Ingredient? GetById(int id) => ingredientRepo.GetById(id);

        public List<Ingredient> GetValues() => ingredientRepo.GetValues();

        public bool IngredientNameExists(string Name, int ingredientId = -1)
        {
            Dictionary<int, Ingredient> ingredients = ingredientRepo.GetDictionary();

            foreach(var i in ingredients)
            {
                if (i.Value.Name == Name && ingredientId != i.Key)
                    return true;
            }

            return false;

        }

        public bool RemoveFromRecipe(Recipe recipe, RecipeIngredient ingredient)
        {
            if (recipe.Ingredients.Count == 1)
                return false;
            return recipe.Ingredients.Remove(ingredient);
        }

        public string GetNames()
        {
            string ingredientString = "";

            List<Ingredient> ingredientList = ingredientRepo.GetValues();

            if (ingredientList.Count == 0)
            {
                return "Nincs elmentett összetevő.\n";
            }

            int counter = 1;

            foreach (var recipe in ingredientList)
            {
                ingredientString += $"{counter} - {recipe.Name}\n";
                counter++;
            }

            return ingredientString;
        }

        public string Format(Ingredient ingredient)
        {
            string result = "---------------------------------\n";

            result += $"\nNév: {ingredient.Name}\n\n";

            if (ingredient.OnePieceWeigh > 0)
            {
                int caloriePerPiece = (ingredient.CalorieIn100g * ingredient.OnePieceWeigh) / 100;
                int proteinPerPiece = (ingredient.ProteinIn100g * ingredient.OnePieceWeigh) / 100;

                result += $"Tömeg: {ingredient.OnePieceWeigh} g/db\n";
                result += $"Kalória: {caloriePerPiece} kcal/db\n";
                result += $"Fehérje: {proteinPerPiece} g/db\n\n";
            }

            result += $"Kalória: {ingredient.CalorieIn100g} kcal/100g\n";
            result += $"Fehérje: {ingredient.ProteinIn100g} g/100g\n";

            result += "\n---------------------------------\n";

            return result;
        }

        public string FormatAll()
        {
            List<Ingredient> ingredientList = ingredientRepo.GetValues();

            ingredientList = ingredientList.OrderBy(r => r.Name).ToList();

            string ingredientListString = "";

            if (ingredientList.Count == 0)
            {
                return "Nincs elmentett összetevő.\n";
            }

            int counter = 1;

            foreach (var ingredient in ingredientList)
            {
                ingredientListString += $"\n{counter}. összetevő:\n";
                ingredientListString += Format(ingredient);
                counter++;
            }

            return ingredientListString;
        }

        public void Save() => ingredientRepo.Save();

    }
}
