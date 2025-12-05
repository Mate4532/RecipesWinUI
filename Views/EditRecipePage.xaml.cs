using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using RecipesWinUI.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static RecipesWinUI.ViewModels.EditRecipeViewModel;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RecipesWinUI.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EditRecipePage : Page
{
    public EditRecipeViewModel ViewModel { get; private set; } = null!;
    public EditRecipePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is EditRecipeViewModel vm)
        {
            ViewModel = vm;
            DataContext = vm;
        }
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
        if (ViewModel is null)
            throw new ArgumentNullException(nameof(ViewModel));

        if (sender is Button b && b.DataContext is IngredientEntry entry)
        {
            ViewModel.RemoveIngredientFromRecipe(entry);
        }
    }

}
