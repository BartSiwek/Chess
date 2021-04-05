using System;
using Microsoft.Xna.Framework;

namespace GameStateManagmentSubsystem
{
    public interface IGameStateManager : IGameComponent, IDisposable, IUpdateable, IDrawable
    {
        void Push(IGameState state);
        void Change(IGameState state);
        void Pop();
        IGameState Top { get; }
        int StateCount { get; }
        IGameState this[int index] { get; }
    }
}
