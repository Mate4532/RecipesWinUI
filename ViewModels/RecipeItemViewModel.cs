using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Receptek.Services;
using RecipesWinUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RecipesWinUI.ViewModels
{
    public class RecipeItemViewModel : INotifyPropertyChanged
    {
        public Recipe Recipe { get; }
        private readonly RecipeService recs;
        private readonly IngredientService ings;

        private bool isExpanded;
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
            }
        }

        public string Name => Recipe.Name;
        public string CalorieInRecipeString => $"{recs.GetRecipeCalorieOrProtein(Recipe, true)}kcal";
        public string ProteinInRecipeString => $"{recs.GetRecipeCalorieOrProtein(Recipe, false)}g";
        public string Description => Recipe.Description;
        public IList<IngredientLine> IngredientsInRecipeString => IngredientLines;

        public sealed class IngredientLine
        {
            public string Name { get; init; } = "";
            public string AmountAndUnit { get; init; } = "";
        }

        public IList<IngredientLine> IngredientLines
        {
            get
            {
                var result = new List<IngredientLine>();
                var recipeIngredients = Recipe.Ingredients;

                for (int i = 0; i < recipeIngredients.Count; ++i)
                {
                    RecipeIngredient ingredient = recipeIngredients[i];

                    int ingredientId = ingredient.ID;
                    Ingredient? foundIngredient = ings.GetById(ingredientId);
                    if (foundIngredient == null)
                        throw new Exception("A ReceptIngredient id-je nincs benne az Ingredientek közt.");

                    string amountString = foundIngredient.CanBeMeasuredInPiece && !ingredient.IsUnitPiece
                        ? $"{ingredient.Amount * foundIngredient.OnePieceWeigh}"
                        : $"{ingredient.Amount}";

                    string unitPart;

                    if (ingredient.IsUnitPiece && foundIngredient.CanBeMeasuredInPiece)
                    {
                        unitPart =
                            $"db ({ingredient.Amount * foundIngredient.OnePieceWeigh}{(foundIngredient.MeasurementUnit == MeasurementUnit.Gram ? "g" : "ml")})";
                    }
                    else
                    {
                        unitPart = foundIngredient.MeasurementUnit switch
                        {
                            MeasurementUnit.Gram => "g",
                            MeasurementUnit.Milliliter => "ml",
                            _ => ""
                        };
                    }

                    result.Add(new IngredientLine
                    {
                        Name = foundIngredient.Name,
                        AmountAndUnit = $"{amountString}{unitPart}"
                    });
                }

                return result;
            }
        }

        public RecipeItemViewModel(Recipe recipe)
        {
            Recipe = recipe;
            recs = App.Services.GetRequiredService<RecipeService>();
            ings = App.Services.GetRequiredService<IngredientService>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
