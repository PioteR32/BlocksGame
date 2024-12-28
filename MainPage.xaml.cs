//using Microsoft.UI.Xaml.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BlocksGame
{
    public class MainPageView : BindableObject
    {
        int theBestScore;
        public int TheBestScore
        {
            get => theBestScore;
            set
            {
                theBestScore = value;
                OnPropertyChanged(TheBestScoreStr);
            }
        }
        public uint Width { get; set; }
        public uint Height { get;
            set; }
        public string TheBestScoreStr
        {
            get { return "The Best Score: " + TheBestScore; }

        }
        public ICommand Start { get; set; }
        public int[] PickerOptions { get; } =
        {
            3,4,5,6,7,8,9,10
        };
        public async void StartGame()
        {
            if(Width > 1 && Height > 1)
            {
                GamePage gamePage = new GamePage(Width, Height);
                await Global.MainPage.Navigation.PushAsync(gamePage);
            }
        }
        public MainPageView()
        {
            Start = new Command(StartGame);
            string folderPath = Path.Combine(FileSystem.AppDataDirectory, "BlockGame");
            Directory.Delete(folderPath, true);
            if (!Directory.Exists(folderPath))
            {
                Global.CreateFolder(folderPath);
                if (!File.Exists(folderPath + "/" + nameof(TheBestScore) + ".txt"))
                {
                    StreamWriter str = new StreamWriter(folderPath + "/" + nameof(TheBestScore) + ".txt");
                    str.Write("0");
                    str.Close();
                }
                StreamReader reader = new StreamReader(folderPath + "/" + nameof(TheBestScore) + ".txt");
                TheBestScore = int.Parse(reader.ReadToEnd());
            }
        }
    }
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            Global.MainPageView = this.BindingContext as MainPageView;
            Global.MainPage = this;
        }

    }

}
