using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Receptek.Repositories.Interfaces;
using Receptek.Repositories.Json;
using Receptek.Services;
using RecipesWinUI.ViewModels;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.



namespace RecipesWinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = default!;

        private Window? m_window;

        public App()
        {

            InitializeComponent();
            ConfigureServices();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
           // m_window.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            var recs = Services.GetRequiredService<RecipeService>();
            var ings = Services.GetRequiredService<IngredientService>();

            recs.Save();
            ings.Save();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IIngredientRepository, JsonIngredientRepository>();
            services.AddSingleton<IRecipeRepository, JsonRecipeRepository>();

            services.AddSingleton<IngredientService>();
            services.AddSingleton<RecipeService>();

            services.AddSingleton<RecipeListViewModel>();
            services.AddSingleton<IngredientListViewModel>();

            services.AddTransient<EditIngredientViewModel>();

            Services = services.BuildServiceProvider();
        }
    }
}
