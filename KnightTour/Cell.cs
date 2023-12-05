using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KnightTour
{
    public class Cell : Grid
    {
        bool isWhite;
        bool isVisited;
        public Cell() { }
        public Cell(bool isWhite, bool isVisited)
        {
            this.isWhite = isWhite;
            this.isVisited = isVisited;
        }
    }
}
