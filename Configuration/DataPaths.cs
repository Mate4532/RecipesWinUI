using System;
using System.IO;

namespace Receptek.Configuration
{
    public static class DataPaths
    {
        public static readonly string BaseFolder =
            Path.Combine(AppContext.BaseDirectory, "data");

        public static readonly string IngredientFile =
            Path.Combine(BaseFolder, "ingredients.json");

        public static readonly string RecipeFile =
            Path.Combine(BaseFolder, "recipes.json");
    }
}
