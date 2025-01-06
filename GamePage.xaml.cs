//using Microsoft.UI.Xaml.Documents;

using Microsoft.Maui.Controls.PlatformConfiguration.GTKSpecific;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Layouts;
using System.Data.Common;
using System.Xml.Linq;


namespace BlocksGame;
public struct Position
{
    public double X { get; set; }
    public double Y { get; set; }
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
    public  GamePage(uint width, uint height, uint numberOfBlocksToChoose)
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
        VerticalStackLayout.Children.Add(Board);
        VerticalStackLayout.Children.Add(Blocks);
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
        BlocksBoard b  = blocksBoard as BlocksBoard;
        IList<int> completedRows = CheckCompletedRows(b);
        IList<int> completedColumns = CheckCompletedColumns(b);
        foreach (int column in completedColumns)
        {
            for (int i = 0; i < b.fields.GetLength(0); i++)
            {
                b.fields[i, column].IsFree = true;
                b.fields[i, column].BackgroundColor = Color.FromRgb(255, 0, 0);
            }
        }
        foreach (int row in completedRows)
        {
            for (int i = 0; i < b.fields.GetLength(1); i++)
            {
                b.fields[row, i].IsFree = true;
                b.fields[row, i].BackgroundColor = Color.FromRgb(255, 0, 0);
            }
        }
        SumScore(completedRows.Count, completedColumns.Count);
    }
    private void SumScore(int numberOfCompletedRows, int numberOfCompletedColumns)
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
            MainPageView.ChangeTheBestScoreInFile(Score);
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
    private static IList<int> CheckCompletedColumns(BlocksBoard b)
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
    private static IList<int> CheckCompletedRows(BlocksBoard b)
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
    public async void NewButton(object sender, EventArgs e)
    {
        int i = 0;
    }
}