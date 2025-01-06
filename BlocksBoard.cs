using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksGame
{
    public class BlocksBoard : BindableObject
    {
        double MinimumWidthRequest = 30;
        double MinimumHeightRequest = 30;
        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public Field[,] fields;
        public Grid BoardView { get; private set; }
        public BlocksBoard(uint width, uint height)
        {
            this.Width = width;
            this.Height = height;
            fields = new Field[height, width];
        }
        public void CreateGrid()
        {
            var grid = new Grid { /*Margin = 10, Padding = 10 ,*/HorizontalOptions = LayoutOptions.Center };
            Global.CreateGrid(grid, Width, Height);
            for (int i = 0, j = 0; j < this.Width; i++)
            {
                fields[i, j] = new Field();
                var label = new Label
                {
                    BindingContext = fields[i, j],
                    //MinimumWidthRequest = MinimumWidthRequest,
                    //MinimumHeightRequest = MinimumHeightRequest,
                };
                label.SetBinding(Label.BackgroundColorProperty, new Binding("BackgroundColor"));
                var bound = new Label
                {
                    BackgroundColor = Color.FromRgb(0, 0, 0),
                    WidthRequest = 1,
                    HeightRequest = MinimumHeightRequest,
                    HorizontalOptions = LayoutOptions.Start,
                };
                var downBound = new Label
                {
                    BackgroundColor = Color.FromRgb(0, 0, 0),
                    WidthRequest = MinimumWidthRequest,
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                };
                grid.Children.Add(label);
                grid.SetColumn(label, j);
                grid.SetRow(label, i);

                grid.Children.Add(bound);
                grid.SetColumn(bound, j);
                grid.SetRow(bound, i);

                grid.Children.Add(downBound);
                grid.SetColumn(downBound, j);
                grid.SetRow(downBound, i);

                if (i == this.Height - 1)
                {
                    i = -1;
                    j++;
                }

            }
            BoardView = grid;
        }

        public bool IsClear()
        {
            for (int i = 0; i < fields.GetLength(0); i++)
            {
                for (int j = 0; j < fields.GetLength(1); j++)
                {
                    if(!fields[i, j].IsFree)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
