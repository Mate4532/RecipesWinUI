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

            CardWrapper.PointerPressed += (s, e) => CardPressedEvent?.Invoke(this, e);
            CardWrapper.PointerEntered += CardPointerEntered;
            CardWrapper.PointerExited += CardPointerExited;
        }

        private void CardPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CardPointerEnteredEvent?.Invoke(this, e);

            if (!IsExpanded)
                DeleteButtonControl.Visibility = Visibility.Visible;
        }

        private void CardPointerExited(object sender, PointerRoutedEventArgs e)
        {
            CardPointerExitedEvent?.Invoke(this, e);

            DeleteButtonControl.Visibility = Visibility.Collapsed;
        }

        private void DeleteButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            DeleteClickedEvent?.Invoke(this, e);
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

                DeleteButtonControl.Visibility = Visibility.Collapsed;
            }
        }

        public FrameworkElement ExpandSectionElement => ExpandHost;
        public Button DeleteButtonControl => DeleteButton;

        public event PointerEventHandler? CardPressedEvent;
        public event PointerEventHandler? CardPointerEnteredEvent;
        public event PointerEventHandler? CardPointerExitedEvent;
        public event RoutedEventHandler? DeleteClickedEvent;
    }
}
