//using Microsoft.UI.Xaml;
//using GameplayKit;
using Microsoft.Maui;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using static Android.Icu.Text.Transliterator;

namespace BlocksGame
{
    public class Block : BindableObject
    {
        public static int FieldWidth { get; } = 20;
        public static int FieldHeight { get; } = 20;
        public Block(bool[,] BlockStructure, int positionInToChoose)
        {
            Random random = new Random();
            int number = random.Next(4);
            if (0 == number)
                BackgroundColor = Color.FromArgb("#D32F2F");
            else if (1 == number)
                BackgroundColor = Color.FromArgb("#1E88E5");
            else if (2 == number)
                BackgroundColor = Color.FromArgb("#43A047");
            else
                BackgroundColor = Color.FromArgb("#FFD700");
            this.BlockStructure = BlockStructure;
            this.BlockStructure = CutStructure(ChangeTable2DToIList(BlockStructure));
            CreateBlock(this.BlockStructure);
            OnPropertyChanged(nameof(Fields));
            PositionInToChoose = positionInToChoose;
        }
        public int PositionInToChoose { get; set; }

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
        public int Width { get { return Fields.GetLength(1); } }
        public int Height { get { return Fields.GetLength(0); } }
        public Image Image;
        public Color BackgroundColor = Color.FromRgb(255, 0, 0);
        /// <summary>
        /// Structure Of Block 
        /// true = 1 = have block in this position
        /// false = 0 = dont have block
        /// </summary>
        public bool[,] BlockStructure { get; private set; } = new bool[9, 9];
        bool isInGrid = true;

        void CreateBlock(bool[,] structure)
        {
            Field[,] fields = new Field[structure.GetLength(0), structure.GetLength(1)];
            for (int i = 0; i < structure.GetLength(0); i++)
            {
                for (int j = 0; j < structure.GetLength(1); j++)
                {
                    if (structure[i, j])
                    {
                        fields[i, j] = new Field(BackgroundColor, false, i, j, FieldWidth, FieldHeight);
                    }
                    else
                    {
                        fields[i, j] = new Field(Color.FromRgb(0, 0, 0), true, i, j, FieldWidth, FieldHeight);
                    }
                }
            }
            Fields = fields;
        }
        public bool[,] CutStructure(IList<List<bool>> structure)
        {
            int structureHeight = structure.Count;
            int structureWidth = structure[0].Count;
            DeleteRows(structure, structureHeight);
            DeleteColumns(structure, structureWidth);
            return ChangeIListToTable2D(structure);
        }
        public static void DeleteRows(IList<List<bool>> structure, int structureHeight)
        {
            List<bool> rows = new List<bool>();
            for (int i = 0; i < structure.Count; i++)
            {
                bool isToDelete = true;
                for (int j = 0; j < structure[i].Count; j++)
                {
                    if (structure[i][j])
                    {
                        isToDelete = false;
                        break;
                    }
                }
                rows.Add(isToDelete);
            }
            if (structureHeight < 4)
                IsToDelete34(rows);
            else if (structureHeight < 8)
            {
                IsToDelete567(rows);
            }
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i])
                {
                    structure.RemoveAt(i);
                    rows.RemoveAt(i);
                    i--;
                }
            }

        }
        private static void IsToDelete567(List<bool> bools)
        {
            int tmp = -1;
            for (int i = 0; i < bools.Count; i++)
            {
                bool s = bools[i];
                if (!s)
                {
                    if (tmp != -1 && !s)
                    {
                        bools[tmp + 1] = false;
                    }
                    tmp = i;

                }
            }
        }
        private static void IsToDelete34(List<bool> bools)
        {
            int tmp = -1;
            for (int i = 0; i < bools.Count; i++)
            {
                bool s = bools[i];
                if (!s)
                {
                    if (tmp != -1 && !s)
                    {
                        for (int j = tmp; j < i; j++)
                            bools[j] = false;
                    }
                    tmp = i;

                }
            }
        }
        public static void DeleteColumns(IList<List<bool>> structure, int structureWidth)
        {
            List<bool> columns = new List<bool>();
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
                columns.Add(isToDelete);
            }
            if (structureWidth < 4)
                IsToDelete34(columns);
            else if (structureWidth < 7)
            {
                IsToDelete567(columns);
            }
            for (int j = 0; j < columns.Count; j++)
            {
                if (columns[j])
                {
                    for (int i = 0; i < structure.Count; i++)
                    {
                        structure[i].RemoveAt(j);
                    }
                    columns.RemoveAt(j);
                    j--;
                }
            }

        }
        public static T[,] ChangeIListToTable2D<T>(IList<List<T>> list)
        {
            T[,] tmp = new T[list.Count, list[0].Count];
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Count; j++)
                {
                    tmp[i, j] = list[i][j];
                }
            }
            return tmp;
        }
        public static IList<List<T>> ChangeTable2DToIList<T>(T[,] table)
        {
            IList<List<T>> list = (IList<List<T>>)new List<List<T>>();

            for (int i = 0; i < table.GetLength(0); i++)
            {
                list.Add(new List<T>());
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    list[i].Add(table[i, j]);
                }
            }
            return list;
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
    
       
        /// <summary>
        /// Generate block on blocksBoard width and height
        /// </summary>
        /// <param name="blocksBoard"></param>
        /// <returns></returns>
        public static Block GenerateBlock(BlocksBoard blocksBoard, int positionInToChoose)
        {
            Random random = new Random();
            int numberOfBlocks;
            if (blocksBoard.Width != blocksBoard.Height)
                numberOfBlocks = random.Next(1, ((int)blocksBoard.Width + (int)blocksBoard.Height) / 2);
            else if (blocksBoard.Width > 3 || blocksBoard.Height > 3)
                numberOfBlocks = random.Next(1, ((int)blocksBoard.Width + (int)blocksBoard.Height) / 2 - 1);
            else
                numberOfBlocks = random.Next(1, ((int)blocksBoard.Width + (int)blocksBoard.Height) / 2 + 1);
            return new Block(GenerateBlockStructure(blocksBoard, numberOfBlocks), positionInToChoose);
        }
        /// <summary>
        /// Generate Blocks 
        /// </summary>
        /// <param name="blocksBoard"></param>
        /// <param name="numberOfBlocks">number of Fields in Block</param>
        /// <returns></returns>
        public static bool[,] GenerateBlockStructure(BlocksBoard blocksBoard, int numberOfBlocks)
        {
            int width = (int)blocksBoard.Width, height = (int)blocksBoard.Height;
            if ((int)blocksBoard.Width < 5 && (int)blocksBoard.Height < 5)
            {
                width = (int)blocksBoard.Width;
                height = (int)blocksBoard.Height;
            }
            else if ((int)blocksBoard.Width < 5)
            {
                width = (int)blocksBoard.Width;
            }
            else if (blocksBoard.Height < 5)
            {
                height = (int)blocksBoard.Height;
            }
            else
            {
                width = 5;
                height = 5;
            }
            Random random = new Random();
            bool[,] tmp = new bool[blocksBoard.Height, blocksBoard.Width];

            for (int i = 0; i < numberOfBlocks; i++)
            {
                int x = random.Next(width);
                int y = random.Next(height);
                if (!tmp[y, x])
                {
                    tmp[y, x] = true;
                }
                else
                    i--;
            }
            return tmp;
        }
    }
}
