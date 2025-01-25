using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlocksGame.Views
{
    public class MainPageView : BindableObject
    {

        uint numberOfBlocksToChoose;
        public uint NumberOfBlocksToChoose
        {
            get => numberOfBlocksToChoose;
            set
            {
                numberOfBlocksToChoose = value;
                if (numberOfBlocksToChoose > 2)
                {
                    TheBestScore = GetTheBestScore(Width, Height, NumberOfBlocksToChoose);
                    OnPropertyChanged(nameof(TheBestScoreStr));
                }
            }
        }
        static int theBestScore;
        public static int TheBestScore
        {
            get => theBestScore;
            set
            {
                theBestScore = value;
            }
        }
        uint width;
        public uint Width
        {
            get => width;
            set
            {
                width = value;
                if (width > 2)
                {
                    TheBestScore = GetTheBestScore(Width, Height, NumberOfBlocksToChoose);
                    OnPropertyChanged(nameof(TheBestScoreStr));
                }
            }
        }
        uint height;
        public uint Height
        {
            get => height;
            set
            {
                height = value;
                if (height > 2)
                {
                    TheBestScore = GetTheBestScore(Width, Height, NumberOfBlocksToChoose);
                    OnPropertyChanged(nameof(TheBestScoreStr));
                }
            }
        }
        public string TheBestScoreStr
        {
            get { return "The Best Score: " + TheBestScore; }
            set
            {
                OnPropertyChanged();
            }
        }
        public ICommand Start { get; set; }
        public static int[] PickerOptionsStatic { get; } =
        {
            3,4,5,6,7,8,9,10
        };
        public int[] PickerOptions { get; set; } =
        {
            3,4,5,6,7,8,9,10
        };
        public async void StartGame()
        {
            if (Width > 1 && Height > 1 && NumberOfBlocksToChoose > 1)
            {
                GamePage gamePage = new GamePage(Width, Height, NumberOfBlocksToChoose);
                await Global.MainPage.Navigation.PushAsync(gamePage);
            }
        }
        public MainPageView()
        {
            Start = new Command(StartGame);
            string folderPath = Path.Combine(FileSystem.AppDataDirectory, "BlockGame");
            string scoresFolderPath = Path.Combine(FileSystem.AppDataDirectory, "BlockGame", "TheBestScores");
            //Directory.Delete(folderPath, true);
            if (!Directory.Exists(folderPath))
            {
                Global.CreateFolder(folderPath);
            }
            if (!Directory.Exists(scoresFolderPath))
            {
                Global.CreateFolder(scoresFolderPath);
            }
            WriteScoresInFiles();
        }
        public static void ChangeTheBestScoreInFile(int boardWidth, int boardHeight, int blocks, int theBestScore = 0)
        {
            StreamWriter str = new StreamWriter(ScorePath(boardWidth, boardHeight, blocks));
            str.Write(theBestScore.ToString());
            str.Close();
        }
        public static void WriteScoresInFiles()
        {
            foreach (int i in PickerOptionsStatic)
                foreach (int j in PickerOptionsStatic)
                    foreach (int k in PickerOptionsStatic)
                    {
                        string path = ScorePath(i, j, k);
                        if (!File.Exists(path))
                        {
                            ChangeTheBestScoreInFile(i, j, k);
                        }
                    }
        }
        public static int GetTheBestScore(int boardWidth, int boardHeight, int blocks)
        {
            if (boardWidth > 2 && boardHeight > 2 && blocks > 2)
            {
                StreamReader streamReader = new StreamReader(ScorePath(boardWidth, boardHeight, blocks));
                int bestScore = int.Parse(streamReader.ReadToEnd());
                streamReader.Close();
                return bestScore;
            }
            else
                return 0;
        }
        public static int GetTheBestScore(uint boardWidth, uint boardHeight, uint blocks)
        {
            if (boardWidth > 2 && boardHeight > 2 && blocks > 2)
            {
                StreamReader streamReader = new StreamReader(ScorePath(boardWidth, boardHeight, blocks));
                int bestScore = int.Parse(streamReader.ReadToEnd());
                streamReader.Close();
                return bestScore;
            }
            else
                return 0;
        }
        public static string ScorePath(int boardWidth, int boardHeight, int blocks)
        {
            string folderPath = Path.Combine(FileSystem.AppDataDirectory, "BlockGame", "TheBestScores");
            return Path.Combine(folderPath, boardHeight.ToString() + " " + boardWidth.ToString() + " " + blocks.ToString() + ".txt");
        }
        public static string ScorePath(uint boardWidth, uint boardHeight, uint blocks)
        {
            string folderPath = Path.Combine(FileSystem.AppDataDirectory, "BlockGame", "TheBestScores");
            return Path.Combine(folderPath, boardHeight.ToString() + " " + boardWidth.ToString() + " " + blocks.ToString() + ".txt");
        }
    }
}
