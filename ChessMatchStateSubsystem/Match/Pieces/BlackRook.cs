using ChessMatchStateSubsystem.Enums;
using Microsoft.Xna.Framework.Graphics;

namespace ChessMatchStateSubsystem.Match.Pieces
{
    internal class BlackRook : BlackPiece
    {
        public BlackRook(ChessMatchState chessGameState, Chessboard chessboard)
            : base(chessGameState, chessboard)
        { }

        protected override Model GetModel()
        {
            return ChessMatchState.BlackRookModel;
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
                    Chessboard[toRow, toColumn].Color == ChessPlayer.White)
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
                    Chessboard[toRow, toColumn].Color == ChessPlayer.White)
                    return true;
                return false;
            }

            return false;
        }
    }
}
