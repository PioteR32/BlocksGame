using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksGame.UtilsMaui
{
    namespace UtilsMaui.Utils
    {
        public class GridHelpers
        {
            #region RowCount Property

            /// <summary>
            /// Adds the specified number of Rows to RowDefinitions. 
            /// Default Height is Auto
            /// </summary>
            public static readonly BindableProperty RowCountProperty =
                BindableProperty.CreateAttached(
                    "RowCount", typeof(int), typeof(GridHelpers), -1,
                    propertyChanged: RowCountChanged);

            // Get
            public static int GetRowCount(BindableObject obj)
            {
                return (int)obj.GetValue(RowCountProperty);
            }

            // Set
            public static void SetRowCount(BindableObject obj, int value)
            {
                obj.SetValue(RowCountProperty, value);
            }

            // Change Event - Adds the Rows
            public static void RowCountChanged(BindableObject obj, object oldValue, object newValue)
            {
                if (!(obj is Grid) || (int)newValue < 0)
                    return;

                Grid grid = (Grid)obj;
                grid.RowDefinitions.Clear();

                for (int i = 0; i < (int)newValue; i++)
                    grid.RowDefinitions.Add(
                        new RowDefinition() { Height = GridLength.Auto});
            }

            #endregion

            #region ColumnCount Property

            /// <summary>
            /// Adds the specified number of Columns to ColumnDefinitions. 
            /// Default Width is Auto
            /// </summary>
            public static readonly BindableProperty ColumnCountProperty =
                BindableProperty.CreateAttached(
                    "ColumnCount", typeof(int), typeof(GridHelpers), -1,
                    propertyChanged: ColumnCountChanged);

            // Get
            public static int GetColumnCount(BindableObject obj)
            {
                return (int)obj.GetValue(ColumnCountProperty);
            }

            // Set
            public static void SetColumnCount(BindableObject obj, int value)
            {
                obj.SetValue(ColumnCountProperty, value);
            }

            // Change Event - Add the Columns
            public static void ColumnCountChanged(BindableObject obj, object oldValue, object newValue)
            {
                if (!(obj is Grid) || (int)newValue < 0)
                    return;

                Grid grid = (Grid)obj;
                grid.ColumnDefinitions.Clear();

                for (int i = 0; i < (int)newValue; i++)
                    grid.ColumnDefinitions.Add(
                        new ColumnDefinition() { Width = GridLength.Auto });
            }

            #endregion
        }
    }
}
