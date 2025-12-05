using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RecipesWinUI.Models;
using RecipesWinUI.ViewModels;
using System;
using static AddRecipeViewModel;

namespace RecipesWinUI.Views
{
    public sealed partial class AddRecipePage : Page
    {
        public AddRecipeViewModel ViewModel { get; }

        public AddRecipePage()
        {
            InitializeComponent();
            ViewModel = new AddRecipeViewModel();
            DataContext = ViewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool ok = ViewModel.Save();

            if (!ok)
            {
                Popup.ShowInfo("Hiba", ViewModel.LastError ?? "Ismeretlen hiba.", "OK");
                return;
            }

            Frame.GoBack();
        }

        private void IngredientSuggestionChosen(
            AutoSuggestBox sender,
            AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is Ingredient ingredient)
            {
                ViewModel.AddIngredientToRecipe(ingredient);
                sender.Text = "";
            }
        }

        private void RemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.DataContext is IngredientEntry entry)
            {
                ViewModel.RemoveIngredientFromRecipe(entry);
            }
        }

    }
}
