//using Microsoft.UI.Xaml.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BlocksGame
{
    public class MainPageView : BindableObject
    {
        static int theBestScore;
        public uint NumberOfBlocksToChoose { get; set; } = 0;
        public static int TheBestScore
        {
            get => theBestScore;
            set
            {
                theBestScore = value;
            }
        }
        public uint Width { get; set; }
        public uint Height { get;
            set; }
        public string TheBestScoreStr
        {
            get { return "The Best Score: " + TheBestScore; }
            set
            {
                OnPropertyChanged();
            }
        }
        public ICommand Start { get; set; }
        public int[] PickerOptions { get; } =
        {
            3,4,5,6,7,8,9,10
        };
        public async void StartGame()
        {
            if(Width > 1 && Height > 1 && NumberOfBlocksToChoose > 1)
            {
                GamePage gamePage = new GamePage(Width, Height, NumberOfBlocksToChoose);
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
            }
            if (!File.Exists(folderPath + "/" + nameof(TheBestScore) + ".txt"))
            {
                ChangeTheBestScoreInFile();
            }
            StreamReader reader = new StreamReader(folderPath + "/" + nameof(TheBestScore) + ".txt");
            TheBestScore = int.Parse(reader.ReadToEnd());
            reader.Close();
        }
        public static void ChangeTheBestScoreInFile(int theBestScore = 0)
        {
            string folderPath = Path.Combine(FileSystem.AppDataDirectory, "BlockGame");
            StreamWriter str = new StreamWriter(folderPath + "/" + nameof(TheBestScore) + ".txt");
            str.Write(theBestScore.ToString());
            str.Close();
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.MainPageView.TheBestScoreStr = Global.MainPageView.TheBestScoreStr;
        }
    }

}
