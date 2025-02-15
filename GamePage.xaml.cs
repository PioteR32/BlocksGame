//using Microsoft.UI.Xaml.Documents;
using BlocksGame.Views;
using Microsoft.Maui.Controls.PlatformConfiguration.GTKSpecific;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Layouts;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Linq;
using static BlocksGame.Block;


namespace BlocksGame;
public struct Position
{
	public Position(double x, double y)
	{
		X = x; Y = y;
	}
	public double X { get; set; }
	public double Y { get; set; }
	public static bool operator ==(Position p, Position p1)
	{
		if (p.X == p1.X && p.Y == p1.Y)
			return true;
		else
			return false;
	}
	public static bool operator !=(Position p, Position p1)
	{
		if (p.X != p1.X || p.Y != p1.Y)
			return true;
		else
			return false;
	}
}
public enum GameModes
{
	Normal, ChaoticBoard, ChaoticBlocks, ChaoticNumberOfBlocks, MaxChaotic

}
public partial class GamePage : ContentPage
{
	List<Position> TmpAddedBlocksPositions = new List<Position>();
	public static IList<Block> NextToChoose { get; set; } = [];

	static int score = 0;
	Views.GamePageView viewModel;

	public GamePage(uint width, uint height, uint numberOfBlocksToChoose)
	{
		viewModel = new Views.GamePageView(width, height, numberOfBlocksToChoose, GameModes.Normal);
		BindingContext = viewModel;

		InitializeComponent();
	}

	public GamePage(uint width, uint height, uint numberOfBlocksToChoose, GameModes gameModes)
	{
		viewModel = new Views.GamePageView(width, height, numberOfBlocksToChoose, gameModes);
		BindingContext = viewModel;
		InitializeComponent();
	}
	/// <summary>
	/// Check block on grid position in blocksView
	/// </summary>
	/// <param name="grid"></param>
	/// <returns>Object of Blocks</returns>
	Block WhichToChooseBlock(Grid grid)
	{
		for (int i = 0; i < blocksView.Children.Count; i++)
		{
			if (blocksView.Children[i] is Grid)
			{
				if (blocksView.Children[i] == grid)
					return viewModel.ToChoose[i];
			}
		}
		return null;
	}
	#region AddBlocksToBoard

	#region CheckPositionsBlocks
	private bool IsPositionInGrid(int rowPosition, int columnPosition, IList<PositionsInTable> positions)
	{
		try
		{
			if (viewModel.BlocksBoard.Fields[rowPosition, columnPosition].IsFree)
			{
				positions.Add(new PositionsInTable(columnPosition, rowPosition));
				return true;
			}
		}
		catch
		{
			return false;
		}
		return false;
	}
	private bool IsFreeSpaceOnBlock(int x, int y, double boardColumnWidth, double boardColumnHeight, IList<PositionsInTable> positions, Block block)
	{
		for (int i = 0, j = 0; j < block.BlockStructure.GetLength(0); i++)
		{
			if (block.BlockStructure[j, i])
			{

				if (!IsPositionInGrid(j + y, i + x, positions))
				{
					return false;
				}
			}
			if (i + 1 == block.BlockStructure.GetLength(1))
			{
				i = -1;
				j++;
			}
		}
		return true;
	}
	private bool IsPositionInGrid(int rowPosition, int columnPosition)
	{
		try
		{
			if (viewModel.BlocksBoard.Fields[rowPosition, columnPosition].IsFree)
				return true;
		}
		catch
		{
			return false;
		}
		return false;
	}
	public bool IsFreeSpaceOnBlock(int x, int y, Block block)
	{
		for (int i = 0, j = 0; j < block.BlockStructure.GetLength(0); i++)
		{
			if (block.BlockStructure[j, i])
			{
				if (!IsPositionInGrid(j + y, i + x))
				{
					return false;
				}
			}
			if (i + 1 == block.BlockStructure.GetLength(1))
			{
				i = -1;
				j++;
			}
		}
		return true;
	}
	#endregion

	/// <summary>
	/// Add Blocks To Board
	/// </summary>
	/// <param name="BlockView"> Block's Grid </param>
	/// <returns> Has added block </returns>
	public bool AddBlockToBoard(Grid BlockView)
	{
		Position grid = GamePage.GetGridXY((Grid)BlockView.Parent);

		double boardColumnWidth = boardGrid.Width / viewModel.BlocksBoard.Width;
		double boardColumnHeight = boardGrid.Height / viewModel.BlocksBoard.Height;
		IList<PositionsInTable> positions = new List<PositionsInTable>();
		for (int i = 0, j = 0; j < viewModel.BlocksBoard.Height; i += 1)
		{
			if (BlockView.X + grid.X + BlockView.TranslationX > i * boardColumnWidth + boardGrid.X && BlockView.X + grid.X + BlockView.TranslationX < (i + 1) * boardColumnWidth + boardGrid.X
				&& BlockView.Y + grid.Y + BlockView.TranslationY > j * boardColumnHeight + boardGrid.Y && BlockView.Y + grid.Y + BlockView.TranslationY < (j + 1) * boardColumnHeight + boardGrid.Y)
			{
				if (panGetsureBlock != null)
					if (IsFreeSpaceOnBlock(i, j, boardColumnWidth, boardColumnHeight, positions, panGetsureBlock))
					{
						foreach (var position in positions)
						{
							viewModel.BlocksBoard.Fields[position.y, position.x].BackgroundColor = panGetsureBlock.BackgroundColor;
							viewModel.BlocksBoard.Fields[position.y, position.x].IsFree = false;
							viewModel.BlocksBoard.Fields[position.y, position.x].ShadowOpacity = 0;
						}

						return true;
					}
			}
			if (i + 1 == viewModel.BlocksBoard.Width)
			{
				i = -1;
				j++;
			}
		}
		return false;
	}

	#region AddBlockToBoardWhilePanGetsureRunning
	private bool AddBlockToBoardWhileRunning(Grid BlockView)
	{
		Position grid = GamePage.GetGridXY((Grid)BlockView.Parent);

		double boardColumnWidth = boardGrid.Width / viewModel.BlocksBoard.Width;
		double boardColumnHeight = boardGrid.Height / viewModel.BlocksBoard.Height;
		IList<PositionsInTable> positions = new List<PositionsInTable>();
		for (int j = 0; j < viewModel.BlocksBoard.Height; j++)
		{
			for (int i = 0; i < viewModel.BlocksBoard.Width; i++)
				if (BlockView.X + grid.X + BlockView.TranslationX > i * boardColumnWidth + boardGrid.X && BlockView.X + grid.X + BlockView.TranslationX < (i + 1) * boardColumnWidth + boardGrid.X
					&& BlockView.Y + grid.Y + BlockView.TranslationY > j * boardColumnHeight + boardGrid.Y && BlockView.Y + grid.Y + BlockView.TranslationY < (j + 1) * boardColumnHeight + boardGrid.Y)
				{
					if (panGetsureBlock != null)
						if (IsFreeSpaceOnBlock(i, j, boardColumnWidth, boardColumnHeight, positions, panGetsureBlock))
						{
							List<Position> actualPositions = new List<Position>();
							foreach (var position in positions)
							{
								viewModel.BlocksBoard.Fields[position.y, position.x].BackgroundColor = panGetsureBlock.BackgroundColor;
								actualPositions.Add(new Position(position.x, position.y));
							}
							if (!IsThisSamePositions(TmpAddedBlocksPositions, actualPositions))
							{
								DeleteTmpBlocks();
							}
							TmpAddedBlocksPositions = actualPositions;
							return true;
						}
				}
		}
		if (TmpAddedBlocksPositions != null)
			if (TmpAddedBlocksPositions.Count > 0)
			{
				DeleteTmpBlocks();
				TmpAddedBlocksPositions = new List<Position>();
			}
		return false;
	}

	private void DeleteTmpBlocks()
	{
		foreach (Position position in TmpAddedBlocksPositions)
		{
			try
			{
				if (viewModel.BlocksBoard.Fields[(int)position.Y, (int)position.X].IsFree)
				{
					viewModel.BlocksBoard.Fields[(int)position.Y, (int)position.X].BackgroundColor = Field.DefaultColor;
					viewModel.BlocksBoard.Fields[(int)position.Y, (int)position.X].ShadowOpacity = 0;
				}
			}
			catch { }
		}
	}

	private bool IsThisSamePositions(IList<Position> positions, IList<Position> positionsToCheck)
	{
		IList<Position> positionsTmp = new List<Position>(positions); IList<Position> positionsToCheckTmp = new List<Position>(positionsToCheck);
		for (int i = 0; i < positionsTmp.Count; i++)
		{
			for (int j = 0; j < positionsToCheckTmp.Count; j++)
			{
				try
				{
					if (positionsTmp[i] == positionsToCheckTmp[j])
					{
						positionsTmp.RemoveAt(i);
						positionsToCheckTmp.RemoveAt(j);
						j--;
					}
					else
						return false;
				}
				catch { }
			}
			i--;
		}
		return true;
	}

	private void ChangeBlockView(double newWidth, double newHeight, Grid grid)
	{
		foreach (var child in grid)
		{
			if (child.DesiredSize.Width > 10 && child.DesiredSize.Height > 10)
			{
				VisualElement childL = child as VisualElement;
				childL.WidthRequest = newWidth;
				childL.HeightRequest = newHeight;
			}
		}
	}
	private bool IListsIsThisSameValues(IList<int> values, IList<int> values1)
	{
		if (values.Count != values1.Count)
			return false;
		IList<int> tmp = new List<int>(values), tmp1 = new List<int>(values1);
		for (int i = 0; i < tmp.Count; i++)
		{
			bool IsTheSame = false;
			for (int j = 0; j < tmp1.Count; j++)
			{
				if (values[i] == tmp1[j])
				{
					IsTheSame = true;
					tmp.RemoveAt(i);
					tmp1.RemoveAt(j);
					i--;
					j--;
					break;
				}
			}
			if (!IsTheSame)
			{
				return false;
			}
		}
		return true;
	}
	public static void ChangeCompletedPositions(BlocksBoard b, Color newColor, IList<int> completedRows, IList<int> completedColumns, double Opacity = 1)
	{
		foreach (int column in completedColumns)
		{
			for (int i = 0; i < b.Fields.GetLength(0); i++)
			{
				//if(!b.Fields[i, column].IsFree)
				//	b.Fields[i, column].BackgroundColor = newColor;
				b.Fields[i, column].ShadowColor = newColor;
				b.Fields[i, column].ShadowOpacity = Opacity;
			}
		}
		foreach (int row in completedRows)
		{
			for (int i = 0; i < b.Fields.GetLength(1); i++)
			{
				//if (!b.Fields[row, i].IsFree)
				//	b.Fields[row, i].BackgroundColor = newColor;
				b.Fields[row, i].ShadowColor = newColor;
				b.Fields[row, i].ShadowOpacity = Opacity;
				//object value = await b.fields[row, i].Label.ColorTo(b.fields[row, i].BackgroundColor, newColor, c => b.fields[row, i].Label.BackgroundColor = c, 10);
			}
		}
	}

	#endregion
	#endregion

	Block panGetsureBlock;
	bool newPanGetsure = true;
	int toSwitch = 50;
	IList<int> completedRows = new List<int>();
	IList<int> completedColumns = new List<int>();
	public void PanGetsureFnc(object sender, PanUpdatedEventArgs e)
	{
		if (sender is Grid)
		{
			Grid grid = sender as Grid;
			switch (e.StatusType)
			{
				case GestureStatus.Started:

				case GestureStatus.Running:
					if (newPanGetsure)
					{
						panGetsureBlock = WhichToChooseBlock(grid);
					}

					if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
					{
						grid.TranslationX += e.TotalX; grid.TranslationY += e.TotalY - toSwitch;
					}
					else
					{
						grid.TranslationX = e.TotalX; grid.TranslationY = e.TotalY - toSwitch;
					}
					if (grid.Width / grid.ColumnDefinitions.Count != boardGrid.Width / viewModel.BlocksBoard.Width)
					{

						ChangeBlockView(boardGrid.Width / viewModel.BlocksBoard.Width,
							boardGrid.Height / viewModel.BlocksBoard.Height,
							grid);

					}
					IList<int> completedRows = CheckCompletedRows(viewModel.BlocksBoard, Field.DefaultColor);
					IList<int> completedColumns = CheckCompletedColumns(viewModel.BlocksBoard, Field.DefaultColor);
					if (!IListsIsThisSameValues(completedColumns, this.completedColumns))
					{
						ChangeCompletedPositions(viewModel.BlocksBoard, panGetsureBlock.BackgroundColor, new List<int>(), this.completedColumns, 0);
					}
					if (!IListsIsThisSameValues(completedRows, this.completedRows))
					{
						ChangeCompletedPositions(viewModel.BlocksBoard, panGetsureBlock.BackgroundColor, this.completedRows, new List<int>(), 0);
					}
					ChangeCompletedPositions(viewModel.BlocksBoard, panGetsureBlock.BackgroundColor, completedRows, completedColumns);
					AddBlockToBoardWhileRunning(grid);
					this.completedColumns = completedColumns;
					this.completedRows = completedRows;
					break;
				case GestureStatus.Completed:
				case GestureStatus.Canceled:
					newPanGetsure = true;
					if (!AddBlockToBoard(grid))
					{
						grid.TranslateTo(e.TotalX, e.TotalY);
						ChangeBlockView(Block.FieldWidth, Block.FieldHeight, grid);
					}
					else
					{
						grid.IsVisible = false;
						MenageToChoose((Grid)grid.Parent);
					}
					ChangeCompletedPositions(viewModel.BlocksBoard, panGetsureBlock.BackgroundColor, new List<int>(), this.completedColumns, 0);
					ChangeCompletedPositions(viewModel.BlocksBoard, panGetsureBlock.BackgroundColor, this.completedRows, new List<int>(), 0);
					break;
			}
		}
	}

	#region MenageGame
	public async void MenageToChoose(Grid layout)
	{
		Random random = new Random();
		bool canGenerateNewBlocks = true;
		int beginBlocksToChoose = viewModel.ToChoose.Count;

		for (int i = 0; i < viewModel.ToChoose.Count; i++)
		{
			if (blocksView.Children[i] is Grid)
			{
				Grid grid = (Grid)blocksView.Children[i];
				if (grid.IsVisible)
				{
					canGenerateNewBlocks = false;
					break;
				}

			}
		}
		IsGetUserScore(viewModel.BlocksBoard);
		if (canGenerateNewBlocks)
		{
			if (viewModel.GameMode == GameModes.ChaoticBoard || viewModel.GameMode == GameModes.MaxChaotic)
			{
				viewModel.BlocksBoard = new BlocksBoard((uint)random.Next(3, 10), (uint)random.Next(3, 10), viewModel.BlocksBoard);
			}
			if (viewModel.GameMode == GameModes.ChaoticNumberOfBlocks || viewModel.GameMode == GameModes.MaxChaotic)
			{
				int numberOfBlocks = random.Next(3, 10);
				viewModel.ToChoose.Clear();
				if (viewModel.IsEndCombo)
					viewModel.Combo = 0;
				viewModel.IsEndCombo = true;
				for (int i = 0; i < numberOfBlocks; i++)
				{
					viewModel.ToChoose.Add(Block.GenerateBlock(viewModel.BlocksBoard, i));
				}
				viewModel.NumberOfBlocks = viewModel.ToChoose.Count;
			}
			else if (viewModel.GameMode != GameModes.ChaoticBlocks)
			{
				if (viewModel.IsEndCombo)
					viewModel.Combo = 0;
				viewModel.IsEndCombo = true;
				for (int i = 0; i < viewModel.ToChoose.Count; i++)
				{
					viewModel.ToChoose[i] = new Block(viewModel.BlocksBoard, i);
				}
			}

		}
		if (viewModel.GameMode == GameModes.ChaoticBlocks || viewModel.GameMode == GameModes.MaxChaotic)
		{
			viewModel.ToChoose.Clear();
			if (viewModel.NumberOfBlocks - 1 == 0)
			{
				viewModel.NumberOfBlocks = viewModel.BaseNumberOfBlocks + 1;
				if (viewModel.IsEndCombo)
					viewModel.Combo = 0;
				viewModel.IsEndCombo = true;
			}
			for (int i = 0; i < viewModel.NumberOfBlocks - 1; i++)
			{
				viewModel.ToChoose.Add(new Block(viewModel.BlocksBoard, i));
			}
			viewModel.NumberOfBlocks -= 1;
		}
		if (canGenerateNewBlocks && (viewModel.GameMode == GameModes.ChaoticBoard || viewModel.GameMode == GameModes.MaxChaotic))
			IsGetUserScore(viewModel.BlocksBoard);
		if (await IsUserLose())
		{
			loseButton.IsVisible = true;
		}
	}
	public async Task<bool> IsPlaceOnBlock(int blocksViewPosition)
	{
		int i = blocksViewPosition;
		Grid grid = (Grid)blocksView.Children[i];
		if (grid.IsVisible)
			for (int j = 0, k = 0; j < viewModel.BlocksBoard.Fields.GetLength(1); k++)
			{

				if (IsFreeSpaceOnBlock(k, j, viewModel.ToChoose[i]))
				{
					return true;
				}
				else
				{

				}

				if (k + 1 == viewModel.BlocksBoard.Fields.GetLength(0))
				{
					k = -1;
					j++;
				}
			}
		return false;
	}
	public async Task<bool> IsUserLose()
	{
		int unVisableElements = 0;
		for (int i = 0; i < viewModel.ToChoose.Count; i++)
			if (blocksView.Children[i] is Grid)
			{

				if (await IsPlaceOnBlock(i))
				{
					return false;
				}
			}
			else
			{
				unVisableElements++;
			}

		return true;

	}
	public async void LoseButtonClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}

	#endregion

	public static Position GetGridXY(Grid layout)
	{
		return new Position
		{
			X = layout.X,
			Y = layout.Y,
		};
	}

	#region UserScore
	private void IsGetUserScore(object blocksBoard)
	{
		BlocksBoard b = blocksBoard as BlocksBoard;
		IList<int> completedRows = CheckCompletedRows(b);
		IList<int> completedColumns = CheckCompletedColumns(b);
		foreach (int column in completedColumns)
		{
			for (int i = 0; i < b.Fields.GetLength(0); i++)
			{
				b.Fields[i, column].IsFree = true;
				b.Fields[i, column].BackgroundColor = Field.DefaultColor;
			}
		}
		foreach (int row in completedRows)
		{
			for (int i = 0; i < b.Fields.GetLength(1); i++)
			{
				b.Fields[row, i].IsFree = true;
				b.Fields[row, i].BackgroundColor = Field.DefaultColor;
			}
		}
		SumScore(completedRows.Count, completedColumns.Count, b);
	}

	private void SumScore(int numberOfCompletedRows, int numberOfCompletedColumns, BlocksBoard board)
	{
		int score = 0;
		//if (numberOfCompletedRows != 0 && numberOfCompletedColumns != 0)
		//	score += (int)Global.Impressivenes((int)viewModel.BlocksBoard.Width, numberOfCompletedRows)
		//		* (int)Global.Impressivenes(viewModel.BlocksBoard.Height, numberOfCompletedColumns);
		//else if (numberOfCompletedRows != 0)
		//	score += (int)Global.Impressivenes((int)viewModel.BlocksBoard.Width, numberOfCompletedRows);
		//else if (numberOfCompletedColumns != 0)
		//	score += (int)Global.Impressivenes(viewModel.BlocksBoard.Height, numberOfCompletedColumns);

		if (numberOfCompletedRows != 0 && numberOfCompletedColumns != 0)
			score += ((int)viewModel.BlocksBoard.Width + numberOfCompletedRows * 10) * numberOfCompletedRows * 10
				+ ((int)viewModel.BlocksBoard.Height + numberOfCompletedColumns * 10) * numberOfCompletedColumns * 10;
		else if (numberOfCompletedRows != 0)
			score += ((int)viewModel.BlocksBoard.Width + numberOfCompletedRows * 10) * numberOfCompletedRows * 10;
		else if (numberOfCompletedColumns != 0)
			score += ((int)viewModel.BlocksBoard.Height + numberOfCompletedColumns * 10) * numberOfCompletedColumns * 10;
		if (viewModel.BlocksBoard.IsClear())
		{
			score = (int)score * 2;
		}
		if (score > 0)
		{
			viewModel.Score += score += viewModel.Combo * 10;
			viewModel.IsEndCombo = false;
			viewModel.Combo++;
		}
		if (score != 0)
		{
			viewModel.NewScore = score;
			newScoreLabel.Opacity = 1;
			newScoreLabel.FadeTo(0, 1500);
		}
		if (viewModel.Score > MainPageView.TheBestScore)
		{
			viewModel.ScoreStr = "New the best score";
			MainPageView.TheBestScore = viewModel.Score;
			MainPageView.ChangeTheBestScoreInFile(viewModel.BaseWidth, viewModel.BaseHeigh, viewModel.BaseNumberOfBlocks, viewModel.GameMode, viewModel.Score);
		}
	}
	#endregion

	#region CheckCompletedPositions
	private static IList<int> CheckCompletedColumns(BlocksBoard b)
	{
		IList<int> completedRows = new List<int>();
		for (int i = 0, j = 0; i < b.Width; j++)
		{
			if (b.Fields[j, i].IsFree)
			{
				i++;
				j = -1;
				continue;
			}
			if (j + 1 == b.Height)
			{
				completedRows.Add(i);
				i++;
				j = -1;
			}
		}
		return completedRows;
	}
	public static IList<int> CheckCompletedColumns(BlocksBoard b, Color DefaultColor)
	{
		IList<int> completedRows = new List<int>();
		for (int i = 0, j = 0; i < b.Width; j++)
		{
			if (b.Fields[j, i].BackgroundColor == DefaultColor)
			{
				i++;
				j = -1;
				continue;
			}
			if (j + 1 == b.Height)
			{
				completedRows.Add(i);
				i++;
				j = -1;
			}
		}
		return completedRows;
	}
	private static IList<int> CheckCompletedRows(BlocksBoard b)
	{
		IList<int> completedColumns = new List<int>();
		for (int i = 0, j = 0; j < b.Height; i++)
		{
			if (b.Fields[j, i].IsFree)
			{
				j++;
				i = -1;
				continue;
			}
			if (i + 1 == b.Width)
			{
				completedColumns.Add(j);
				j++;
				i = -1;
			}
		}
		return completedColumns;
	}
	public static IList<int> CheckCompletedRows(BlocksBoard b, Color DefaultColor)
	{
		IList<int> completedColumns = new List<int>();
		for (int i = 0, j = 0; j < b.Height; i++)
		{
			if (b.Fields[j, i].BackgroundColor == DefaultColor)
			{
				j++;
				i = -1;
				continue;
			}
			if (i + 1 == b.Width)
			{
				completedColumns.Add(j);
				j++;
				i = -1;
			}
		}
		return completedColumns;
	}
	#endregion
}
