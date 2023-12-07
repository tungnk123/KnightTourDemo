using System.Windows.Controls;
using System.Windows.Media;

namespace KnightTour
{
    public class Cell : Grid
    {
        private bool isWhite;
        private bool _isVisited;
        private int number;

        public bool IsVisited
        {
            get { return _isVisited; }
            set
            {
                _isVisited = value;
                if (value)
                {
                    // Set the background color to blue
                    this.Background = new SolidColorBrush(Colors.Blue);

                    // Add a TextBlock with the number to the cell
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = number.ToString();
                    textBlock.FontSize = 24;
                    textBlock.Foreground = new SolidColorBrush(Colors.White);
                    textBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                    this.Children.Add(textBlock);
                }
            }
        }

        public Cell()
        {
            InitializeCell(false, 0, false);
        }

        public Cell(bool isWhite, int number, bool isVisited)
        {
            InitializeCell(isWhite, number, isVisited);
        }

        private void InitializeCell(bool isWhite, int number, bool isVisited)
        {
            this.isWhite = isWhite;
            this.number = number;
            this._isVisited = isVisited;
        }
    }
}
