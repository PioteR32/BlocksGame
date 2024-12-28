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
        public  Field[,] fields;
        public Grid BoardView { get; private set; }
        public BlocksBoard(uint width,uint height)
        {
            this.Width = width;
            this.Height = height;
            fields = new Field[width, height];
        }
        public Grid CreateGrid()
        {
            var grid = new Grid { /*Margin = 10, Padding = 10 ,*/HorizontalOptions = LayoutOptions.Center};
            Global.CreateGrid(grid, Width, Height);
            for (int i = 0, j = 0; j < this.Height; i++)
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
                grid.SetColumn(label, i);
                grid.SetRow(label, j);

                grid.Children.Add(bound);
                grid.SetColumn(bound, i);
                grid.SetRow(bound,j);

                grid.Children.Add(downBound);
                grid.SetColumn(downBound, i);
                grid.SetRow(downBound, j);

                if (i == this.Width - 1)
                {
                    i = -1;
                    j++;
                }
               
            }
            BoardView = grid;
            return grid;
        }
    }
}
