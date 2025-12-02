using Microsoft.Extensions.DependencyInjection;
using Receptek.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesWinUI.ViewModels
{
    public class IngredientListViewModel
    {
        public ObservableCollection<IngredientItemViewModel> Ingredients { get; }

        public IngredientListViewModel(IngredientService service)
        {
            Ingredients = new ObservableCollection<IngredientItemViewModel>(
            service.GetValues().Select(i => new IngredientItemViewModel(i)));
        }

        public bool Delete(IngredientItemViewModel iivm)
        {
            var ings = App.Services.GetRequiredService<IngredientService>();
            var recs = App.Services.GetRequiredService<RecipeService>();

            recs.DeleteIdInRecipes(ings.GetId(iivm.Ingredient));

            if (!ings.Remove(iivm.Ingredient))
                return false;

            if (!Ingredients.Remove(iivm))
                return false;

            return true;
        }
        public void Add(IngredientItemViewModel iivm) => Ingredients.Add(iivm);
    }
}
