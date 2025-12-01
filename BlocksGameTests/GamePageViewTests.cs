using BlocksGame;
using BlocksGame.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksGameTests
{
	public class GamePageViewTests
	{
		[Fact]
		public void DefaultConstructor_Sets_Initial_Values()
		{
			var view = new GamePageView();
			Assert.Equal(0, view.Score);
			Assert.NotNull(view.BlocksBoard);
			Assert.Equal(3, view.BlocksBoard.Width);
			Assert.Equal(3, view.BlocksBoard.Height);
			Assert.Equal(GamePageView.columnCount, view.ColumnCount);
		}

		[Fact]
		public void ParameterizedConstructor_Populates_ToChoose_And_NumberOfBlocks()
		{
			uint width = 3, height = 2, numberToChoose = 4;
			var view = new GamePageView(width, height, numberToChoose, GameModes.Normal);

			Assert.NotNull(view.BlocksBoard);
			Assert.Equal((int)width, view.BaseWidth);
			Assert.Equal((int)height, view.BaseHeigh);
			Assert.Equal((int)numberToChoose, view.BaseNumberOfBlocks);
			Assert.Equal((int)numberToChoose, view.NumberOfBlocks);
		}

		[Fact]
		public void RowCount_Computes_Ceil_Based_On_NumberOfBlocks_And_ColumnCount()
		{
			var view = new GamePageView();
			view.NumberOfBlocks = 1;
			int expected = (1 + GamePageView.columnCount - 1) / GamePageView.columnCount;
			Assert.Equal(expected, view.RowCount);

			view.NumberOfBlocks = GamePageView.columnCount + 1;
			expected = (view.NumberOfBlocks + GamePageView.columnCount - 1) / GamePageView.columnCount;
			Assert.Equal(expected, view.RowCount);
		}

		[Fact]
		public void Lives_Property_Updates_LivesStr()
		{
			var view = new GamePageView();
			view.Lives = 5;
			Assert.Equal(5, view.Lives);
			Assert.Equal("Lives: 5", view.LivesStr);
		}

		[Fact]
		public void Combo_ChangeCombo_Setting_Updates_ChangeComboOpacity_Immediately()
		{
			var view = new GamePageView();
			// initial combo is 0
			view.Combo = 3;
			// setting Combo triggers ChangeCombo setter which sets ChangeComboOpacity to 1 synchronously
			Assert.Equal(1.0, view.ChangeComboOpacity);
			// ChangeCombo should show a plus sign for positive changes
			Assert.StartsWith("+", view.ChangeCombo);
		}

	}
}
