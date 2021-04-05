using System;
using ChessMatchStateSubsystem.Enums;
using Microsoft.Xna.Framework.Graphics;

namespace ChessMatchStateSubsystem.Match.Pieces
{
    internal class WhiteQueen : WhitePiece
    {
        public WhiteQueen(ChessMatchState chessGameState, Chessboard chessboard) 
            : base(chessGameState, chessboard)
        { }

        protected override Model GetModel()
        {
            return ChessMatchState.WhiteQueenModel;
        }

        public override bool CanMove(int toRow, int toColumn)
        {
            if (toRow == CurrentRow && toColumn != CurrentColumn)
            {
                int start, stop;
                if (toColumn > CurrentColumn)
                {
                    start = CurrentColumn + 1;
                    stop = toColumn;
                }
                else
                {
                    start = toColumn + 1;
                    stop = CurrentColumn;
                }

                for (int i = start; i < stop; ++i)
                    if (Chessboard[toRow, i] != null)
                        return false;

                if (Chessboard[toRow, toColumn] == null ||
                    Chessboard[toRow, toColumn].Color == ChessPlayer.Black)
                    return true;
                return false;
            }

            if (toRow != CurrentRow && toColumn == CurrentColumn)
            {
                int start, stop;
                if (toRow > CurrentRow)
                {
                    start = CurrentRow + 1;
                    stop = toRow;
                }
                else
                {
                    start = toRow + 1;
                    stop = CurrentRow;
                }

                for (int i = start; i < stop; ++i)
                    if (Chessboard[i, toColumn] != null)
                        return false;

                if (Chessboard[toRow, toColumn] == null ||
                    Chessboard[toRow, toColumn].Color == ChessPlayer.Black)
                    return true;
                return false;
            }

            int rowDiff = Math.Abs(toRow - CurrentRow);
            int colDiff = Math.Abs(toColumn - CurrentColumn);

            if (rowDiff == colDiff)
            {
                int deltaRow = (toRow > CurrentRow ? 1 : -1);
                int deltaCol = (toColumn > CurrentColumn ? 1 : -1);

                int i = CurrentRow + deltaRow;
                int j = CurrentColumn + deltaCol;
                while (i != toRow && j != toColumn)
                {
                    if (Chessboard[i, j] != null)
                        return false;

                    i += deltaRow;
                    j += deltaCol;
                }

                if (Chessboard[toRow, toColumn] == null ||
                   Chessboard[toRow, toColumn].Color == ChessPlayer.Black)
                {
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
