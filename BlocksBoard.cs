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
                };
                fields[i, j].BackgroundColor = Field.DefaultColor;
                label.SetBinding(Label.BackgroundColorProperty, new Binding(nameof(Field.BackgroundColor)));
                grid.Children.Add(label);
                grid.SetColumn(label, j);
                grid.SetRow(label, i);
                var leftBound = new Label
                {
                    BackgroundColor = Global.BoundsColor,
                    WidthRequest = 1,
                    HeightRequest = MinimumHeightRequest,
                    HorizontalOptions = LayoutOptions.Start,
                };
                grid.Children.Add(leftBound);
                grid.SetColumn(leftBound, j);
                grid.SetRow(leftBound, i);

                var downBound = new Label
                {
                    BackgroundColor = Global.BoundsColor,
                    WidthRequest = MinimumWidthRequest,
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                };
                grid.Children.Add(downBound);
                grid.SetColumn(downBound, j);
                grid.SetRow(downBound, i);

                var hightBound = new Label
                {
                    BackgroundColor = Global.BoundsColor,
                    WidthRequest = MinimumWidthRequest,
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.Start,
                };
                grid.Children.Add(hightBound);
                grid.SetColumn(hightBound, j);
                grid.SetRow(hightBound, i);

                var rightBound = new Label
                {
                    BackgroundColor = Global.BoundsColor,
                    WidthRequest = 1,
                    HeightRequest = MinimumHeightRequest,
                    HorizontalOptions = LayoutOptions.End,
                };
                grid.Children.Add(rightBound);
                grid.SetColumn(rightBound, j);
                grid.SetRow(rightBound, i);


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
