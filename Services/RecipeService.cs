using Receptek.Repositories.Interfaces;
using RecipesWinUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Receptek.Services
{
    public class RecipeService
    {
        private readonly IRecipeRepository recipeRepo;
        private readonly IIngredientRepository ingredientRepo;

        public RecipeService(IRecipeRepository recipeRepo, IIngredientRepository ingredientRepo)
        {
            this.recipeRepo = recipeRepo;
            this.ingredientRepo = ingredientRepo;
        }

        public void Add(Recipe recipe) => recipeRepo.Add(recipe);

        public bool Remove(Recipe recipe) => recipeRepo.Remove(recipe);

        public List<Recipe> GetValues() => recipeRepo.GetValues();

        public Recipe? FindByName(string recipeName) => recipeRepo.FindByName(recipeName);

        public RecipeIngredient? FindInRecipe(Recipe recipe, Ingredient ingredient)
        {
            int ingredientId = ingredientRepo.GetId(ingredient);
            return recipe.Ingredients.Find(x => x.ID == ingredientId);
        }

        public bool RecipeNameExists(string Name, int recipeId = -1)
        {
            Dictionary<int, Recipe> recipes = recipeRepo.GetDictionary();

            foreach (var r in recipes)
            {
                if (r.Value.Name == Name && recipeId != r.Key)
                    return true;
            }

            return false;

        }

        public void DeleteIdInRecipes(int id)
        {
            var recipes = recipeRepo.GetValues();

            foreach (var recipe in recipes)
            {
                recipe.Ingredients.RemoveAll(ri => ri.ID == id);
            }
        }

        public int GetRecipeCalorieOrProtein(Recipe recipe, bool isCalorie)
        {
            int sum = 0;
            foreach (var ingredient in recipe.Ingredients)
            {
                int ingredientId = ingredient.ID;
                Ingredient? foundIngredient = ingredientRepo.GetById(ingredientId);
                if (foundIngredient == null)
                {
                    throw new Exception("A ReceptIngredient id-je nincs benne az Ingredientek közt.");
                }

                bool isUnitPiece = ingredient.IsUnitPiece;
                int amount = ingredient.Amount;
                int amountIn100g = isCalorie ? foundIngredient.CalorieIn100g : foundIngredient.ProteinIn100g;
                int onePieceWeigh = foundIngredient.OnePieceWeigh;

                int calorieVal = isUnitPiece ? onePieceWeigh * amountIn100g / 100 : amountIn100g * amount / 100 ;

                sum += calorieVal;
            }

            return sum;
        } 

        public string GetNames()
        {
            string recipeString = "";

            List<Recipe> recipeList = recipeRepo.GetValues();

            if (recipeList.Count == 0)
            {
                return "Nincs elmentett recept.\n";
            }

            int counter = 1;

            foreach (var recipe in recipeList)
            {
                recipeString += $"{counter} - {recipe.Name}\n";
                counter++;
            }

            return recipeString;
        }

        public void Save() => recipeRepo.Save();

    }
}
