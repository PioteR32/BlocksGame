
using System.Collections.ObjectModel;
using System.Runtime;
using System.Windows.Input;

namespace BlocksGame
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            Global.MainPageView = this.BindingContext as Views.MainPageView;
            Global.MainPage = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.MainPageView.TheBestScoreStr = Global.MainPageView.TheBestScoreStr;
            GCSettings.LatencyMode = Global.DefaultGCLatencyMode;
        }
    }

}
