using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KnightTour
{
    public partial class MainWindow : Window
    {
        private int boardSize;
        private int startX, startY;
        public Board board;
        public int[,] visited;
        static int[] rowDir = { 2, 1, -1, -2, -2, -1, 1, 2 };
        static int[] colDir = { 1, 2, 2, 1, -1, -2, -2, -1 };
        public List<int[,]> ansList;
        public MainWindow()
        {
            InitializeComponent();
            board = new Board(boardSize);
            ansList = new List<int[,]>();
        }

        public void ChayMaDiTuan(int i, int curRow, int curCol, int n, int[,] visited)
        {
            for (int j = 0; j < n; j++)
            {
                int newRow = curRow + rowDir[j];
                int newCol = curCol + colDir[j];

                // Check xem o (newRow, newCol) co hop le hay khong
                if (IsValid(newRow, newCol, n, visited))
                {
                    visited[newRow, newCol] = i;
                    // i bat dau la 1
                    if (i == n * n)
                    {
                        // Luu vao tap ket qua
                        ansList.Add(visited);
                    }
                    else
                    {
                        ChayMaDiTuan(i + 1, newRow, newCol, n, visited);
                    }
                    visited[newRow,newCol] = 0;
                }
            }
        }

        public bool IsValid(int row, int col, int n, int[,] visited)
        {
            if (row < 0 || row >= n || col < 0 || col >= n || visited[row, col] != 0)
            {
                return false;
            }
            return true;
        }

        public void KhoiChayConNgua()
        {

        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            boardSize = Int32.Parse(ChessboardSizeComboBox.Text);
            startX = Int32.Parse(StartPointXComboBox.Text);
            startY = Int32.Parse(StartPointYComboBox.Text);
            board = new Board(boardSize);
            visited = new int[boardSize, boardSize];
            visited[startX, startY] = -1;
            KhoiTaoBanCo(boardSize);
            ChayMaDiTuan(1, startX, startY, boardSize, visited);
        }

        private void KhoiTaoBanCo(int size)
        {
            for (int i = 0; i < size; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                RowDefinition rowDefinition = new RowDefinition();
                ChessboardGrid.ColumnDefinitions.Add(columnDefinition);
                ChessboardGrid.RowDefinitions.Add(rowDefinition);
            }
            board = new Board(boardSize);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++) {
                    if (((i + j) % 2) == 0)
                    {
                        board.Cells[i, j].Background = new SolidColorBrush(
                            Color.FromRgb(
                                Convert.ToByte(0xF1), // Red component
                                Convert.ToByte(0xE0), // Green component
                                Convert.ToByte(0xC0)  // Blue component
                            )
                        );

                    }
                    else
                    {
                        board.Cells[i, j].Background = new SolidColorBrush(
                            Color.FromRgb(
                                Convert.ToByte(0x5D), // Red component
                                Convert.ToByte(0x99), // Green component
                                Convert.ToByte(0x48)  // Blue component
                            )
                        );
                    }
                    Grid.SetRow(board.Cells[i, j], i);
                    Grid.SetColumn(board.Cells[i, j], j);
                    ChessboardGrid.Children.Add(board.Cells[i, j]);
                }
            }
            // Setup con ngua
            Image img = new Image();
            img.HorizontalAlignment = HorizontalAlignment.Stretch;
            img.VerticalAlignment = VerticalAlignment.Stretch;

            try
            {
                // Assuming the image is in a folder named "Resources" in your project
                img.Source = new BitmapImage(new Uri("Resources/knight.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }



            board.Cells[startX, startY].Children.Add(img);

            board.Cells[startX, startY].IsVisited = true;

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý khi nhấn nút Reset
            ChessboardGrid.Children.Clear();
            ChessboardSizeComboBox.Text = "";
            StartPointXComboBox.Text = "";
            StartPointYComboBox.Text = "";
        }
    }
}
