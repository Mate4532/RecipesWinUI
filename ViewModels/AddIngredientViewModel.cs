using Microsoft.Extensions.DependencyInjection;
using Receptek.Services;
using RecipesWinUI;
using RecipesWinUI.Models;
using RecipesWinUI.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks.Dataflow;

public class AddIngredientViewModel : INotifyPropertyChanged
{
    private readonly IngredientService ings;

    public record UnitDisplay(string Label, MeasurementUnit Value);
    public List<UnitDisplay> Units { get; } =
    [
        new("gramm", MeasurementUnit.Gram),
        new("milliliter", MeasurementUnit.Milliliter)
    ];

    private string displayString = string.Empty;
    public string DisplayString
    {
        get => displayString;
        set
        {
            if (displayString != value)
            {
                displayString = value;
                OnChanged(nameof(DisplayString));
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
            }
        }
    }

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

    public AddIngredientViewModel()
    {
        ings = App.Services.GetRequiredService<IngredientService>();
        DisplayString = Units[0].Label;
    }

    private string? Validate()
    {
        if (string.IsNullOrWhiteSpace(EditedName))
            return "A név nem lehet üres.";

        if (ings.IngredientNameExists(EditedName))
            return "Már létezik ilyen nevű összetevő.";

        if (EditedCalorie < 0)
            return "A kalória nem lehet negatív.";

        if (EditedProtein < 0)
            return "A fehérje nem lehet negatív.";

        if (EditedCanBeMeasuredInPiece && EditedOnePieceWeigh <= 0)
            return "A darab súlya nagyobb kell legyen, mint 0.";

        return null;
    }

    public bool Save()
    {
        LastError = Validate();
        if (LastError != null)
            return false;

        var newIngredient = new Ingredient
        {
            Name = EditedName,
            CalorieIn100g = EditedCalorie,
            ProteinIn100g = EditedProtein,
            CanBeMeasuredInPiece = EditedCanBeMeasuredInPiece,
            OnePieceWeigh = EditedOnePieceWeigh,
            MeasurementUnit = EditedMeasurementUnit
        };

        ings.Add(newIngredient);

        var ilvm = App.Services.GetRequiredService<IngredientListViewModel>();
        var iivm = new IngredientItemViewModel(newIngredient);
        ilvm.Add(iivm);

        return true;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnChanged(string prop) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}
