//using Microsoft.UI.Xaml.Documents;

using Microsoft.Maui.Controls.PlatformConfiguration.GTKSpecific;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Layouts;


namespace BlocksGame;

public partial class GamePage : ContentPage
{
    Grid Board;
    Thread MenageToChooseThread;
    Grid Blocks = new Grid
    {
        ColumnSpacing = 20,
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.Center,
        Padding = 10,
    };
    AbsoluteLayout layout = new AbsoluteLayout();
    BlocksBoard b;
    public static IList<Block> ToChoose { get; set; } = [];

    private static readonly object lockObject = new object();
    DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;
    static double screenWidth;
    static double screenHeight;
    public GamePage(uint width , uint height)
    {
        screenWidth = displayInfo.Width;
        screenHeight = displayInfo.Height;

        b = new BlocksBoard(width, height);
        InitializeComponent();
        layout = MainAbsoluteLayout;
        Board = b.CreateGrid();
        VerticalStackLayout.Children.Add(Board);
        for (int i = 0; i < 3; i++)
        {
            ToChoose.Add(Block.GenerateBlock(b));
        }
        board = Board;
        blocks = Blocks;
        Global.CreateGrid(Blocks, 3, 1);
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
        while(ToChoose[0].GridX + ToChoose[0].GridY <= 0)
        for (int i = 0; i < ToChoose.Count; i++)
        {
            ToChoose[i].GridX = Blocks.X;
            ToChoose[i].GridY = Blocks.Y;
        }
    }
    public static void MenageToChoose(Grid layout, BlocksBoard b)
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
            return;
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
    private static void IsGetUserScore(BlocksBoard b)
    {
        IList<int> completedRows = CheckCompletedRows(b);
        IList<int> completedColumns = CheckCompletedColumns(b);
        foreach (int row in completedRows)
        {
            for (int i = 0; i < b.fields.GetLength(0); i++)
            {
                b.fields[i, row].IsFree = true;
                b.fields[i, row].BackgroundColor = Color.FromRgb(255, 0, 0);
            }
        }
        foreach (int column in completedColumns)
        {
            for (int i = 0; i < b.fields.GetLength(1); i++)
            {
                b.fields[column, i].IsFree = true;
                b.fields[column, i].BackgroundColor = Color.FromRgb(255, 0, 0);
            }
        }
    }
    private static bool IsUserLose(BlocksBoard b)
    {
        for (int i = 0; i < ToChoose.Count; i++)
            for (int j = 0, k = 0; j < b.fields.GetLength(1); k++)
            {
                if (ToChoose[i].IsFreeSpaceOnBlock(k, j))
                {
                    return false;
                }
                if (k + 1 == b.fields.GetLength(0))
                {
                    k = -1;
                    j++;
                }
            }
        return true;
    }
    private static IList<int> CheckCompletedColumns(BlocksBoard b)
    {
        IList<int> completedRows = new List<int>();
        for (int i = 0, j = 0; i < b.Width; j++)
        {
            if (b.fields[i, j].IsFree)
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
            if (b.fields[i, j].IsFree)
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