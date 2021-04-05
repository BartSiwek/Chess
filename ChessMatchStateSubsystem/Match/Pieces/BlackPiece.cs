using ChessMatchStateSubsystem.Enums;
using ChessMatchStateSubsystem.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessMatchStateSubsystem.Match.Pieces
{
    internal abstract class BlackPiece : ChessPiece
    {
        protected BlackPiece(ChessMatchState chessGameState, Chessboard chessboard)
            : base(chessGameState, ChessPlayer.Black, chessboard)
        { }

        protected override Texture2D GetTexture()
        {
            return ChessMatchState.BlackPiecesTexture;
        }

        protected override Matrix GetTransformationMatrix()
        {
            Matrix translation = Matrix.CreateRotationY(MathHelper.Pi);;
            translation = translation * Matrix.CreateTranslation(
                ChessboardConstants.A1X + CurrentColumn * ChessboardConstants.FiledSize,
                0.0f,
                ChessboardConstants.A1Z - CurrentRow * ChessboardConstants.FiledSize);

            return translation;
        }
    }
}
