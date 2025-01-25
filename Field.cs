using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksGame
{
    public class Field : BindableObject, ClassesToViews.GridPosition
    {
        public bool IsFree { get; set; } = true;
        public int Width { get; set; }
        public int Height { get; set; }
        public Field() 
        {

        }
        public Field(Color color, bool isFree) { BackgroundColor = color; this.IsFree = IsFree; }
        public Field(int rowPosition, int columnPosition)
        {
            RowPosition = rowPosition;
            ColumnPosition = columnPosition;
        }

        public Field(int rowPosition, int columnPosition,int width,int height)
        {
            RowPosition = rowPosition;
            ColumnPosition = columnPosition;
            Width = width;
            Height = height;
        }
        public Field(Color color, bool isFree, int rowPosition, int columnPosition)
        {
            BackgroundColor = color;
            this.IsFree = IsFree;
            RowPosition = rowPosition;
            ColumnPosition = columnPosition;
        }
        public Field(Color color, bool isFree, int rowPosition, int columnPosition, int width, int height)
        {
            BackgroundColor = color;
            this.IsFree = IsFree;
            RowPosition = rowPosition;
            ColumnPosition = columnPosition;
            Width = width;
            Height = height;
        }
        Color backgroundColor = DefaultColor;
        public static Color DefaultColor { get; } = Color.FromRgba(0, 0, 0, 255);
        public Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                OnPropertyChanged();
            }
        }
        int rowPosition = 0;
        int columnPosition = 0;
        public int RowPosition
        {
            get => rowPosition;
            set
            { rowPosition = value; OnPropertyChanged(); }
        }
        public int ColumnPosition { get => columnPosition; set { columnPosition = value; OnPropertyChanged(); } }
    }
}
