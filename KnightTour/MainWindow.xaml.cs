using System;
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

        public MainWindow()
        {
            InitializeComponent();
            board = new Board(boardSize);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            boardSize = Int32.Parse(ChessboardSizeTextBox.Text);
            startX = Int32.Parse(StartPointXTextBox.Text);
            startY = Int32.Parse(StartPointYTextBox.Text);
            KhoiTaoBanCo(boardSize);
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
            ChessboardSizeTextBox.Text = "";
            StartPointXTextBox.Text = "";
            StartPointYTextBox.Text = "";
        }
    }
}
