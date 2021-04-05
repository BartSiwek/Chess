using ChessMatchStateSubsystem.Enums;
using ChessMatchStateSubsystem.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessMatchStateSubsystem.Match.Pieces
{
    internal abstract class WhitePiece : ChessPiece
    {
        protected WhitePiece(ChessMatchState chessGameState, Chessboard chessboard)
            : base(chessGameState, ChessPlayer.White, chessboard)
        { }

        protected override Texture2D GetTexture()
        {
            return ChessMatchState.WhitePiecesTexture;
        }

        protected override Matrix GetTransformationMatrix()
        {
            Matrix translation = Matrix.CreateTranslation(
                ChessboardConstants.A1X + CurrentColumn * ChessboardConstants.FiledSize,
                0.0f,
                ChessboardConstants.A1Z - CurrentRow * ChessboardConstants.FiledSize);
            return translation;
        }
    }
}
