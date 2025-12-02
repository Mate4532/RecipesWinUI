using Receptek.Models;
using RecipesWinUI.Models;
using System;

public class Ingredient : IRecipeAndIngredient
{
    public string Name
    {
        get => name;
        set => name = value ?? throw new ArgumentNullException(nameof(value));
    }
    private string name = "";

    public int CalorieIn100g
    {
        get => calorieIn100g;
        set
        {
            if (value < 0)
                throw new ArgumentException("Calorie must be >= 0");
            calorieIn100g = value;
        }
    }
    private int calorieIn100g;

    public int ProteinIn100g
    {
        get => proteinIn100g;
        set
        {
            if (value < 0)
                throw new ArgumentException("Protein must be >= 0");
            proteinIn100g = value;
        }
    }
    private int proteinIn100g;

    public bool CanBeMeasuredInPiece { get; set; }
    public int OnePieceWeigh
    {
        get => onePieceWeigh;
        set
        {
            if (value < 0)  
                throw new ArgumentException("Weight must be >= 0");
            onePieceWeigh = value;
        }
    }
    private int onePieceWeigh;

    public MeasurementUnit MeasurementUnit { get; set; }

    public Ingredient() { }

    public Ingredient(int calorie, int protein, string name,
                      bool canBeMeasuredInPiece, int onePieceWeigh = 0)
    {
        Name = name;
        CalorieIn100g = calorie;
        ProteinIn100g = protein;
        CanBeMeasuredInPiece = canBeMeasuredInPiece;
        OnePieceWeigh = onePieceWeigh;
    }
}
