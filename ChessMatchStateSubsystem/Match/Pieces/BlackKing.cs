using System;
using ChessMatchStateSubsystem.Enums;
using Microsoft.Xna.Framework.Graphics;

namespace ChessMatchStateSubsystem.Match.Pieces
{
    internal class BlackKing : BlackPiece
    {
        public BlackKing(ChessMatchState chessGameState, Chessboard chessboard) 
            : base(chessGameState, chessboard)
        { }

        protected override Model GetModel()
        {
            return ChessMatchState.BlackKingModel;
        }

        public override bool CanMove(int toRow, int toColumn)
        {
            int rowDiff = Math.Abs(toRow - CurrentRow);
            int colDiff = Math.Abs(toColumn - CurrentColumn);

            if (rowDiff <= 1 && colDiff <= 1)
            {
                if (Chessboard[toRow, toColumn] == null ||
                    Chessboard[toRow, toColumn].Color == ChessPlayer.White)
                {
                    return true;
                }                
            }
            return false;
        }
    }
}
