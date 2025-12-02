using System.Collections.Generic;

namespace Receptek.Repositories.Interfaces
{
    public interface IIngredientRepository
    {
        void Add(Ingredient ingredient);
        bool Remove(Ingredient ingredient);
        Ingredient? FindByName(string ingredientName);
        int GetId(Ingredient ingredient);
        Ingredient? GetById(int id);
        List<Ingredient> GetValues();
        Dictionary<int, Ingredient> GetDictionary();

        void Load();
        void Save();
    }
}
