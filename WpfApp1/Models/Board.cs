using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace File_IO.Models
{
    public class Board
    {
        private ChessPiece[][] board;
        private PieceColor lastColor = PieceColor.D;

        public Board()
        {
            board = new ChessPiece[8][];
            for (int y = 0; y < board.Length; y++)
            {
                board[y] = new ChessPiece[8];
            }
            SetUpBoard();
        }

        private void SetUpBoard()
        {
            //Light
            this[0, 0] = new ChessPiece(Pieces.R, PieceColor.L, new BitmapImage(new Uri("../Resources/R_L.png", UriKind.Relative)));
            this[0, 1] = new ChessPiece(Pieces.N, PieceColor.L, new BitmapImage(new Uri("../Resources/K_L.png", UriKind.Relative)));
            this[0, 2] = new ChessPiece(Pieces.B, PieceColor.L, new BitmapImage(new Uri("../Resources/B_L.png", UriKind.Relative)));
            this[0, 3] = new ChessPiece(Pieces.Q, PieceColor.L, new BitmapImage(new Uri("../Resources/Q_L.png", UriKind.Relative)));
            this[0, 4] = new ChessPiece(Pieces.K, PieceColor.L, new BitmapImage(new Uri("../Resources/K_L.png", UriKind.Relative)));
            this[0, 5] = new ChessPiece(Pieces.B, PieceColor.L, new BitmapImage(new Uri("../Resources/B_L.png", UriKind.Relative)));
            this[0, 6] = new ChessPiece(Pieces.N, PieceColor.L, new BitmapImage(new Uri("../Resources/K_L.png", UriKind.Relative)));
            this[0, 7] = new ChessPiece(Pieces.R, PieceColor.L, new BitmapImage(new Uri("../Resources/R_L.png", UriKind.Relative)));

            for (int i = 0; i < 8; i++)
            {
                this[1, i] = new ChessPiece(Pieces.P, PieceColor.L, new BitmapImage(new Uri("../Resources/P_L.png", UriKind.Relative)));
            }

            //Dark
            this[7, 0] = new ChessPiece(Pieces.R, PieceColor.D, new BitmapImage(new Uri("../Resources/R_D.png", UriKind.Relative)));
            this[7, 1] = new ChessPiece(Pieces.N, PieceColor.D, new BitmapImage(new Uri("../Resources/K_D.png", UriKind.Relative)));
            this[7, 2] = new ChessPiece(Pieces.B, PieceColor.D, new BitmapImage(new Uri("../Resources/B_D.png", UriKind.Relative)));
            this[7, 3] = new ChessPiece(Pieces.Q, PieceColor.D, new BitmapImage(new Uri("../Resources/Q_D.png", UriKind.Relative)));
            this[7, 4] = new ChessPiece(Pieces.K, PieceColor.D, new BitmapImage(new Uri("../Resources/K_D.png", UriKind.Relative)));
            this[7, 5] = new ChessPiece(Pieces.B, PieceColor.D, new BitmapImage(new Uri("../Resources/B_D.png", UriKind.Relative)));
            this[7, 6] = new ChessPiece(Pieces.N, PieceColor.D, new BitmapImage(new Uri("../Resources/K_D.png", UriKind.Relative)));
            this[7, 7] = new ChessPiece(Pieces.R, PieceColor.D, new BitmapImage(new Uri("../Resources/R_D.png", UriKind.Relative)));

            for (int i = 0; i < 8; i++)
            {
                this[6, i] = new ChessPiece(Pieces.P, PieceColor.D, new BitmapImage(new Uri("../Resources/P_D.png", UriKind.Relative)));
            }
        }

        public void SetPiece(int x, int y, ChessPiece piece)
        {
            board[y][x] = piece;
        }

        public ChessPiece GetPiece(int x, int y)
        {
            return board[y][x];
        }

        public ChessPiece this[int x, int y]
        {
            get
            {
                return board[y][x];
            }
            set
            {
                board[y][x] = value;
            }
        }

        public bool Check(PieceColor kingColor)
        {
            int kingX = 0;
            int kingY = 0;
            for (int y = 0; y < board.Length; y++)
            {
                for (int x = 0; x < board[y].Length; x++)
                {
                    if (board[y][x] != null && board[y][x].Piece == Pieces.K && board[y][x].Color == kingColor)
                    {
                        kingX = x;
                        kingY = y;
                    }
                }
            }
            for (int y = 0; y < board.Length; y++)
            {
                for (int x = 0; x < board[y].Length; x++)
                {
                    if (board[y][x] != null && board[y][x].Color != kingColor)
                    {
                        if (CheckMove(x, y, kingX, kingY))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckMate(PieceColor kingColor)
        {
            if (Check(kingColor))
            {
                //Find King
                int kingX = 0;
                int kingY = 0;
                ChessPiece king = new ChessPiece(Pieces.K, kingColor);
                for (int y = 0; y < board.Length; y++)
                {
                    for (int x = 0; x < board[y].Length; x++)
                    {
                        if (board[y][x] != null && board[y][x].Piece == Pieces.K && board[y][x].Color == kingColor)
                        {
                            kingX = x;
                            kingY = y;
                            king = board[y][x];
                        }
                    }
                }

                //Try moves to get out of check
                for (int startY = 0; startY < board.Length; startY++)
                {
                    for (int startX = 0; startX < board[startY].Length; startX++)
                    {
                        if (board[startY][startX] != null && board[startY][startX].Color == kingColor)
                        {
                            for (int toY = 0; toY < board.Length; toY++)
                            {
                                for (int toX = 0; toX < board[toY].Length; toX++)
                                {
                                    Board boardClone = this.Clone();
                                    if (boardClone.Move(startX, startY, toX, toY) &&
                                        !boardClone.Check(kingColor))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public Board Clone()
        {
            Board newBoard = new Board()
            {
                lastColor = this.lastColor
            };
            for (int y = 0; y < board.Length; y++)
            {
                for (int x = 0; x < board[y].Length; x++)
                {
                    newBoard[x, y] = board[y][x];
                }
            }
            return newBoard;
        }
        public void Copy(Board otherBoard)
        {
            for (int y = 0; y < board.Length; y++)
            {
                for (int x = 0; x < board[y].Length; x++)
                {
                    board[y][x] = otherBoard[x, y];
                }
            }
            lastColor = otherBoard.lastColor;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            //for (int y = 0; y < board[0].Length; y++) {
            //    for (int x = 0; x < board.Length; x++) {
            //        if (board[x][y] == null) {
            //            output.Append("-");
            //        } else {
            //            output.Append(board[x][y].ToString());
            //        }
            //    }
            //    if (y != 7) {
            //        output.Append("\n");
            //    }
            //}
            foreach (ChessPiece[] row in board)
            {
                foreach (ChessPiece piece in row)
                {
                    if (piece == null)
                    {
                        output.Append("-");
                    }
                    else
                    {
                        output.Append(piece.ToString());
                    }
                }
                if (row != board.Last())
                {
                    output.Append("\n");
                }
            }
            return output.ToString();
        }

        public bool Move(int locationX, int locationY, int toX, int toY)
        {
            ChessPiece movingPiece = this[locationX, locationY];
            if (movingPiece != null)
            {
                if (this.CheckMove(locationX, locationY, toX, toY) && movingPiece.Color != lastColor)
                {
                    Board boardClone = this.Clone();
                    boardClone[locationX, locationY] = null;
                    boardClone[toX, toY] = movingPiece;
                    if (!boardClone.Check(movingPiece.Color))
                    {
                        this.Copy(boardClone);
                        lastColor = movingPiece.Color;
                        return true;
                    }
                }
            }
            return false;
        }
        private bool CheckMove(int locationX, int locationY, int toX, int toY)
        {
            bool result = false;
            switch (this[locationX, locationY].Piece)
            {
                case Pieces.K:
                    result = MoveKing(locationX, locationY, toX, toY);
                    break;
                case Pieces.Q:
                    result = MoveQueen(locationX, locationY, toX, toY);
                    break;
                case Pieces.B:
                    result = MoveBishop(locationX, locationY, toX, toY);
                    break;
                case Pieces.N:
                    result = MoveKnight(locationX, locationY, toX, toY);
                    break;
                case Pieces.R:
                    result = MoveRook(locationX, locationY, toX, toY);
                    break;
                case Pieces.P:
                    result = MovePawn(locationX, locationY, toX, toY);
                    break;
            }
            return result;
        }

        private bool MovePawn(int locationX, int locationY, int toX, int toY)
        {
            int curX = locationY;
            int curY = locationX;
            int nX = toY;
            int nY = toX;
            ChessPiece movingPiece = this[locationX, locationY];
            int colorCoefficient = 1;
            if (movingPiece.Color == PieceColor.D)
            {
                colorCoefficient = -1;
            }
            if (curX == nX)
            {
                //Two-space movement check
                if ((curY == 1 || curY == 6) && nY - curY == 2 * colorCoefficient)
                {
                    if (this[curY + colorCoefficient, curX] == null &&
                        this[curY + colorCoefficient * 2, curX] == null)
                    {
                        return true;
                    }
                }
                else if (nY - curY == colorCoefficient)
                {   //One-space movement check
                    if (this[(curY + colorCoefficient), curX] == null)
                    {
                        return true;
                    }
                }
            }
            else if (Math.Abs(nX - curX) == 1 && nY - curY == colorCoefficient)
            {     //Capture check
                if (this[nX, curY + colorCoefficient] != null && this[curY + colorCoefficient, nX].Color != this[curY, curX].Color)
                {
                    return true;
                }
            }
            return false;
        }

        private bool MoveRook(int locationX, int locationY, int toX, int toY)
        {
            int curX = locationY;
            int curY = locationX;
            int nX = toY;
            int nY = toX;
            if (curX == nX ^ curY == nY)
            {
                return CheckDirection(curX, curY, nX, nY);
            }
            else
            {
                return false;
            }
        }

        private bool MoveKnight(int locationX, int locationY, int toX, int toY)
        {
            bool isValidLocation = IsValidMoveKnight(locationX, locationY, toX, toY);
            ChessPiece placeMovedTo = this.GetPiece(toX, toY);
            bool isNotOccupiedByFriendlyPiece = placeMovedTo == null ||
                placeMovedTo.Color != this[locationX, locationY].Color;
            bool isValidMove = isValidLocation && isNotOccupiedByFriendlyPiece;
            return isValidMove;
        }

        private bool IsValidMoveKnight(int locationX, int locationY, int toX, int toY)
        {
            int curX = locationY;
            int curY = locationX;
            int nX = toY;
            int nY = toX;
            bool isValid;
            int absoluteValueX = Math.Abs(curY - nY);
            int absoluteValueY = Math.Abs(curX - nX);
            switch (absoluteValueX)
            {
                case 1:
                    isValid = absoluteValueY == 2;
                    break;
                case 2:
                    isValid = absoluteValueY == 1;
                    break;
                default:
                    isValid = false;
                    break;
            }
            return isValid;
        }

        private bool MoveBishop(int locationX, int locationY, int toX, int toY)
        {
            int curX = locationY;
            int curY = locationX;
            int nX = toY;
            int nY = toX;
            if (Math.Abs(curX - nX) == Math.Abs(curY - nY) && Math.Abs(curX - nX) != 0)
            {
                return CheckDirection(curX, curY, nX, nY);
            }
            else
            {
                return false;
            }
        }

        private bool MoveQueen(int locationX, int locationY, int toX, int toY)
        {
            if (locationX == toX && locationY == toY)
            {
                return false;
            }
            else
            {
                return CheckDirection(locationX, locationY, toX, toY);
            }
        }

        private bool MoveKing(int locationX, int locationY, int toX, int toY)
        {
            int curX = locationY;
            int curY = locationX;
            int nX = toY;
            int nY = toX;
            if (Math.Abs(curX - nX) < 2 && Math.Abs(curY - nY) < 2 &&
                !(curX == nX && curY == nY))
            {
                return this[nY, nX] == null || this[nY, nX].Color != this[curY, curX].Color;
            }
            else
            {
                return false;
            }
        }

        private bool CheckDirection(int locationX, int locationY, int toX, int toY)
        {
            return CheckDirection(locationX, locationY, toX, toY, this[locationX, locationY]);
        }
        private bool CheckDirection(int locationX, int locationY, int toX, int toY, ChessPiece startPiece)
        {
            if (locationX == toX && locationY == toY)
            { //main check method, basically if the piece is where it needs to be
                if (this[locationY, locationX] == null ||
                    this[locationX, locationY].Color != startPiece.Color)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if ((locationX != toX && locationY != toY) && //logic check moving in one direction only
              Math.Abs(locationX - toX) != Math.Abs(locationY - toY))
            {
                return false;
            }
            else if (this[locationY, locationX] == null || this[locationX, locationY] == startPiece)
            { //Rook and Bishop Logic if makes it
                if (locationX == toX)
                {
                    return CheckDirection(locationX, locationY + (Math.Abs(toY - locationY) / (toY - locationY)),
                        toX, toY, startPiece);
                }
                else if (locationY == toY)
                {
                    return CheckDirection(locationX + (Math.Abs(toX - locationX) / (toX - locationX)), locationY,//Calls in a loop where is true?
                        toX, toY, startPiece);
                }
                else
                {
                    return CheckDirection(locationX + (Math.Abs(toX - locationX) / (toX - locationX)),
                        locationY + (Math.Abs(toY - locationY) / (toY - locationY)), toX, toY, startPiece);
                }
            }
            else
            {
                return false;
            }
        }
    }
}
