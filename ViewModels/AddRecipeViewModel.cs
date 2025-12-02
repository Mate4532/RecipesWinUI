using Microsoft.Extensions.DependencyInjection;
using Receptek.Services;
using RecipesWinUI;
using RecipesWinUI.Models;
using RecipesWinUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

public class AddRecipeViewModel : INotifyPropertyChanged
{
    public sealed class IngredientEntry : INotifyPropertyChanged
    {
        private string name = "";
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public int Id { get; }

        private int amount;
        public int Amount
        {
            get => amount;
            set
            {
                if (amount != value)
                {
                    amount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                }
            }
        }

        public IngredientEntry(int id, string name, int amount = 0)
        {
            Id = id;
            Name = name;
            Amount = amount;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    private readonly RecipeService recs;
    private readonly IngredientService ings;

    public List<Ingredient> AllIngredients { get; }
    public ObservableCollection<IngredientEntry> SelectedIngredients { get; } = new();

    private List<Ingredient> searchResults = new();
    public List<Ingredient> SearchResults
    {
        get => searchResults;
        private set
        {
            if (searchResults != value)
            {
                searchResults = value;
                OnChanged(nameof(SearchResults));
            }
        }
    }

    private string editedName = "";
    public string EditedName
    {
        get => editedName;
        set
        {
            if (editedName != value)
            {
                editedName = value;
                OnChanged(nameof(EditedName));
            }
        }
    }

    private string editedDescription = "";
    public string EditedDescription
    {
        get => editedDescription;
        set
        {
            if (editedDescription != value)
            {
                editedDescription = value;
                OnChanged(nameof(EditedDescription));
            }
        }
    }

    private string ingredientSearchText = "";
    public string IngredientSearchText
    {
        get => ingredientSearchText;
        set
        {
            if (ingredientSearchText != value)
            {
                ingredientSearchText = value;
                OnChanged(nameof(IngredientSearchText));
                UpdateSearchResults();
            }
        }
    }

    public string? LastError { get; private set; }

    public AddRecipeViewModel()
    {
        recs = App.Services.GetRequiredService<RecipeService>();
        ings = App.Services.GetRequiredService<IngredientService>();

        AllIngredients = ings.GetValues();
        SearchResults = AllIngredients;
    }

    private void UpdateSearchResults()
    {
        if (string.IsNullOrWhiteSpace(IngredientSearchText))
        {
            SearchResults = AllIngredients;
            return;
        }

        SearchResults = AllIngredients
            .Where(i => i.Name.Contains(IngredientSearchText, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public void AddIngredientToRecipe(Ingredient ingredient)
    {
        if (ingredient == null)
            return;

        int id = ings.GetId(ingredient);

        bool exists = SelectedIngredients.Any(x => x.Id == id);
        if (exists)
            return;

        var entry = new IngredientEntry(
            id: id,
            name: ingredient.Name,
            amount: 0
        );

        SelectedIngredients.Add(entry);

        var sorted = SelectedIngredients.OrderBy(x => x.Name).ToList();
        SelectedIngredients.Clear();
        foreach (var s in sorted)
            SelectedIngredients.Add(s);
    }

    public void RemoveIngredientFromRecipe(IngredientEntry ie)
    {
        SelectedIngredients.Remove(ie);
    }

    private string? Validate()
    {
        if (string.IsNullOrWhiteSpace(EditedName))
            return "A recept neve nem lehet üres.";

        if (recs.RecipeNameExists(EditedName))
            return "Már létezik ilyen nevű recept.";

        if (SelectedIngredients.Count == 0)
            return "Legalább egy összetevőt hozzá kell adni.";

        if (SelectedIngredients.Any(x => x.Amount <= 0))
            return "Egyik összetevő mennyisége sem lehet kisebb mint egy.";

        return null;
    }

    public bool Save()
    {
        LastError = Validate();
        if (LastError != null)
            return false;

        List<RecipeIngredient> recipeIngredients = new();

        foreach(IngredientEntry ie in SelectedIngredients)
        {
            RecipeIngredient ri = new RecipeIngredient(ie.Id, ie.Amount, false);
            recipeIngredients.Add(ri);
        }

        var recipe = new Recipe(
            EditedName,
            recipeIngredients,
            EditedDescription
        );

        recs.Add(recipe);

        var rlvm = App.Services.GetRequiredService<RecipeListViewModel>();
        rlvm.Add(new RecipeItemViewModel(recipe));

        return true;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnChanged(string prop) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}
