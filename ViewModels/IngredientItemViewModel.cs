using System.ComponentModel;

namespace RecipesWinUI.ViewModels
{
    public class IngredientItemViewModel : INotifyPropertyChanged
    {
        public Ingredient Ingredient { get; }

        private bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        public string Name => Ingredient.Name;
        public string CalorieText => $"{Ingredient.CalorieIn100g} kcal / 100g";
        public string ProteinText => $"{Ingredient.ProteinIn100g} g / 100g";


        public IngredientItemViewModel(Ingredient ingredient)
        {
            Ingredient = ingredient;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
