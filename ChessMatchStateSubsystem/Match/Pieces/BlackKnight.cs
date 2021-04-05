using ChessMatchStateSubsystem.Enums;
using Microsoft.Xna.Framework.Graphics;

namespace ChessMatchStateSubsystem.Match.Pieces
{
    class BlackKnight : BlackPiece
    {
        public BlackKnight(ChessMatchState chessGameState, Chessboard chessboard) 
            : base(chessGameState, chessboard)
        { }

        protected override Model GetModel()
        {
            return ChessMatchState.BlackKnightModel;
        }

        public override bool CanMove(int toRow, int toColumn)
        {
            bool moveOk = false;
            if(toRow == CurrentRow + 2 && toColumn == CurrentColumn + 1)
                moveOk = true;
            if (toRow == CurrentRow + 2 && toColumn == CurrentColumn - 1)
                moveOk = true;
            if (toRow == CurrentRow + 1 && toColumn == CurrentColumn + 2)
                moveOk = true;
            if (toRow == CurrentRow + 1 && toColumn == CurrentColumn - 2)
                moveOk = true;
            if (toRow == CurrentRow - 1 && toColumn == CurrentColumn + 2)
                moveOk = true;
            if (toRow == CurrentRow - 1 && toColumn == CurrentColumn - 2)
                moveOk = true;
            if (toRow == CurrentRow - 2 && toColumn == CurrentColumn + 1)
                moveOk = true;
            if (toRow == CurrentRow - 2 && toColumn == CurrentColumn - 1)
                moveOk = true;

            if (moveOk)
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
