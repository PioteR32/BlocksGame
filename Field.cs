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
        Color backgroundColor = Color.FromRgb(255, 0, 0);
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
