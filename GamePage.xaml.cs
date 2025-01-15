//using Microsoft.UI.Xaml.Documents;

using Microsoft.Maui.Controls.PlatformConfiguration.GTKSpecific;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Layouts;
using System.Data.Common;
using System.Xml.Linq;


namespace BlocksGame;
public struct Position
{
    public Position(double x, double y)
    {
        X = x; Y = y;
    }
    public double X { get; set; }
    public double Y { get; set; }
    public static bool operator ==(Position left, Position right)
    {
        if (left.X == right.X && left.Y == right.Y)
            return true;
        else
            return false;
    }
    public static bool operator !=(Position left, Position right)
    {
        if (left.X != right.X || left.Y != right.Y)
            return true;
        else
            return false;
    }
}
public partial class GamePage : ContentPage
{
    Thread UserScoreThread;
    Grid Board;
    Grid Blocks = new Grid
    {
        ColumnSpacing = 20,
        HorizontalOptions = LayoutOptions.Center,
        Padding = 10,
    };
    AbsoluteLayout layout = new AbsoluteLayout();
    BlocksBoard b;
    public static IList<Block> ToChoose { get; set; } = [];
    public static IList<Block> NextToChoose { get; set; } = [];

    private static readonly object lockObject = new object();
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
    public GamePage(uint width, uint height, uint numberOfBlocksToChoose)
    {
        Score = 0;
        Global.GamePage = this;
        b = new BlocksBoard(width, height);
        InitializeComponent();
        ToChoose = new List<Block>();
        Thread createBoard = new Thread(new ThreadStart(b.CreateGrid));
        createBoard.Start();

        for (int i = 0; i < numberOfBlocksToChoose; i++)
        {
            ToChoose.Add(Block.GenerateBlock(b));
        }
        //board = Board;
        //blocks = Blocks;
        Global.CreateGrid(Blocks, numberOfBlocksToChoose, 1);
        for (int i = 0; i < ToChoose.Count; i++)
        {
            ToChoose[i].ShowBlock(i, 0, Blocks);
        }
        createBoard.Join();
        Board = b.BoardView;
        MainStackLayout.Children.Add(Board);
        MainStackLayout.Children.Add(Blocks);
    }
    /// <summary>
    /// Check that player lose and give score
    /// </summary>
    /// <param name="layout"></param>
    /// <param name="b"></param>
    public void MenageToChoose(Grid layout, BlocksBoard b)
    {
        int beginBlocksToChoose = ToChoose.Count;
        int BlocksToChoose = 0;
        for (int i = 0; i < ToChoose.Count; i++)
        {
            if (ToChoose[i].BlockView.IsVisible)
                BlocksToChoose++;
        }

        beginBlocksToChoose = BlocksToChoose;

        IsGetUserScore(b);
        if (BlocksToChoose == 0)
        {

            for (int i = 0; i < ToChoose.Count; i++)
            {
                ToChoose[i] = Block.GenerateBlock(b);
            }
            for (int i = 0; i < ToChoose.Count; i++)
            {

                ToChoose[i].ShowBlock(i, 1, layout);

            }
        }

        if (IsUserLose(b))
        {
            loseButton.IsVisible = true;
        }
    }
    public async void LoseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    public static Position GetGridXY(Grid layout)
    {
        return new Position
        {
            X = layout.X,
            Y = layout.Y,
        };
    }
    private void IsGetUserScore(object blocksBoard)
    {
        BlocksBoard b = blocksBoard as BlocksBoard;
        IList<int> completedRows = CheckCompletedRows(b);
        IList<int> completedColumns = CheckCompletedColumns(b);
        ChangeCompletedPositions(b, Field.DefaultColor, true, completedRows, completedColumns);
        SumScore(completedRows.Count, completedColumns.Count, b);
    }

    private void SumScore(int numberOfCompletedRows, int numberOfCompletedColumns, BlocksBoard board)
    {
        int score = 0;
        if (numberOfCompletedRows != 0 && numberOfCompletedColumns != 0)
            score += (int)Global.Impressivenes((int)b.Width, numberOfCompletedRows) * (int)Global.Impressivenes(b.Height, numberOfCompletedColumns);
        else if (numberOfCompletedRows != 0)
            score += (int)Global.Impressivenes((int)b.Width, numberOfCompletedRows);
        else if (numberOfCompletedColumns != 0)
            score += (int)Global.Impressivenes(b.Height, numberOfCompletedColumns);
        if (b.IsClear())
        {
            Score += (int)Global.Impressivenes(score, 2);
        }
        else
            Score += score;
        if (Score > MainPageView.TheBestScore)
        {
            ScoreStr = "New The Best Score :";
            MainPageView.TheBestScore = Score;
            MainPageView.ChangeTheBestScoreInFile((int)board.Width, (int)board.Height, ToChoose.Count, Score);
        }
    }
    public static bool IsUserLose(BlocksBoard b)
    {
        int unVisableElements = 0;
        for (int i = 0; i < ToChoose.Count; i++)
            if (ToChoose[i].BlockView.IsVisible)
                for (int j = 0, k = 0; j < b.fields.GetLength(1); k++)
                {

                    if (ToChoose[i].IsFreeSpaceOnBlock(k, j))
                    {
                        return false;
                    }
                    else
                    {

                    }

                    if (k + 1 == b.fields.GetLength(0))
                    {
                        k = -1;
                        j++;
                    }
                }
            else
            {
                unVisableElements++;
            }

        return true;

    }
    #region CompletedColumnsAndRows
    /// <summary>
    /// Changing isFree and Color in completed positions in board
    /// </summary>
    /// <param name="b"></param>
    /// <param name="newColor"> new Color when position changed</param>
    /// <param name="isFree"></param>
    public static void ChangeCompletedPositions(BlocksBoard b,Color newColor,bool isFree, IList<int> completedRows, IList<int> completedColumns)
    {
        foreach (int column in completedColumns)
        {
            for (int i = 0; i < b.fields.GetLength(0); i++)
            {
                b.fields[i, column].IsFree = isFree;
                b.fields[i, column].BackgroundColor = newColor ;
            }
        }
        foreach (int row in completedRows)
        {
            for (int i = 0; i < b.fields.GetLength(1); i++)
            {
                b.fields[row, i].IsFree = isFree;
                b.fields[row, i].BackgroundColor = newColor;
            }
        }
    }
    public static void ChangeCompletedPositions(BlocksBoard b, Color newColor, IList<int> completedRows, IList<int> completedColumns)
    {
        foreach (int column in completedColumns)
        {
            for (int i = 0; i < b.fields.GetLength(0); i++)
            {
                b.fields[i, column].BackgroundColor = newColor;
                //object value = await b.fields[i, column].Label.ColorTo(b.fields[i, column].BackgroundColor, newColor, c => b.fields[i, column].Label.BackgroundColor = c, 10);
            }
        }
        foreach (int row in completedRows)
        {
            for (int i = 0; i < b.fields.GetLength(1); i++)
            {
                b.fields[row, i].BackgroundColor = newColor;
                //object value = await b.fields[row, i].Label.ColorTo(b.fields[row, i].BackgroundColor, newColor, c => b.fields[row, i].Label.BackgroundColor = c, 10);
            }
        }
    }
    /// <summary>
    /// Check is columns completed
    /// </summary>
    /// <param name="b"></param>
    /// <returns>List of columns positions</returns>
    public static IList<int> CheckCompletedColumns(BlocksBoard b)
    {
        IList<int> completedRows = new List<int>();
        for (int i = 0, j = 0; i < b.Width; j++)
        {
            if (b.fields[j, i].IsFree)
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
    public static IList<int> CheckCompletedColumns(BlocksBoard b,Color DefaultColor)
    {
        IList<int> completedRows = new List<int>();
        for (int i = 0, j = 0; i < b.Width; j++)
        {
            if (b.fields[j, i].BackgroundColor == DefaultColor)
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
    /// <summary>
    /// Check is row completed
    /// </summary>
    /// <param name="b"></param>
    /// <returns>List of rows positions</returns>
    public static IList<int> CheckCompletedRows(BlocksBoard b)
    {
        IList<int> completedColumns = new List<int>();
        for (int i = 0, j = 0; j < b.Height; i++)
        {
            if (b.fields[j, i].IsFree)
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
    public static IList<int> CheckCompletedRows(BlocksBoard b,Color DefaultColor)
    {
        IList<int> completedColumns = new List<int>();
        for (int i = 0, j = 0; j < b.Height; i++)
        {
            if (b.fields[j, i].BackgroundColor == DefaultColor)
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
    public async void NewButton(object sender, EventArgs e)
    {
        int i = 0;
    }
}