using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using RecipesWinUI.ViewModels;
using System;

namespace RecipesWinUI.Views
{
    public sealed partial class EditIngredientPage : Page
    {
        public EditIngredientViewModel? ViewModel { get; private set; }

        public EditIngredientPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is EditIngredientViewModel vm)
            {
                ViewModel = vm;
                DataContext = vm;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
                return;

            bool ok = ViewModel.Save();

            if (!ok)
            {
                Popup.ShowInfo("Hiba", ViewModel.LastError ?? "Nem sikerült menteni az összetevõt.", "OK");
                return;
            }

            Frame.GoBack();
        }

    }
}
