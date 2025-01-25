
//using Android.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksGame.Views
{
    public class GamePageView : BindableObject
    {
        int numberOfBlocks = 0;
        public int NumberOfBlocks
        { 
            get => numberOfBlocks;
            set
            {
                numberOfBlocks = value;
                OnPropertyChanged(nameof(NumberOfBlocks));
            }
        }
      
        public ObservableCollection<Block> ToChoose { get; set; } = [];
        public static IList<Block> NextToChoose { get; set; } = [];
        public
        static int score = 0;
        public int Score
        {
            get => score;
            set
            {
                score = value;
                OnPropertyChanged(nameof(ScoreStr));
            }
        }
        string scoreStr = "Score: ";
        public string ScoreStr
        {
            get => scoreStr + score;
            set
            {
                scoreStr = value;
                OnPropertyChanged();
            }
        }
        public BlocksBoard BlocksBoard { get; set; }
        public GamePageView()
        {
            Score = 0;
            BlocksBoard = new BlocksBoard(3, 3);

        }
        public GamePageView(uint width, uint height, uint numberOfBlocksToChoose)
        {
            Score = 0;
            BlocksBoard = new BlocksBoard(width, height);
            for (int i = 0; i < numberOfBlocksToChoose; i++)
            {
                ToChoose.Add(Block.GenerateBlock(BlocksBoard, i));
                NumberOfBlocks++;
            }
            OnPropertyChanged(nameof(ToChoose));
        }
        /// <summary>
        /// Check that player lose and give score
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="b"></param>
   
    }
}
