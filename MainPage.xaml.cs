using CheckMate.Resources.ViewModel;
using Microsoft.Maui.Controls;

namespace CheckMate.View
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            MainViewModel mainViewModel = new MainViewModel();

            BindingContext = mainViewModel;
        }

        private void OnBtnClicked(object sender, EventArgs e)
        {

        }

        
    }
}
