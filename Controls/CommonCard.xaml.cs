using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace RecipesWinUI.Controls
{
    public sealed partial class CommonCard : UserControl
    {
        public CommonCard()
        {
            InitializeComponent();

            CardWrapper.Tapped += (s, e) => CardTapped?.Invoke(this, e);
            CardWrapper.PointerEntered += (s, e) => CardPointerEntered?.Invoke(this, e);
            CardWrapper.PointerExited += (s, e) => CardPointerExited?.Invoke(this, e);
        }

        private void DeleteButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            DeleteClicked?.Invoke(this, e);
        }

        public UIElement InnerContent
        {
            get => (ContentHost.Content as UIElement) ?? new Grid();
            set => ContentHost.Content = value;
        }

        public UIElement ExpandContent
        {
            get => (ExpandHost.Content as UIElement) ?? new Grid();
            set => ExpandHost.Content = value;
        }

        public bool IsExpanded
        {
            get => ExpandHost.Visibility == Visibility.Visible;
            set
            {
                ExpandHost.Visibility = value ? Visibility.Visible : Visibility.Collapsed;

                DeleteButton.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public FrameworkElement ExpandSectionElement => ExpandHost;
        public Button DeleteButtonControl => DeleteButton;

        public event TappedEventHandler? CardTapped;
        public event PointerEventHandler? CardPointerEntered;
        public event PointerEventHandler? CardPointerExited;
        public event RoutedEventHandler? DeleteClicked;
    }
}
