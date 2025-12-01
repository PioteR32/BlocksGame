using BlocksGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksGameTests
{
	public  class BlocksBoardTests
	{
		[Fact]
		public void Constructor_Creates_Field_Array_With_Correct_Dimensions()
		{
			var board = new BlocksBoard(2, 3);
			Assert.Equal(2, board.Width);
			Assert.Equal(3, board.Height);
			Assert.NotNull(board.Fields);
			Assert.Equal(board.Height, board.Fields.GetLength(0));
			Assert.Equal(board.Width, board.Fields.GetLength(1));
		}

		[Fact]
		public async Task GenerateBaseBoard_Populates_Fields()
		{
			var board = new BlocksBoard(3, 2);
			// GenerateBaseBoard called by constructor; verify non-null entries
			for (int i = 0; i < board.Height; i++)
				for (int j = 0; j < board.Width; j++)
					Assert.NotNull(board.Fields[i, j]);

			await Task.CompletedTask;
		}

		[Fact]
		public void IsClear_Returns_False_When_Any_Field_IsNotFree()
		{
			var board = new BlocksBoard(2, 2);
			// mutate one field to be not free
			board.Fields[0, 0].IsFree = false;
			Assert.False(board.IsClear());
		}

		[Fact]
		public void ChangeFieldsOnCreate_Copies_Overlapping_Fields()
		{
			var src = new BlocksBoard(2, 2);
			// set a distinctive reference in src
			var marker = src.Fields[1, 1];
			// create target board larger than source using the special ctor
			var target = new BlocksBoard(3, 3, src);

			// overlapping area should reference original fields
			Assert.Same(marker, target.Fields[1, 1]);

			// non-overlapping should be created (not null)
			Assert.NotNull(target.Fields[2, 2]);
		}
	}
}
