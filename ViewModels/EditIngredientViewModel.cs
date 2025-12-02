using Microsoft.Extensions.DependencyInjection;
using Receptek.Services;
using RecipesWinUI;
using RecipesWinUI.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.Graphics.Effects;

public class  EditIngredientViewModel : INotifyPropertyChanged
{
    private readonly IngredientService ings;

    public Ingredient Ingredient { get; }

    public record UnitDisplay(string Label, MeasurementUnit Value);
    public List<UnitDisplay> Units { get; } =
    [
        new("gramm", MeasurementUnit.Gram),
        new("milliliter", MeasurementUnit.Milliliter)
    ];

    private string editedName = string.Empty;
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

    private MeasurementUnit editedMeasurementUnit;

    public MeasurementUnit EditedMeasurementUnit
    {
        get => editedMeasurementUnit;
        set
        {
            if (editedMeasurementUnit != value)
            {
                editedMeasurementUnit = value;
                OnChanged(nameof(EditedMeasurementUnit));
                OnChanged(nameof(SelectedUnit));
            }
        }
    }

    public UnitDisplay? SelectedUnit
    {
        get => Units.Find(u => u.Value == EditedMeasurementUnit);
        set
        {
            if (value != null && editedMeasurementUnit != value.Value)
            {
                editedMeasurementUnit = value.Value;
                OnChanged(nameof(EditedMeasurementUnit));
                OnChanged(nameof(SelectedUnit));
            }
        }
    }

    private bool editedCanBeMeasuredInPiece;
    public bool EditedCanBeMeasuredInPiece
    {
        get => editedCanBeMeasuredInPiece;
        set
        {
            if (editedCanBeMeasuredInPiece != value)
            {
                editedCanBeMeasuredInPiece = value;
                OnChanged(nameof(EditedCanBeMeasuredInPiece));
            }
        }
    }

    private int editedOnePieceWeigh;
    public int EditedOnePieceWeigh
    {
        get => editedOnePieceWeigh;
        set
        {
            if (editedOnePieceWeigh != value)
            {
                editedOnePieceWeigh = value;
                OnChanged(nameof(EditedOnePieceWeigh));
            }
        }
    }

    private int editedCalorie;
    public int EditedCalorie
    {
        get => editedCalorie;
        set
        {
            if (editedCalorie != value)
            {
                editedCalorie = value;
                OnChanged(nameof(EditedCalorie));
            }
        }
    }

    private int editedProtein;
    public int EditedProtein
    {
        get => editedProtein;
        set
        {
            if (editedProtein != value)
            {
                editedProtein = value;
                OnChanged(nameof(EditedProtein));
            }
        }
    }

    public string? LastError { get; private set; }

    public EditIngredientViewModel(Ingredient ingredient)
    {
        Ingredient = ingredient;
        ings = App.Services.GetRequiredService<IngredientService>();

        EditedName = ingredient.Name;
        EditedCanBeMeasuredInPiece = ingredient.CanBeMeasuredInPiece;
        EditedOnePieceWeigh = ingredient.OnePieceWeigh;
        EditedCalorie = ingredient.CalorieIn100g;
        EditedProtein = ingredient.ProteinIn100g;
        EditedMeasurementUnit = Ingredient.MeasurementUnit;
    }

    private string? Validate()
    {
        if (string.IsNullOrWhiteSpace(EditedName))
            return "A név nem lehet üres.";

        if (ings.IngredientNameExists(EditedName, ings.GetId(Ingredient)))
            return "Már létezik ilyen nevű összetevő.";

        if (EditedCanBeMeasuredInPiece && EditedOnePieceWeigh <= 0)
            return "A darab súlya 1 g-nál nagyobb kell legyen.";

        return null;
    }

    public bool Save()
    {
        LastError = Validate();
        if (LastError != null)
            return false;

        Ingredient.Name = EditedName;
        Ingredient.CanBeMeasuredInPiece = EditedCanBeMeasuredInPiece;
        Ingredient.OnePieceWeigh = EditedOnePieceWeigh;
        Ingredient.CalorieIn100g = EditedCalorie;
        Ingredient.ProteinIn100g = EditedProtein;
        Ingredient.MeasurementUnit = EditedMeasurementUnit;

        //Ide kell adni majd egy updateIngredient-et, ami egyből ment

        return true;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnChanged(string prop) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}

