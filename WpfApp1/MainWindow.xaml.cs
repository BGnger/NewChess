using File_IO.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfApp1;

namespace ChessDisplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board board = new Board();

        public MainWindow()
        {
            InitializeComponent();
            SetUpBoard();

        }

        private void SetUpBoard()
        {
            ChessBoard.Children.Clear();
            ChessSquare chessSquare;
            bool grey = false;
            for (int column = 0; column < 8; column++)
            {
                for (int row = 0; row < 8; row++)
                {
                    chessSquare = new ChessSquare(grey);
                    chessSquare.SetValue(Grid.ColumnProperty, column);
                    chessSquare.SetValue(Grid.RowProperty, row);


                    chessSquare.SetPicture(board[column, row]?.BitmapImage);

                    ChessBoard.Children.Add(chessSquare);
                    grey = !grey;
                }
                grey = !grey;
            }
        }

        private void ChessBoard_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            UniformGrid con = (UniformGrid)sender;
            Point p = e.GetPosition(this);
            double x = p.X;
            double y = p.Y;
            int xPos = (int)x;
            int yPos = (int)y;
            FrameworkElement fe = e.OriginalSource as FrameworkElement;
            MessageBox.Show($"{fe.ToString()} {xPos} {yPos}");

            ChessBoard = con;
        }

        private void getPiece_Click(object sender, RoutedEventArgs e)
        {
            int x = 0;
            Int32.TryParse(xPos.Text, out x);
            int y = 0;
            Int32.TryParse(yPos.Text, out y);
            int Newx = 0;
            Int32.TryParse(NewxPos.Text, out Newx);
            int Newy = 0;
            Int32.TryParse(NewyPos.Text, out Newy);
            if (board.GetPiece(x, y) != null)
            {
                board.Move(x, y, Newx, Newy);
                SetUpBoard();
            }
            else
            {
                MessageBox.Show("No piece there!");
            }
        }
    }
}
