using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using RecipesWinUI.Controls;
using RecipesWinUI.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace RecipesWinUI.Views
{
    public sealed partial class RecipeListPage : Page
    {
        public RecipeListViewModel ViewModel { get; }
        private RecipeItemViewModel? recipeToDelete;

        private bool isSticky = false;
        private double stickyThreshold = 0;

        private readonly Color BaseCardColor =
            Color.FromArgb(255, 245, 228, 193);

        private readonly Color SelectedCardColor =
            Color.FromArgb(255, 238, 215, 176);

        private readonly Color BaseBorderColor =
            Color.FromArgb(255, 215, 192, 161);

        private readonly Color SelectedBorderColor =
            Color.FromArgb(255, 201, 174, 140);

        public RecipeListPage()
        {
            InitializeComponent();

            ViewModel = App.Services.GetRequiredService<RecipeListViewModel>();
            DataContext = ViewModel;

            Loaded += RecipeListPage_Loaded;
        }

        private void RecipeListPage_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutRoot.UpdateLayout();

            if (ScrollHost.Content is not UIElement content)
                return;

            var transform = OriginalToolbar.TransformToVisual(content);
            Point pt = transform.TransformPoint(new Point(0, 0));
            stickyThreshold = pt.Y;
        }

        private void ScrollHost_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            double offset = ScrollHost.VerticalOffset;

            if (!isSticky && offset >= stickyThreshold)
            {
                isSticky = true;

                StickyToolbarOverlay.Visibility = Visibility.Visible;

                OriginalToolbar.Opacity = 0;
                OriginalToolbar.IsHitTestVisible = false;
            }
            else if (isSticky && offset < stickyThreshold)
            {
                isSticky = false;

                StickyToolbarOverlay.Visibility = Visibility.Collapsed;

                OriginalToolbar.Opacity = 1;
                OriginalToolbar.IsHitTestVisible = true;
            }
        }

        private void CardTapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is RecipeItemViewModel vm)
            {
                vm.IsExpanded = !vm.IsExpanded;

                var container = RecipeList.ContainerFromItem(vm) as ListViewItem;
                if (container != null)
                    UpdateCardVisual(vm, container);

                e.Handled = true;
            }
        }

        private void CardPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is CommonCard card && !card.IsExpanded)
                card.DeleteButtonControl.Visibility = Visibility.Visible;
        }

        private void CardPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is CommonCard card)
                card.DeleteButtonControl.Visibility = Visibility.Collapsed;
        }

        private async void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            var vm = new AddRecipeViewModel();
            Frame.Navigate(typeof(AddRecipePage), vm);
        }

        private async void EditRecipe_Click(object sender, RoutedEventArgs e)
        {
            var selected = ViewModel.Recipes.FirstOrDefault(r => r.IsExpanded);

            if (selected == null)
            {
                var dlg = new ContentDialog
                {
                    Title = "Nincs kiválasztva",
                    Content = "Válassz ki egy receptet!",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await dlg.ShowAsync();
                return;
            }

            var edit = new ContentDialog
            {
                Title = "Recept módosítása",
                Content = "Itt lesz majd a módosító felület.",
                CloseButtonText = "Bezár",
                XamlRoot = this.XamlRoot
            };

            await edit.ShowAsync();
        }

        private void SearchRecipe_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var filtered = ViewModel.Recipes
                    .Where(r => r.Name.Contains(sender.Text, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                RecipeList.ItemsSource = filtered;
                sender.Focus(FocusState.Programmatic);
            }
        }

        private void StickyRecipeSearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ScrollHost.ChangeView(0, 0, 1);

                var normalSearch = OriginalToolbar.SearchBoxControl;
                normalSearch.Text = sender.Text;
                normalSearch.Focus(FocusState.Programmatic);

                sender.Text = "";
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Controls.CommonCard card && card.DataContext is RecipeItemViewModel vm)
            {
                recipeToDelete = vm;
                card.IsExpanded = false;

                Popup.Show(
                    "Recept törlése",
                    $"Biztos törlöd a(z) „{vm.Name}” receptet?",
                    "Mégse",
                    "Törlés"
                );

                Popup.Confirmed += OnDeleteConfirmed;
                Popup.Cancelled += OnDeleteCancelled;
            }
        }

        private void OnDeleteConfirmed()
        {
            if (recipeToDelete != null)
                ViewModel.Delete(recipeToDelete);

            CleanupDeleteHandlers();
        }

        private void OnDeleteCancelled()
        {
            CleanupDeleteHandlers();
        }

        private void CleanupDeleteHandlers()
        {
            recipeToDelete = null;

            Popup.Confirmed -= OnDeleteConfirmed;
            Popup.Cancelled -= OnDeleteCancelled;
        }

        private void UpdateCardVisual(RecipeItemViewModel vm, ListViewItem container)
        {
            var card = (CommonCard)container.ContentTemplateRoot;

            var cardRoot = (Border)card.FindName("CardRoot");

            if (vm.IsExpanded)
            {

                card.IsExpanded = true;

                cardRoot.Background = new SolidColorBrush(SelectedCardColor);
                cardRoot.BorderBrush = new SolidColorBrush(SelectedBorderColor);
                cardRoot.BorderThickness = new Thickness(3);
            }
            else
            {

                card.IsExpanded = false;

                cardRoot.Background = new SolidColorBrush(BaseCardColor);
                cardRoot.BorderBrush = new SolidColorBrush(BaseBorderColor);
                cardRoot.BorderThickness = new Thickness(2);
            }
        }
    }
}
