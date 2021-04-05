using System;
using ChessMatchStateSubsystem.Enums;
using Microsoft.Xna.Framework.Graphics;

namespace ChessMatchStateSubsystem.Match.Pieces
{
    internal class WhiteKing : WhitePiece
    {
        public WhiteKing(ChessMatchState chessGameState, Chessboard chessboard) 
            : base(chessGameState, chessboard)
        { }

        protected override Model GetModel()
        {
            return ChessMatchState.WhiteKingModel;
        }

        public override bool CanMove(int toRow, int toColumn)
        {
            int rowDiff = Math.Abs(toRow - CurrentRow);
            int colDiff = Math.Abs(toColumn - CurrentColumn);

            if (rowDiff <= 1 && colDiff <= 1)
            {
                if (Chessboard[toRow, toColumn] == null ||
                   Chessboard[toRow, toColumn].Color == ChessPlayer.Black)
                {
                    return true;                    
                }
            }
            return false;
        }
    }
}
