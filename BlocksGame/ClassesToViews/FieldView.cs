using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksGame.ClassesToViews
{
    interface GridPosition
    {
        public abstract int RowPosition { get; set; }
        public abstract int ColumnPosition
        {
            get;
            set;
        }

    }
}
