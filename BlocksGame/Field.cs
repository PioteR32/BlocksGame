using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		public Color baseColor;
		public Field(Color color, bool isFree) { BackgroundColor = color; this.IsFree = isFree; shadowColor = BackgroundColor; }
		public Field(int rowPosition, int columnPosition)
		{
			RowPosition = rowPosition;
			ColumnPosition = columnPosition;
			shadowColor = BackgroundColor;
		}

		public Field(int rowPosition, int columnPosition, int width, int height)
		{
			RowPosition = rowPosition;
			ColumnPosition = columnPosition;
			Width = width;
			Height = height;
			baseColor = shadowColor = BackgroundColor;
		}
		public Field(Color color, bool isFree, int rowPosition, int columnPosition)
		{
			baseColor =BackgroundColor = color;
			this.IsFree = isFree;
			RowPosition = rowPosition;
			ColumnPosition = columnPosition;
			shadowColor = BackgroundColor;
		}
		public Field(Color color, bool isFree, int rowPosition, int columnPosition, int width, int height)
		{
			baseColor = BackgroundColor = color;
			this.IsFree = isFree;
			RowPosition = rowPosition;
			ColumnPosition = columnPosition;
			Width = width;
			Height = height;
			shadowColor = BackgroundColor;

		}

		private Color shadowColor = Color.FromRgb(255, 0, 0);
		private double shadowOpacity = 0;
		private Point shadowOffset = new Point(0, 0);
		private double shadowRadius = 20;
		int zIndex = 1;
		public int ZIndex
		{
			get => zIndex;
			set
			{
				zIndex = value;
				OnPropertyChanged();
			}
		}
		public Color ShadowColor
		{
			get => shadowColor;
			set
			{
				if (shadowColor != value)
				{
					shadowColor = value;
					OnPropertyChanged(nameof(ShadowColor));
				}
			}
		}

		public double ShadowOpacity
		{
			get => shadowOpacity;
			set
			{
				if (shadowOpacity != value)
				{
					shadowOpacity = value;
					if (shadowOpacity == 1)
					{
						ZIndex = 1000;
					}
					else
						ZIndex = 1;
					OnPropertyChanged(nameof(ShadowOpacity));
				}
			}
		}
		double opacity = 1;
		public double Opacity
		{
			get => opacity;
			set
			{
				opacity = value;
				OnPropertyChanged();
			}
		}

		public Point ShadowOffset
		{
			get => shadowOffset;
			set
			{
				if (shadowOffset != value)
				{
					shadowOffset = value;
					OnPropertyChanged(nameof(ShadowOffset));
				}
			}
		}

		public double ShadowRadius
		{
			get => shadowRadius;
			set
			{
				if (shadowRadius != value)
				{
					shadowRadius = value;
					OnPropertyChanged(nameof(ShadowRadius));
				}
			}
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

		public async Task FadeOpacity(int opacity , int miliseconds)
		{
			
			if (opacity < Opacity)
			{
				double numberOfMinusOpacity = Opacity - opacity / miliseconds;
				while (Opacity > opacity)
				{
					Opacity -= numberOfMinusOpacity;
					Thread.Sleep(1);
				}
			}
			if (Opacity < opacity)
			{
				double numberOfPlusOpacity = Opacity - opacity / miliseconds;
				while (opacity > Opacity)
				{
					Opacity += numberOfPlusOpacity;
					Thread.Sleep(1);
				}
			}
		}
	}
}
