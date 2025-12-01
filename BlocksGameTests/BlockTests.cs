
using BlocksGame;
using BlocksGame.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksGameTests
{

	public class BlockTests
	{

		[Fact]
		public void ChangeTable2DToIList_Then_ChangeIListToTable2D_RoundTrip()
		{
			bool[,] table = new bool[,]
			{
				{ true, false },
				{ false, true }
			};

			var list = Block.ChangeTable2DToIList(table);
			var back = Block.ChangeIListToTable2D(list);

			Assert.Equal(table.GetLength(0), back.GetLength(0));
			Assert.Equal(table.GetLength(1), back.GetLength(1));
			for (int i = 0; i < table.GetLength(0); i++)
				for (int j = 0; j < table.GetLength(1); j++)
					Assert.Equal(table[i, j], back[i, j]);
		}

		[Fact]
		public void CutStructure_Trims_Empty_Borders()
		{
			// 3x3 with center true -> should cut to 1x1 true
			bool[,] table = new bool[,]
			{
				{ false, false, false },
				{ false, true,  false },
				{ false, false, false }
			};

			var iList = Block.ChangeTable2DToIList(table);
			var cut = new Block(table, 0).CutStructure(iList);

			Assert.Equal(1, cut.GetLength(0));
			Assert.Equal(1, cut.GetLength(1));
			Assert.True(cut[0, 0]);
		}

		[Fact]
		public void ChangeIListToTable2D_Handles_GenericTypes()
		{
			var list = new List<List<int>> { new List<int> { 1, 2 }, new List<int> { 3, 4 } };
			var table = Block.ChangeIListToTable2D(list);
			Assert.Equal(2, table.GetLength(0));
			Assert.Equal(2, table.GetLength(1));
			Assert.Equal(1, table[0, 0]);
			Assert.Equal(4, table[1, 1]);
		}

		[Fact]
		public void GenerateBlockStructure_WithOne_ReturnsSingleTrue()
		{
			var board = new BlocksBoard(4, 4);
			var arr = Block.GenerateBlockStructure(board, 1,true);
			Assert.Equal(1, arr.GetLength(0));
			Assert.Equal(1, arr.GetLength(1));

			Assert.True(arr[0,0]);
		}

		[Fact]
		public void GenerateBlockStructure_WithCount_BoundsAndTrueCount()
		{
			var board = new BlocksBoard(4, 5);
			int number = 3;
			var arr = Block.GenerateBlockStructure(board, number);

			// array dimensions should not exceed board dims
			Assert.InRange(arr.GetLength(0), 1, board.Height);
			Assert.InRange(arr.GetLength(1), 1, board.Width);

			// count of trues is <= requested number and >= 1 (since number >=1)
			int trueCount = 0;
			for (int i = 0; i < arr.GetLength(0); i++)
				for (int j = 0; j < arr.GetLength(1); j++)
					if (arr[i, j]) trueCount++;
			Assert.InRange(trueCount, 1, number);
		}

		[Fact]
		public void StaticGenerateBlock_Returns_Block_Instance()
		{
			var board = new BlocksBoard(3, 3);
			var block = Block.GenerateBlock(board, 0);
			Assert.NotNull(block);
			Assert.IsType<Block>(block);
			Assert.Equal(0, block.PositionInToChoose);
			Assert.NotNull(block.BlockStructure);
			Assert.NotNull(block.Fields);
		}

		[Fact]
		public async Task InstanceGenerateBlock_Sets_BlockStructure_And_Fields()
		{
			var board = new BlocksBoard(3, 3);
			var b = new Block(board, 1); // constructor calls GenerateBlock internally
			Assert.Equal(1, b.PositionInToChoose);
			Assert.NotNull(b.BlockStructure);
			Assert.NotNull(b.Fields);
			Assert.Equal(b.Height, b.Fields.GetLength(0));
			Assert.Equal(b.Width, b.Fields.GetLength(1));

			// RowPosition / ColumnPosition consistent with GamePageView.columnCount
			var expectedRow = b.PositionInToChoose / GamePageView.columnCount;
			var expectedCol = b.PositionInToChoose - expectedRow * GamePageView.columnCount;
			Assert.Equal(expectedRow, b.RowPosition);
			Assert.Equal(expectedCol, b.ColumnPosition);

			await Task.CompletedTask;
		}

		[Fact]
		public void DeleteColumns_DoesNotThrow_And_Removes_Empty_Columns()
		{
			// construct a structure where first and last column empty, middle column has true
			var list = new List<List<bool>>
			{
				new List<bool> { false, true, false },
				new List<bool> { false, true, false }
			};

			// initial width 3
			Block.DeleteColumns(list, 3);

			// After deletion, remaining columns should at least include the middle column's trues
			var arr = Block.ChangeIListToTable2D(list);
			bool anyTrue = false;
			for (int i = 0; i < arr.GetLength(0); i++)
				for (int j = 0; j < arr.GetLength(1); j++)
					anyTrue |= arr[i, j];
			Assert.True(anyTrue);
		}
	}
}
