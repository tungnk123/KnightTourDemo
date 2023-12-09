using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Media;

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
        private SoundPlayer soundPlayer;
        public MainWindow()
        {
            InitializeComponent();
            board = new Board(boardSize);
            ansList = new List<int[,]>();
            soundPlayer = new SoundPlayer("Resources/sound.wav");
        }

        public void ChayMaDiTuan(int i, int curRow, int curCol, int n, int[,] visited)
        {
            if (ansList.Count >= 1000 || (boardSize == 8 && ansList.Count == 2))
            {
                return;
            }
            for (int j = 0; j < 8; j++)
            {
                int newRow = curRow + rowDir[j];
                int newCol = curCol + colDir[j];

                // Check xem o (newRow, newCol) co hop le hay khong
                if (IsValid(newRow, newCol, n, visited))
                {
                    visited[newRow, newCol] = i;
                    
                    // i bat dau la 1
                    if (i == n * n - 1)
                    {
                        // Luu vao tap ket qua
                        int[,] solution = new int[visited.GetLength(0), visited.GetLength(1)];
                        Array.Copy(visited, solution, visited.Length);
                        ansList.Add(solution);

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

        private async void KhoiChayConNgua(int truongHopI)
        {
            try
            {
                int[,] dapAnThuI = ansList[truongHopI - 1];
                int i = 1;
                // Xy ly truong hop di dau tien
                TextBlock textBlock = new TextBlock();
                textBlock.FontSize = 15;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.Height = 20;
                int[] toaDoThuNhat = SearchToaDo(i, dapAnThuI);
                textBlock.Text = $"Bước 0:    Đi từ ({startX}, {startY}) đến ({toaDoThuNhat[0]}, {toaDoThuNhat[1]})";
                ResultStackPanel.Children.Add(textBlock);


                while (i != boardSize * boardSize)
                {
                    await ChuyenToaDoMa(i, dapAnThuI);
                    textBlock = new TextBlock();
                    textBlock.FontSize = 15;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    int[] toaDoHienTai = SearchToaDo(i, dapAnThuI);
                    if (i + 1 == boardSize * boardSize)
                    {
                        break;
                    }
                    int[] toaDoTiepTheo = SearchToaDo(i + 1, dapAnThuI);
                    board.Cells[toaDoTiepTheo[0], toaDoTiepTheo[1]].Background = new SolidColorBrush(Colors.Red);
                    textBlock.Text = $"Bước {i}:    Đi từ ({toaDoHienTai[0]}, {toaDoHienTai[1]}) đến ({toaDoTiepTheo[0]}, {toaDoTiepTheo[1]})";
                    ResultStackPanel.Children.Add(textBlock);
                    i++;
                    PlayChessMoveSound();
                    await Task.Delay(500);
                }
            }
            catch
            {
                MessageBox.Show("Lựa chọn không hợp lệ!");
            }
        }

        private int[] SearchToaDo(int key, int[,] dapAnThuI)
        {
            int[] ans = new int[2];
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (dapAnThuI[i, j] == key)
                    {
                        return new int[] { i, j };
                    }
                }
            }
            return ans;
        }

        private async Task ChuyenToaDoMa(int k, int[,] dapAnThuI)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (dapAnThuI[i, j] == k)
                    {
                        board.Cells[i, j].Number = k;
                        board.Cells[i, j].IsVisited = true;
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
                        board.Cells[i, j].Children.Add(img);
                        await Task.Delay(500);
                        board.Cells[i, j].Children.Remove(img);
                        return;
                    }
                }
            }
        }
        public void AddStartName()
        {
            Image img = new Image();
            img.HorizontalAlignment = HorizontalAlignment.Stretch;
            img.VerticalAlignment = VerticalAlignment.Stretch;

            try
            {
                // Assuming the image is in a folder named "Resources" in your project
                img.Source = new BitmapImage(new Uri("Resources/go.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }

            board.Cells[startX, startY].Children.Add(img);
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                boardSize = Int32.Parse(ChessboardSizeTextBox.Text);
                startX = Int32.Parse(StartPointXTextBox.Text);
                startY = Int32.Parse(StartPointYTextBox.Text);
                board = new Board(boardSize);


                visited = new int[boardSize, boardSize];
                visited[startX, startY] = -1;
                AddStartName();
                ResetChessBoard();
                KhoiTaoBanCo(boardSize);
                //board.Cells[startX, startY].Background = new SolidColorBrush(Colors.Blue);
                AddStartName();
                ChayMaDiTuan(1, startX, startY, boardSize, visited);
                
                if (ansList.Count < 1000)
                {
                    CachChayTextBox.Text = "Có tất cả " + ansList.Count + " cách chạy";
                }
                else
                {
                    CachChayTextBox.Text = "Đã chạy được " + ansList.Count + " cách chạy";
                }
                if (boardSize >= 8)
                {
                    CachChayTextBox.Text = "Đã chạy được " + ansList.Count + " cách chạy";
                }
                if (boardSize >= 8)
                {
                    ThongBaoTextBox.Text = "(Cho dừng thuật toán khi tập kết quả > 2)";
                }
                else
                {
                    ThongBaoTextBox.Text = "(Cho dừng thuật toán khi tập kết quả > 1000)";
                }
                AddStartName();
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập thông tin hợp lệ!");
            }
        }

        private void MinhHoaButton_Click(object sender, RoutedEventArgs e)
        {
            ResultStackPanel.Children.Clear();
            visited = new int[boardSize, boardSize];
            visited[startX, startY] = -1;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board.Cells[i, j].IsVisited = false;
                }
            }
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++) {
                    board.Cells[i, j].Children.Clear();
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
                }
            }
            board.Cells[startX, startY].Background = new SolidColorBrush(Colors.Blue);
            AddStartName();
            int cachChay = Int32.Parse(LuaChonTextBox.Text);
            KhoiChayConNgua(cachChay);
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

        private void ResetChessBoard()
        {
            ChessboardGrid.Children.Clear();
            ChessboardGrid.ColumnDefinitions.Clear();
            ChessboardGrid.RowDefinitions.Clear();
            ansList = new List<int[,]>();
        }

        private void PlayChessMoveSound()
        {
            try
            {
                if (SoundToggle.IsChecked == true)
                {
                    soundPlayer.Play();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing sound: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
