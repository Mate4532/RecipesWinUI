using Microsoft.Extensions.DependencyInjection;
using Receptek.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace RecipesWinUI.ViewModels
{
    public class RecipeListViewModel
    {
        public ObservableCollection<RecipeItemViewModel> Recipes { get; }

        public RecipeListViewModel()
        {
            var recipeService = App.Services.GetRequiredService<RecipeService>();

            Recipes = new ObservableCollection<RecipeItemViewModel>(
                recipeService.GetValues().Select(r => new RecipeItemViewModel(r)));
        }

        public bool Delete(RecipeItemViewModel rivm)
        {
            var recs = App.Services.GetRequiredService<RecipeService>();

            if (!recs.Remove(rivm.Recipe))
                return false;

            if (!Recipes.Remove(rivm))
                return false;

            return true;
        }
        public void Add(RecipeItemViewModel rivm) => Recipes.Add(rivm);

    }
}
