using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Navigation;
using Receptek.Services;
using RecipesWinUI.Models;
using RecipesWinUI.ViewModels;
using System;

namespace RecipesWinUI.Views
{
    public sealed partial class AddIngredientPage : Page
    {
        public AddIngredientViewModel ViewModel { get; }

        public AddIngredientPage()
        {
            InitializeComponent();
            ViewModel = new AddIngredientViewModel();
            DataContext = ViewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            bool ok = ViewModel.Save();

            if (!ok)
            {
                Popup.ShowInfo("Hiba", ViewModel.LastError ?? "Ismeretlen hiba.", "OK");
                return;
            }

            Frame.GoBack();
        }

        private void MeasurementComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (MeasurementComboBox.SelectedIndex == -1)
                MeasurementComboBox.SelectedIndex = 0;
        }

    }
}
