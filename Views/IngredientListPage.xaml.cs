using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using RecipesWinUI.Controls;
using RecipesWinUI.ViewModels;
using System;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Controls = RecipesWinUI.Controls;

namespace RecipesWinUI.Views
{
    public sealed partial class IngredientListPage : Page
    {
        public IngredientListViewModel ViewModel { get; }
        private IngredientItemViewModel? ingredientToDelete;
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

        public IngredientListPage()
        {
            InitializeComponent();

            ViewModel = App.Services.GetRequiredService<IngredientListViewModel>();
            DataContext = ViewModel;

            Loaded += IngredientListPage_Loaded;
        }

        private void IngredientListPage_Loaded(object sender, RoutedEventArgs e)
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

        private void CardWrapper_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is IngredientItemViewModel vm)
            {
                foreach (var item in ViewModel.Ingredients)
                {
                    item.IsSelected = item == vm && !item.IsSelected;

                    var container = IngredientList.ContainerFromItem(item) as ListViewItem;
                    if (container != null)
                        UpdateCardVisual(item, container);
                }

                e.Handled = true;
            }
        }

        private void CardWrapper_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is CommonCard card)
                card.DeleteButtonControl.Visibility = Visibility.Visible;
        }

        private void CardWrapper_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is CommonCard card)
                card.DeleteButtonControl.Visibility = Visibility.Collapsed;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Controls.CommonCard card && card.DataContext is IngredientItemViewModel vm)
            {
                ingredientToDelete = vm;

                Popup.Show(
                    "Összetevõ törlése",
                    $"Biztos törlöd a(z) „{vm.Name}” összetevõt?",
                    "Mégse",
                    "Törlés"
                );

                Popup.Confirmed += OnDeleteConfirmed;
                Popup.Cancelled += OnDeleteCancelled;
            }
        }

        private void OnDeleteConfirmed()
        {
            if (ingredientToDelete != null)
                ViewModel.Delete(ingredientToDelete);

            CleanupDeleteHandlers();
        }

        private void OnDeleteCancelled()
        {
            CleanupDeleteHandlers();
        }

        private void CleanupDeleteHandlers()
        {
            ingredientToDelete = null;

            Popup.Confirmed -= OnDeleteConfirmed;
            Popup.Cancelled -= OnDeleteCancelled;
        }

        private void UpdateCardVisual(IngredientItemViewModel vm, ListViewItem container)
        {
            var card = (Border)((FrameworkElement)container.ContentTemplateRoot)
                    .FindName("CardRoot");
            var wrapper = (Grid)((FrameworkElement)container.ContentTemplateRoot)
                .FindName("CardWrapper");

            if (vm.IsSelected)
            {
                card.Background = new SolidColorBrush(SelectedCardColor);
                card.BorderBrush = new SolidColorBrush(SelectedBorderColor);
                card.BorderThickness = new Thickness(3);
                wrapper.Translation = new Vector3(0, 0, 16);
            }
            else
            {
                card.Background = new SolidColorBrush(BaseCardColor);
                card.BorderBrush = new SolidColorBrush(BaseBorderColor);
                card.BorderThickness = new Thickness(2);
                wrapper.Translation = new Vector3(0, 0, 6);
            }
        }

        private void EditIngredient_Click(object sender, RoutedEventArgs e)
        {
            var selected = ViewModel.Ingredients.FirstOrDefault(i => i.IsSelected);

            if (selected == null)
            {
                Popup.ShowInfo(
                    "Nincs összetevõ kiválasztva",
                    "Válassz ki egy összetevõt!",
                    "Ok"
                );
                return;
            }

            var ingredient = selected.Ingredient;
            var vm = new EditIngredientViewModel(ingredient);

            Frame.Navigate(typeof(EditIngredientPage), vm);
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            var vm = new AddIngredientViewModel();
            Frame.Navigate(typeof(AddIngredientPage), vm);
        }

        private void SearchIngredient_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var filtered = ViewModel.Ingredients
                    .Where(i => i.Name.Contains(sender.Text, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                IngredientList.ItemsSource = filtered;
                sender.Focus(FocusState.Programmatic);
            }
        }

        private void StickyIngredientSearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
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
    }
}
