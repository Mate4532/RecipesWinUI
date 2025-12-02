using Receptek.Models;
using RecipesWinUI.Models;
using System;
using System.Collections.Generic;

public class Recipe : IRecipeAndIngredient
{
    public string Name
    {
        get => name;
        set => name = value ?? throw new ArgumentNullException(nameof(value));
    }
    private string name = "";

    public string Description
    {
        get => description;
        set => description = value ?? throw new ArgumentNullException(nameof(value));
    }
    private string description = "";

    public List<RecipeIngredient> Ingredients { get; set; } = new();
    public Recipe() { }

    public Recipe(string name, List<RecipeIngredient> ingredients, string description)
    {
        Name = name;
        Ingredients = ingredients ?? new List<RecipeIngredient>();
        Description = description;
    }

    public void AddIngredient(int id, int amount, bool isUnitPiece)
        => Ingredients.Add(new RecipeIngredient(id, amount, isUnitPiece));

    public void AddIngredient(RecipeIngredient recipeIngredient)
        => Ingredients.Add(recipeIngredient);
}
