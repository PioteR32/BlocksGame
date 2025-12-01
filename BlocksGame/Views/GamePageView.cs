
//using Android.Database;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksGame.Views
{

	public class GamePageView : BindableObject
	{
		public GameModes GameMode { get; private set; } = GameModes.Normal;
#if ANDROID
		public static int columnCount { get; private set; }  = 3;
#elif WINDOWS
		public static int columnCount { get; private set; }  = 4;
#else
		public static int columnCount { get; private set; } = 4;
#endif
		public int ColumnCount { get; } = columnCount;
		public int RowCount
		{
			get
			{
				int tmp = NumberOfBlocks / columnCount;
				float tmpF = float.Parse(NumberOfBlocks.ToString()) / columnCount;
				if (tmp < tmpF)
					return tmp + 1;
				return tmp;
			}
		}
		int numberOfBlocks = 0;
		public int NumberOfBlocks
		{
			get => numberOfBlocks;
			set
			{
				numberOfBlocks = value;
				OnPropertyChanged(nameof(NumberOfBlocks));
				OnPropertyChanged(nameof(RowCount));
			}
		}
		private int combo = 0;
		public int Combo
		{
			get => combo;
			set
			{
				ChangeCombo = (value - combo).ToString();
				combo = value;
				OnPropertyChanged();
			}
		}
		private int changeCombo = 0;
		public string ChangeCombo
		{
			get
			{
				if (changeCombo < 0)
					return changeCombo.ToString();
				else return "+" + changeCombo.ToString();
			}
			set
			{
				changeCombo = int.Parse(value);
				ChangeComboOpacity = 1;
				OpacityAnimation(150);
				OnPropertyChanged();
			}
		}
		private double changeComboOpacity = 0;
		public double ChangeComboOpacity
		{
			get => changeComboOpacity;
			set
			{
				changeComboOpacity = value;
				OnPropertyChanged();
			}
		}
		private int lives = 2;
		public int Lives
		{
			get => lives;
			set
			{
				lives = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(LivesStr));
			}
		}

		public string LivesStr
		{
			get => "Lives: " + Lives;
		}

		public bool IsEndCombo { get; set; } = false;
		public int BaseWidth { get; private set; }
		public int BaseHeigh { get; private set; }
		public int BaseNumberOfBlocks { get; private set; }
		public ObservableCollection<Block> ToChoose { get; set; } = [];
		int newScore = 0;
		public int NewScore
		{
			get => newScore;
			set
			{
				newScore = value;
				OnPropertyChanged(nameof(NewScoreStr));
			}
		}
		public string NewScoreStr
		{
			get => "+" + NewScore.ToString();

		}
		static int score = 0;
		public int Score
		{
			get => score;
			set
			{
				score = value;
				OnPropertyChanged(nameof(ScoreStr));
			}
		}
		string scoreStr = "Score: ";
		public string ScoreStr
		{
			get => scoreStr + score;
			set
			{
				scoreStr = value;
				OnPropertyChanged();
			}
		}
		public BlocksBoard blocksBoard;

		public BlocksBoard BlocksBoard
		{
			get => blocksBoard;
			set
			{
				blocksBoard = value;
				OnPropertyChanged();
			}
		}
		public GamePageView()
		{
			Score = 0;
			BlocksBoard = new BlocksBoard(3, 3);

		}
		public GamePageView(uint width, uint height, uint numberOfBlocksToChoose, GameModes gameModes)
		{

			Score = 0;
			BlocksBoard = new BlocksBoard(width, height);
			for (int i = 0; i < numberOfBlocksToChoose; i++)
			{
				ToChoose.Add(new Block(BlocksBoard, i));
				NumberOfBlocks++;
			}

			BaseHeigh = (int)height;
			BaseWidth = (int)width;
			BaseNumberOfBlocks = (int)numberOfBlocksToChoose;
			OnPropertyChanged(nameof(ToChoose));
			GameMode = gameModes;
		}
		/// <summary>
		/// Check that player lose and give score
		/// </summary>
		/// <param name="layout"></param>
		/// <param name="b"></param>
		public async Task OpacityAnimation(int miliseconds)
		{
			double minusOpacity = ChangeComboOpacity / miliseconds;
			while (ChangeComboOpacity > 0)
			{
				await Task.Delay(1);
				ChangeComboOpacity = ChangeComboOpacity - minusOpacity;
			}
		}
	}
}
