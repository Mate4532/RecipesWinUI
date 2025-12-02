using System;
using System.Collections.Generic;

namespace Receptek.Models
{
    public class IdManager<T> where T : IRecipeAndIngredient
    {
        //Lecserélni listára, minden Recipe, Ingredient, RecipeIngredientnek saját ID
        private Dictionary<int, T> items = new();
        private Queue<int> freeIds = new();
        private int nextId = 0;

        public IdManager()
        {

        }

        public int Add(T item)
        {
            int id = freeIds.Count > 0 ? freeIds.Dequeue() : nextId++;
            items[id] = item;
            return id;
        }

        public bool Remove(T item)
        {
            foreach (var kvp in items)
            {
                if (Equals(kvp.Value, item))
                {
                    items.Remove(kvp.Key);
                    freeIds.Enqueue(kvp.Key);
                    return true;
                }
            }

            return false;
        }

        public int GetId(T item)
        {
            foreach (var kvp in items)
            {
                if (Equals(kvp.Value, item))
                    return kvp.Key;
            }

            return -1;
        }

        public T? GetById(int id)
        {
            items.TryGetValue(id, out T? value);
            return value;
        }

        public T? FindByName(string name)
        {
            foreach (var kvp in items)
            {
                if (string.Equals(kvp.Value.Name, name, StringComparison.OrdinalIgnoreCase))
                    return kvp.Value;
            }

            return default;
        }

        public T? GetValue(int id) => items.TryGetValue(id, out T? value) ? value : default;

        public List<T> GetValues()
        {
            List<T> returnList = new();

            foreach (var kvp in items)
            {
                returnList.Add(kvp.Value);
            }

            return returnList;
        }

        public IdManagerDto<T> ToDto()
        {
            return new IdManagerDto<T>
            {
                Items = new Dictionary<int, T>(items),
                FreeIds = new Queue<int>(freeIds),
                NextId = nextId
            };
        }

        public void FromDto(IdManagerDto<T> dto)
        {
            items = new Dictionary<int, T>(dto.Items);
            freeIds = new Queue<int>(dto.FreeIds);
            nextId = dto.NextId;
        }
        public Dictionary<int, T> GetDictionary() => items;


    }
}
