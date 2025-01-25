
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
        int MinimumWidthRequest = 30;
        int MinimumHeightRequest = 30;
        public int Width { get; private set; }
        public int Height { get; private set; }
        Field[,] fields;
        public Field[,] Fields
        {
            get => fields; 
            set
            {
               fields = value;
               OnPropertyChanged();
            }
        }
        public BlocksBoard(uint width, uint height)
        {
            this.Width = (int)width;
            this.Height = (int)height;
            Fields = new Field[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Fields[i, j] = new Field(i,j, MinimumWidthRequest, MinimumHeightRequest);
                }
            }
        }
        public bool IsClear()
        {
            for (int i = 0; i < Fields.GetLength(0); i++)
            {
                for (int j = 0; j < Fields.GetLength(1); j++)
                {
                    if(!Fields[i, j].IsFree)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
