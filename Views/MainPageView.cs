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
		#region Properties
		uint numberOfBlocksToChoose;
        public uint NumberOfBlocksToChoose
        {
            get => numberOfBlocksToChoose;
            set
            {
                numberOfBlocksToChoose = value;
                if (numberOfBlocksToChoose > 2)
                {
                    TheBestScore = GetTheBestScore(Width, Height, NumberOfBlocksToChoose,GameMode);
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
                    TheBestScore = GetTheBestScore(Width, Height, NumberOfBlocksToChoose,GameMode);
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
                    TheBestScore = GetTheBestScore(Width, Height, NumberOfBlocksToChoose, GameMode);
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
      
        int gameModeInt = 0;
		public int GameModeInt 
        { 
            get => gameModeInt;
            set
            {
                gameModeInt = value;
                GameMode = (GameModes)gameModeInt;
				TheBestScore = GetTheBestScore(Width, Height, NumberOfBlocksToChoose, GameMode);
				OnPropertyChanged();
				OnPropertyChanged(nameof(TheBestScoreStr));
			}
		}

        GameModes gameMode = GameModes.Normal;
		public GameModes GameMode
        {
            get => gameMode;
            set
            {
                gameMode = value;
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
		public string[] GameModesStr { get; } = GameModesStrStatic;

		public static string[] GameModesStrStatic { get; } =
		{
			"Normal","ChaoticBoard","ChaoticBlocks","ChaoticNumberOfBlocks","MaxChaotic"
		};
		#endregion
		public async void StartGame()
        {
            if (Width > 1 && Height > 1 && NumberOfBlocksToChoose > 1)
            {
                GamePage gamePage = new GamePage(Width, Height, NumberOfBlocksToChoose,(GameModes)GameModeInt);
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
            for (int i = 0; GameModesStrStatic.Length > i; i++)
            {
				string tmp = Path.Combine(FileSystem.AppDataDirectory, "BlockGame", "TheBestScores", GameModesStrStatic[i]);
				if (!Directory.Exists(tmp))
				{
					Global.CreateFolder(tmp);
				}
				WriteScoresInFiles((GameModes)i);
            }
        }
		#region FilesMenageFunctions
		public static void ChangeTheBestScoreInFile(int boardWidth, int boardHeight, int blocks,GameModes gameMode, int theBestScore = 0)
        {
            StreamWriter str = new StreamWriter(ScorePath(boardWidth, boardHeight, blocks, gameMode));
            str.Write(theBestScore.ToString());
            str.Close();
        }
        public static void WriteScoresInFiles(GameModes gameMode)
        {
            foreach (int i in PickerOptionsStatic)
                foreach (int j in PickerOptionsStatic)
                    foreach (int k in PickerOptionsStatic)
                    {
                        string path = ScorePath(i, j, k, gameMode);
                        if (!File.Exists(path))
                        {
                            ChangeTheBestScoreInFile(i, j, k, gameMode);
                        }
                    }
        }
        public static int GetTheBestScore(int boardWidth, int boardHeight, int blocks,GameModes gameMode)
        {
            if (boardWidth > 2 && boardHeight > 2 && blocks > 2)
            {
                StreamReader streamReader = new StreamReader(ScorePath(boardWidth, boardHeight, blocks, gameMode));
                int bestScore = int.Parse(streamReader.ReadToEnd());
                streamReader.Close();
                return bestScore;
            }
            else
                return 0;
        }
        public static int GetTheBestScore(uint boardWidth, uint boardHeight, uint blocks, GameModes gameMode)
        {
            if (boardWidth > 2 && boardHeight > 2 && blocks > 2)
            {
                StreamReader streamReader = new StreamReader(ScorePath(boardWidth, boardHeight, blocks, gameMode));
                int bestScore = int.Parse(streamReader.ReadToEnd());
                streamReader.Close();
                return bestScore;
            }
            else
                return 0;
        }
        public static string ScorePath(int boardWidth, int boardHeight, int blocks,GameModes Mode)
        {
            string folderPath = Path.Combine(FileSystem.AppDataDirectory, "BlockGame", "TheBestScores", GameModesStrStatic[(int)Mode]);
            return Path.Combine(folderPath, boardHeight.ToString() + " " + boardWidth.ToString() + " " + blocks.ToString() + ".txt");
        }
        public static string ScorePath(uint boardWidth, uint boardHeight, uint blocks, GameModes Mode)
        {
            string folderPath = Path.Combine(FileSystem.AppDataDirectory, "BlockGame", "TheBestScores", GameModesStrStatic[(int)Mode]);
            return Path.Combine(folderPath, boardHeight.ToString() + " " + boardWidth.ToString() + " " + blocks.ToString() + ".txt");
        }
    }
	#endregion
}
