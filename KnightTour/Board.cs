using KnightTour;
using System.Windows.Media;

public class Board
{
    public int Size { get; private set; }
    public Cell[,] Cells { get; private set; }

    public Board(int size)
    {
        Size = size;
        Cells = new Cell[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Cells[i, j] = new Cell();
                Cells[i, j].Background = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
