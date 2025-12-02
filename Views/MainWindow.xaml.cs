using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RecipesWinUI.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RecipesWinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavView.SelectedItem = NavView.MenuItems[1];
            ContentFrame.Navigate(typeof(RecipeListPage));
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
                return;

            var item = args.SelectedItem as NavigationViewItem;
            if (item == null)
                return;

            switch (item.Tag?.ToString())
            {

                case "recipes":
                    ContentFrame.Navigate(typeof(RecipeListPage));
                    break;

                case "ingredients":
                    ContentFrame.Navigate(typeof(IngredientListPage));
                    break;
            }
        }
    }
}
