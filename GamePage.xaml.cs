//using Microsoft.UI.Xaml.Documents;

using Microsoft.Maui.Controls.PlatformConfiguration.GTKSpecific;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Layouts;
using System.Data.Common;


namespace BlocksGame;

public partial class GamePage : ContentPage
{
    Grid Board;
    Thread MenageToChooseThread;
    Grid Blocks = new Grid
    {
        ColumnSpacing = 20,
        HorizontalOptions = LayoutOptions.Center,
        Padding = 10,
        BackgroundColor = Color.FromRgb(0, 0, 255),
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
    public string ScoreStr
    {
        get => "Score: " + score;
    }
    public GamePage(uint width, uint height, uint numberOfBlocksToChoose)
    {
        Global.GamePage = this;
        b = new BlocksBoard(width, height);
        InitializeComponent();
        ToChoose = new List<Block>();
        Board = b.CreateGrid();
        VerticalStackLayout.Children.Add(Board);
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
        VerticalStackLayout.Children.Add(Blocks);
        MenageToChooseThread = new Thread(new ThreadStart(AddXToBlocks));
        MenageToChooseThread.Start();
    }
    private void AddXToBlocks()
    {
        while (ToChoose[0].GridX <= 0 || ToChoose[0].GridY <= 0)
            for (int i = 0; i < ToChoose.Count; i++)
            {
                ToChoose[i].GridX = Blocks.X;
                ToChoose[i].GridY = Blocks.Y;
            }
    }
    public async void MenageToChoose(Grid layout, BlocksBoard b)
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
        if (IsUserLose(b))
        {
            await Navigation.PopAsync();
        }

        if (BlocksToChoose == 0)
        {

            for (int i = 0; i < ToChoose.Count; i++)
            {
                ToChoose[i] = Block.GenerateBlock(b);
            }
            for (int i = 0; i < ToChoose.Count; i++)
            {

                ToChoose[i].ShowBlock(i, 1, layout);
                ToChoose[i].GridX = layout.X;
                ToChoose[i].GridY = layout.Y;
            }
        }

    }
    public static void ChangeGridXY(Grid layout)
    {
        for (int i = 0; i < ToChoose.Count; i++)
        {
            ToChoose[i].GridX = layout.X;
            ToChoose[i].GridY = layout.Y;
        }
    }
    private void IsGetUserScore(BlocksBoard b)
    {
        IList<int>  completedRows = CheckCompletedRows(b);
        IList<int> completedolumns =  CheckCompletedColumns(b);
        foreach (int column in completedolumns)
        {
            for (int i = 0; i < b.fields.GetLength(0); i++)
            {
                b.fields[i,column].IsFree = true;
                b.fields[i,column].BackgroundColor = Color.FromRgb(255, 0, 0);
            }
        }
        foreach (int row in completedRows)
        {
            for (int i = 0; i < b.fields.GetLength(1); i++)
            {
                b.fields[row,i].IsFree = true;
                b.fields[row,i].BackgroundColor = Color.FromRgb(255, 0, 0);
            }
        }

        if (completedRows.Count != 0 && completedolumns.Count != 0)
            Score += (int)Global.Impressivenes((int)b.Width, completedRows.Count) * (int)Global.Impressivenes(b.Height, completedolumns.Count);
        else if (completedRows.Count != 0)
            Score += (int)Global.Impressivenes((int)b.Width, completedRows.Count);
        else if (completedolumns.Count != 0)
            Score += (int)Global.Impressivenes(b.Height, completedolumns.Count);

        if (Score > MainPageView.TheBestScore)
        {
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
        if (unVisableElements != ToChoose.Count)
            return true;
        else
        {
            return false;
        }
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