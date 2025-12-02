using System;

public class RecipeIngredient
{
    public int ID { get; set; }

    public bool IsUnitPiece { get; set; }

    private int amount;
    public int Amount
    {
        get => amount;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Amount must be > 0");
            amount = value;
        }
    }
    public RecipeIngredient() { }

    public RecipeIngredient(int id, int amount, bool isUnitPiece)
    {
        if (id < 0) throw new ArgumentException("Invalid ID");

        ID = id;
        Amount = amount;
        IsUnitPiece = isUnitPiece;
    }
}
