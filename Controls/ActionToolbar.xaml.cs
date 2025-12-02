using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace RecipesWinUI.Controls
{
    public sealed partial class ActionToolbar : UserControl
    {
        public ActionToolbar()
        {
            InitializeComponent();

            EditButton.Click += (s, e) => EditClicked?.Invoke(s, e);
            AddButton.Click += (s, e) => AddClicked?.Invoke(s, e);
            SearchBox.TextChanged += (s, e) => SearchTextChanged?.Invoke(SearchBox, e);
        }

        public AutoSuggestBox SearchBoxControl => SearchBox;

        public event RoutedEventHandler? EditClicked;
        public event RoutedEventHandler? AddClicked;
        public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs>? SearchTextChanged;


        public static readonly DependencyProperty EditButtonTextProperty =
            DependencyProperty.Register(nameof(EditButtonText), typeof(string), typeof(ActionToolbar), new PropertyMetadata("Módosítás"));

        public string EditButtonText
        {
            get => (string)GetValue(EditButtonTextProperty);
            set => SetValue(EditButtonTextProperty, value);
        }


        public static readonly DependencyProperty AddButtonTextProperty =
            DependencyProperty.Register(nameof(AddButtonText), typeof(string), typeof(ActionToolbar), new PropertyMetadata("Hozzáadás"));

        public string AddButtonText
        {
            get => (string)GetValue(AddButtonTextProperty);
            set => SetValue(AddButtonTextProperty, value);
        }


        public static readonly DependencyProperty SearchPlaceholderTextProperty =
            DependencyProperty.Register(nameof(SearchPlaceholderText), typeof(string), typeof(ActionToolbar), new PropertyMetadata("Keresés..."));

        public string SearchPlaceholderText
        {
            get => (string)GetValue(SearchPlaceholderTextProperty);
            set => SetValue(SearchPlaceholderTextProperty, value);
        }
    }
}
