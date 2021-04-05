using ChessMatchStateSubsystem.Enums;
using Microsoft.Xna.Framework.Graphics;

namespace ChessMatchStateSubsystem.Match.Pieces
{
    internal class BlackPawn : BlackPiece
    {
        public BlackPawn(ChessMatchState chessGameState, Chessboard chessboard)
            : base(chessGameState, chessboard)
        { }

        protected override Model GetModel()
        {
            return ChessMatchState.BlackPawnModel;
        }

        public override bool CanMove(int toRow, int toColumn)
        {
            if(toRow == CurrentRow - 1 && toColumn == CurrentColumn)
            {
                if(Chessboard[toRow, toColumn] == null)
                {
                    return true;   
                }
                return false;
            }

            if (toRow == CurrentRow - 2 && 
                toColumn == CurrentColumn &&
                Chessboard[CurrentRow - 1, CurrentColumn] == null &&
                FirstMove)
            {
                if (Chessboard[toRow, toColumn] == null)
                {
                    return true;
                }
                return false;
            }

            if (toRow == CurrentRow - 1 && 
                toColumn == CurrentColumn - 1 && 
                Chessboard[toRow, toColumn] != null &&
                Chessboard[toRow, toColumn].Color == ChessPlayer.White)
            {
                return true;
            }

            if (toRow == CurrentRow - 1 && 
                toColumn == CurrentColumn + 1 && 
                Chessboard[toRow, toColumn] != null &&
                Chessboard[toRow, toColumn].Color == ChessPlayer.White)
            {
                return true;
            }
            return false;
        }
    }
}
