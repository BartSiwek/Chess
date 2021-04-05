using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace InputSubsystem
{
    public interface IInputManager : IGameComponent, IDisposable, IUpdateable
    {
        //Keyboard section
        bool IsKeyDown(Keys key);
        bool IsKeyUp(Keys key);
        bool IsKeyHoldDown(Keys key);

        //Mouse secion
        bool IsLeftMouseButtonDown();
        bool IsRightMouseButtonDown();
        bool IsLeftMouseButtonUp();
        bool IsRightMouseButtonUp();
        bool IsLeftMouseButtonHoldDown();
        bool IsRightMouseButtonHoldDown();
        Vector2 GetMousePosition();
    }
}
