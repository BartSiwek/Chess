using ChessMatchStateSubsystem.Enums;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ChessMatchStateSubsystem.Match.Pieces
{
    internal class WhiteBishop : WhitePiece
    {
        public WhiteBishop(ChessMatchState chessGameState, Chessboard chessboard) : 
            base(chessGameState, chessboard)
        { }

        protected override Model GetModel()
        {
            return ChessMatchState.WhiteBishopModel;
        }

        public override bool CanMove(int toRow, int toColumn)
        {
            int rowDiff = Math.Abs(toRow - CurrentRow);
            int colDiff = Math.Abs(toColumn - CurrentColumn);

            if(rowDiff == colDiff)
            {
                int deltaRow = (toRow > CurrentRow ? 1 : -1);
                int deltaCol = (toColumn > CurrentColumn ? 1 : -1);

                int i = CurrentRow + deltaRow;
                int j = CurrentColumn + deltaCol;
                while (i != toRow && j != toColumn)
                {
                    if(Chessboard[i, j] != null)
                        return false;

                    i += deltaRow;
                    j += deltaCol;
                }

                if(Chessboard[toRow, toColumn] == null ||
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
