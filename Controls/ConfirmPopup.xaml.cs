using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace RecipesWinUI.Controls
{
    public sealed partial class ConfirmPopup : UserControl
    {
        public event Action? Confirmed;
        public event Action? Cancelled;

        public ConfirmPopup()
        {
            this.InitializeComponent();
        }

        public void ShowInfo(string title, string message, string cancelButtonContent)
        {
            TitleText.Text = title;
            MessageText.Text = message;

            ConfirmButton.Visibility = Visibility.Collapsed;
            CancelButton.Content = cancelButtonContent;

            Root.Visibility = Visibility.Visible;
        }


        public void Show(string title, string message, string cancelButtonContent, string confirmButtonContent)
        {
            TitleText.Text = title;
            MessageText.Text = message;

            ConfirmButton.Visibility = Visibility.Visible;
            ConfirmButton.Content = confirmButtonContent;
            CancelButton.Content = cancelButtonContent;

            Root.Visibility = Visibility.Visible;
        }


        public void Hide()
        {
            Root.Visibility = Visibility.Collapsed;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            Confirmed?.Invoke();
            Hide();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Cancelled?.Invoke();
            Hide();
        }
    }
}
