using Microsoft.Xna.Framework;

namespace ChessMatchStateSubsystem
{
    internal interface IMatchState
    {
        IMatchState Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
