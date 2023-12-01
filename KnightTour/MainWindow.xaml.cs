using System;
using System.Windows;

namespace KnightTour
{
    public partial class MainWindow : Window
    {
        private int boardSize;
        private int startX, startY;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý khi nhấn nút Bắt đầu
            if (int.TryParse(BoardSizeTextBox.Text, out boardSize) && int.TryParse(StartPositionTextBox.Text, out startX))
            {
                // Gọi hàm xử lý mã đi tuần với thông số boardSize, startX, startY
                // Code xử lý mã đi tuần ở đây
            }
            else
            {
                MessageBox.Show("Nhập thông số không hợp lệ!");
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý khi nhấn nút Reset
            ChessboardCanvas.Children.Clear();
            BoardSizeTextBox.Text = "";
            StartPositionTextBox.Text = "";
        }
    }
}
