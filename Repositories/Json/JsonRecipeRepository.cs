using Receptek.Configuration;
using Receptek.Models;
using Receptek.Repositories.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Receptek.Repositories.Json
{
    public class JsonRecipeRepository : IRecipeRepository
    {
        private IdManager<Recipe> manager = new();

        public JsonRecipeRepository()
        {
            Load();
        }

        public void Load()
        {
            Directory.CreateDirectory(DataPaths.BaseFolder);

            if (!File.Exists(DataPaths.RecipeFile))
                File.WriteAllText(DataPaths.RecipeFile,
                    "{\"Items\":{}, \"FreeIds\":[], \"NextId\":0}");

            string json = File.ReadAllText(DataPaths.RecipeFile);
            var dto = JsonSerializer.Deserialize<IdManagerDto<Recipe>>(json);

            manager = new IdManager<Recipe>();
            if (dto != null)
                manager.FromDto(dto);
        }

        public void Save()
        {
            Directory.CreateDirectory(DataPaths.BaseFolder);

            var dto = manager.ToDto();
            string json = JsonSerializer.Serialize(dto,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(DataPaths.RecipeFile, json);
        }

        public void Add(Recipe recipe) => manager.Add(recipe);

        public bool Remove(Recipe recipe) => manager.Remove(recipe);

        public Recipe? FindByName(string recipeName) => manager.FindByName(recipeName);

        public int GetId(Recipe recipe) => manager.GetId(recipe);

        public Recipe? GetById(int id) => manager.GetById(id);

        public List<Recipe> GetValues() => manager.GetValues();

        public Dictionary<int, Recipe> GetDictionary() => manager.GetDictionary();
    }
}
