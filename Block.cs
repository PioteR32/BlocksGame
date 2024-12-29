//using Microsoft.UI.Xaml;
//using GameplayKit;
using Microsoft.Maui;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksGame
{
    public class Block
    {
        public Block(bool[,] BlockStructure, BlocksBoard BlocksBoard)
        {
            this.BlockStructure = BlockStructure;
            this.blocksBoard = BlocksBoard;
            this.BlockStructure = CutStructure(ChangeTable2DToIList(BlockStructure));
            BlockView = CreateBlock();
        }
        public Image Image;
        public Color BackgroundColor = Color.FromRgb(0, 0, 0);
        BlocksBoard blocksBoard;
        public double GridX = 0;
        public double GridY = 0;
        public Grid BlockView { get; set; }
        /// <summary>
        /// Structure Of Block 
        /// true = 1 = have block in this position
        /// false = 0 = dont have block
        /// </summary>
        bool[,] BlockStructure { get; set; } = new bool[9, 9];
        bool isInGrid = true;
        public bool[,] CutStructure(IList<List<bool>> structure)
        {
            for(int i = 0; i < structure.Count;i++)
            {
                bool isToDelete = true;
                for(int j = 0; j < structure[i].Count;j++)
                {
                    if(structure[i][j])
                    {
                        isToDelete = false;
                        break;
                    }
                }
                if(isToDelete)
                {
                    structure.Remove(structure[i]);
                    i--;
                }
            }

            for (int j = 0; j < structure[0].Count; j++)
            {
                bool isToDelete = true;
                for (int i = 0; i < structure.Count; i++)
                {
                    if (structure[i][j])
                    {
                        isToDelete = false;
                        break;
                    }
                }
                if (isToDelete)
                {
                    for(int i = 0;i < structure.Count;i++)
                    {
                        structure[i].Remove(structure[i][j]);
                    }
                    j--;
                }
            }
            return ChangeIListToTable2D(structure); 
        }
        public static T[,] ChangeIListToTable2D<T>(IList<List<T>> list)
        {
            T[,] tmp = new T[list.Count, list[0].Count];
            for(int i = 0; i < list.Count; i++)
            {
                for(int j = 0; j < list[i].Count;j++)
                {
                    tmp[i,j] = list[i][j];
                }
            }
            return tmp;
        }
        public static IList<List<T>> ChangeTable2DToIList<T>(T[,] table)
        {
            IList<List<T>> list = (IList < List < T >> )new List<List<T>>();

            for (int i = 0; i < table.GetLength(0); i++)
            {
                list.Add(new List<T>());
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    list[i].Add(table[i,j]);
                }
            }
            return list;
        }
      
        public Grid CreateBlock()
        {
            var grid = new Grid();
            Global.CreateGrid(grid, (uint)BlockStructure.GetLength(1), (uint)BlockStructure.GetLength(0));
            for (int i = 0, j = 0; j < BlockStructure.GetLength(1); i++)
            {
                Field tmp;
                if (BlockStructure[i, j])
                {
                    tmp = new Field(BackgroundColor, false);
                }
                else
                    tmp = new Field(Color.FromRgba(255, 0, 0, 100), true);
                var label = new Label
                {
                    BindingContext = tmp,
                    BackgroundColor = tmp.BackgroundColor,
                    MinimumWidthRequest = 10,
                    MinimumHeightRequest = 10,
                };
                grid.Children.Add(label);
                grid.SetColumn(label, j);
                grid.SetRow(label, i);


                var bound = new Label
                {
                    BindingContext = tmp,
                    BackgroundColor = Color.FromRgb(255, 0, 0),
                    WidthRequest = 1,
                    HeightRequest = 10,
                    HorizontalOptions = LayoutOptions.Start,
                };

                bound.SetBinding(Label.HeightRequestProperty, new Binding("Height"));
                var downBound = new Label
                {
                    BindingContext = tmp,
                    BackgroundColor = Color.FromRgb(255, 0, 0),
                    WidthRequest = 10,
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                };
                downBound.SetBinding(Label.WidthRequestProperty, new Binding("Width"));
                grid.Children.Add(bound);
                grid.SetColumn(bound, j);
                grid.SetRow(bound, i);

                grid.Children.Add(downBound);
                grid.SetColumn(downBound, j);
                grid.SetRow(downBound, i);

                if (i == BlockStructure.GetLength(0) - 1)
                {
                    i = -1;
                    j++;
                }

            }
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += async (s, e) =>
            {
                PanGetsureFnc(e, grid);
            }; grid.GestureRecognizers.Add(panGesture);
            return grid;
        }
        public Grid CreateBlock(double widthRequest, double heightRequest)
        {
            var grid = new Grid();
            Global.CreateGrid(grid, (uint)BlockStructure.GetLength(0), (uint)BlockStructure.GetLength(1));
            for (int i = 0, j = 0; j < BlockStructure.GetLength(1); i++)
            {
                Field tmp;
                if (BlockStructure[i, j])
                {
                    tmp = new Field(BackgroundColor, false);

                }
                else
                    tmp = new Field(Color.FromRgba(255, 0, 0, 100), true);
                var label = new Label
                {
                    BindingContext = tmp,
                    BackgroundColor = tmp.BackgroundColor,
                    MinimumWidthRequest = widthRequest,
                    MinimumHeightRequest = heightRequest,
                };
                grid.Children.Add(label);
                grid.SetColumn(label, j);
                grid.SetRow(label, i);


                var bound = new Label
                {
                    BindingContext = tmp,
                    BackgroundColor = Color.FromRgb(255, 0, 0),
                    WidthRequest = 1,
                    HeightRequest = 10,
                    HorizontalOptions = LayoutOptions.Start,
                };

                bound.SetBinding(Label.HeightRequestProperty, new Binding("Height"));
                var downBound = new Label
                {
                    BindingContext = tmp,
                    BackgroundColor = Color.FromRgb(255, 0, 0),
                    WidthRequest = 10,
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                };
                downBound.SetBinding(Label.WidthRequestProperty, new Binding("Width"));
                grid.Children.Add(bound);
                grid.SetColumn(bound, j);
                grid.SetRow(bound, i);

                grid.Children.Add(downBound);
                grid.SetColumn(downBound, j);
                grid.SetRow(downBound, i);

                if (i == BlockStructure.GetLength(0) - 1)
                {
                    i = -1;
                    j++;
                }

            }
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += async (s, e) =>
            {
                switch (e.StatusType)
                {
                    case GestureStatus.Running:

                        grid.TranslationX = e.TotalX; grid.TranslationY = e.TotalY;

                        break;
                    case GestureStatus.Completed:
                        if (!AddBlockToBoard())
                            await grid.TranslateTo(e.TotalX, e.TotalY);
                        else
                        {
                            BlockView.IsVisible = false;
                            Global.GamePage.MenageToChoose((Grid)BlockView.Parent, blocksBoard);
                        }
                        break;
                    case GestureStatus.Canceled:
                        if (!AddBlockToBoard())
                            await grid.TranslateTo(e.TotalX, e.TotalY);
                        else
                            BlockView.IsVisible = false;
                        break;
                }
            }; grid.GestureRecognizers.Add(panGesture);
            return grid;
        }
        private async void PanGetsureFnc(PanUpdatedEventArgs e,Grid grid)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:

                    grid.TranslationX = e.TotalX; grid.TranslationY = e.TotalY;
                    //if (grid.Children[0].Width != blocksBoard.BoardView.Width / blocksBoard.Width)
                    //{
                    //    var gridv2 = CreateBlock(blocksBoard.BoardView.Width / blocksBoard.Width, blocksBoard.BoardView.Height / blocksBoard.Height);

                    //    grid = gridv2;
                    //    // Aktualizacja referencji na nowy grid grid = gridv2;
                    //}
                    break;
                case GestureStatus.Completed:
                    if (!AddBlockToBoard())
                    {
                        await grid.TranslateTo(e.TotalX, e.TotalY);
                        if (GamePage.IsUserLose(blocksBoard))
                        {
                            return;
                        }
                    }
                    else
                    {
                        BlockView.IsVisible = false;
                        Global.GamePage.MenageToChoose((Grid)BlockView.Parent, blocksBoard);
                        if (GamePage.IsUserLose(blocksBoard))
                        {
                            return;
                        }
                    }
                    break;
                case GestureStatus.Canceled:
                    if (!AddBlockToBoard())
                        await grid.TranslateTo(e.TotalX, e.TotalY);
                    else
                    {
                        BlockView.IsVisible = false;
                         Global.GamePage.MenageToChoose((Grid)BlockView.Parent, blocksBoard);
                        if (GamePage.IsUserLose(blocksBoard))
                        {
                            return;
                        }
                    }
                    break;
            }
        }

        private bool AddBlockToBoard()
        {
            var boardGrid = blocksBoard.BoardView;
            double boardColumnWidth = boardGrid.Width / blocksBoard.Width;
            double boardColumnHeight = boardGrid.Height / blocksBoard.Height;
            IList<PositionsInTable> positions = new List<PositionsInTable>();
            for (int i = 0, j = 0; j < blocksBoard.Height; i += 1)
            {
                if (BlockView.X + GridX + BlockView.TranslationX > i * boardColumnWidth + boardGrid.X && BlockView.X + GridX + BlockView.TranslationX < (i + 1) * boardColumnWidth + boardGrid.X
                    && BlockView.Y + GridY + BlockView.TranslationY > j * boardColumnHeight + boardGrid.Y && BlockView.Y + GridY  + BlockView.TranslationY < (j + 1) * boardColumnHeight + boardGrid.Y)
                {
                    if (IsFreeSpaceOnBlock(i, j, boardColumnWidth, boardColumnHeight, positions))
                    {
                        foreach (var position in positions)
                        {
                            blocksBoard.fields[position.y, position.x ].BackgroundColor = BackgroundColor;
                            blocksBoard.fields[position.y, position.x].IsFree = false;
                        }
                        return true;
                    }
                }
                if (i + 1 == blocksBoard.Width)
                {
                    i = -1;
                    j++;
                }
            }
            return false;
        }
        public struct PositionsInTable
        {
            public PositionsInTable(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public int x, y;
        }
        private bool IsFreeSpaceOnBlock(int x, int y, double boardColumnWidth, double boardColumnHeight, IList<PositionsInTable> positions)
        {

            PositionsInTable firstVisableBlock = new PositionsInTable(0, 0);
            bool isFirstVisableBlock = false;
            for (int i = 0, j = 0; j < BlockStructure.GetLength(0); i++)
            {
                if (BlockStructure[j, i])
                {
                   
                    if (!IsPositionInGrid(j + y, i + x, positions))
                    {
                        return false;
                    }
                }
                if (i + 1 == BlockStructure.GetLength(1))
                {
                    i = -1;
                    j++;
                }
            }
            return true;
        }
        public bool IsFreeSpaceOnBlock(int x, int y)
        {
            var boardGrid = blocksBoard.BoardView;
            double boardColumnWidth = boardGrid.Width / blocksBoard.Width;
            double boardColumnHeight = boardGrid.Height / blocksBoard.Height;
            PositionsInTable firstVisableBlock = new PositionsInTable(0, 0);
            bool isFirstVisableBlock = false;
            for (int i = 0, j = 0; j < BlockStructure.GetLength(1); i++)
            {
                if (BlockStructure[i, j])
                {
                    if (!isFirstVisableBlock)
                    {
                        firstVisableBlock = new PositionsInTable(i, j);
                        isFirstVisableBlock = true;
                    }
                    if (!IsPositionInGrid(i + x - firstVisableBlock.x, j + y - firstVisableBlock.y))
                    {
                        return false;
                    }
                }
                if (i + 1 == BlockStructure.GetLength(0))
                {
                    i = -1;
                    j++;
                }
            }
            return true;
        }
        private bool IsPositionInGrid(int columnPosition, int rowPosition, IList<PositionsInTable> positions)
        {
            try
            {
                if (blocksBoard.fields[rowPosition,columnPosition ].IsFree)
                    foreach (IView view in blocksBoard.BoardView.Children)
                    {

                        if (blocksBoard.BoardView.GetColumn(view) == columnPosition && blocksBoard.BoardView.GetRow(view) == rowPosition)
                        {
                            positions.Add(new PositionsInTable(columnPosition, rowPosition));
                            return true;
                        }

                    }
            }
            catch
            {
                return false;
            }
            return false;
        }
        private bool IsPositionInGrid(int columnPosition, int rowPosition)
        {
            try
            {
                if (blocksBoard.fields[rowPosition, columnPosition].IsFree)
                    foreach (IView view in blocksBoard.BoardView.Children)
                    {

                        if (blocksBoard.BoardView.GetColumn(view) == columnPosition && blocksBoard.BoardView.GetRow(view) == rowPosition)
                        {
                            return true;
                        }

                    }
            }
            catch
            {
                return false;
            }
            return false;
        }
        public void ShowBlock(int x, int y, Grid views)
        {

            views.Children.Add(this.BlockView);
            views.SetColumn(this.BlockView, x);
            views.SetRow(this.BlockView, y);
        }
        public static Block GenerateBlock(BlocksBoard blocksBoard)
        {
            Random random = new Random();
            int numberOfBlocks = random.Next(1, (int)blocksBoard.Width * (int)blocksBoard.Height / 2 - 1);

            return new Block(GenerateBlockStructure(blocksBoard, numberOfBlocks), blocksBoard);
        }
        public static bool[,] GenerateBlockStructure(BlocksBoard blocksBoard, int numberOfBlocks)
        {
            Random random = new Random();
            bool[,] tmp = new bool[blocksBoard.Width, blocksBoard.Height];

            for (int i = 0; i < numberOfBlocks; i++)
            {
                int x = random.Next((int)blocksBoard.Width);
                int y = random.Next((int)blocksBoard.Height);
                if (!tmp[x, y])
                {
                    tmp[x, y] = true;
                }
                else
                    i--;
            }
            return tmp;
        }
    }
}
