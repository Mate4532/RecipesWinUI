using Receptek.Configuration;
using Receptek.Models;
using Receptek.Repositories.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Receptek.Repositories.Json
{
    public class JsonIngredientRepository : IIngredientRepository
    {
        private IdManager<Ingredient> manager = new();

        public JsonIngredientRepository()
        {
            Load();
        }
        public void Load()
        {
            Directory.CreateDirectory(DataPaths.BaseFolder);

            if (!File.Exists(DataPaths.IngredientFile))
                File.WriteAllText(DataPaths.IngredientFile, "{\"Items\":{},\"FreeIds\":[],\"NextId\":0}");

            string json = File.ReadAllText(DataPaths.IngredientFile);
            var dto = JsonSerializer.Deserialize<IdManagerDto<Ingredient>>(json);

            manager = new IdManager<Ingredient>();
            if (dto != null)
                manager.FromDto(dto);
        }

        public void Save()
        {
            Directory.CreateDirectory(DataPaths.BaseFolder);

            var dto = manager.ToDto();
            string json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(DataPaths.IngredientFile, json);
        }

        public void AddIngredient(Ingredient ingredient) => manager.Add(ingredient);

        public void Add(Ingredient ingredient) => manager.Add(ingredient);

        public bool Remove(Ingredient ingredient) => manager.Remove(ingredient);

        public Ingredient? FindByName(string ingredientName) => manager.FindByName(ingredientName);

        public int GetId(Ingredient ingredient) => manager.GetId(ingredient);

        public Ingredient? GetById(int id) => manager.GetValue(id);

        public List<Ingredient> GetValues() => manager.GetValues();

        public Dictionary<int, Ingredient> GetDictionary() => manager.GetDictionary();
    }
}
