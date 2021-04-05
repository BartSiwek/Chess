using System;
using Microsoft.Xna.Framework;

namespace GameStateManagmentSubsystem
{
    public interface IGameState : IDisposable
    {
        void LoadContent();
        void Activated();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        void Deactivated();
        void UnloadContent();
    }
}
