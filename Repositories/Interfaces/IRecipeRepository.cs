using System.Collections.Generic;

namespace Receptek.Repositories.Interfaces
{
    public interface IRecipeRepository
    {
        void Add(Recipe recipe);
        bool Remove(Recipe recipe);
        Recipe? FindByName(string recipeName);
        List<Recipe> GetValues();
        Dictionary<int, Recipe> GetDictionary();

        void Load();
        void Save();
    }
}
