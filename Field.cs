using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksGame
{
    public class Field : BindableObject
    {
        public bool IsFree { get; set; } = true;
        public Field() { }
        public Field(Color color, bool isFree) { BackgroundColor = color; this.IsFree = IsFree; }
        Color backgroundColor = DefaultColor;
        public static Color DefaultColor { get; } = Color.FromRgba(0, 0, 0, 150);
        public Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                OnPropertyChanged();
            }
        }
    }
}
