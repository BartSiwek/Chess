using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace InputSubsystem
{
    internal class InputManager : GameComponent, IInputManager
    {
        private KeyboardState m_prevKeyboardState;
        private KeyboardState m_currentKeyboardState;
        private MouseState m_prevMouseState;
        private MouseState m_currentMouseState;

        public InputManager(Game game) : base(game)
        { }

        public override void Initialize()
        {
            base.Initialize();

            m_currentKeyboardState = m_prevKeyboardState = Keyboard.GetState();
            m_currentMouseState = m_prevMouseState = Mouse.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(Game.IsActive)
            {
                //Update keyboard
                m_prevKeyboardState = m_currentKeyboardState;
                m_currentKeyboardState = Keyboard.GetState();

                //Update mouse
                m_prevMouseState = m_currentMouseState;
                m_currentMouseState = Mouse.GetState();
            }
        }

        public bool IsKeyDown(Keys key)
        {
            return m_currentKeyboardState.IsKeyDown(key) && !m_prevKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return m_currentKeyboardState.IsKeyUp(key) && !m_prevKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyHoldDown(Keys key)
        {
            return m_prevKeyboardState.IsKeyDown(key) && m_currentKeyboardState.IsKeyDown(key);
        }

        public bool IsLeftMouseButtonDown()
        {
            return (m_currentMouseState.LeftButton == ButtonState.Pressed) &&
                   (m_prevMouseState.LeftButton == ButtonState.Released);
        }

        public bool IsRightMouseButtonDown()
        {
            return (m_currentMouseState.RightButton == ButtonState.Pressed) &&
                   (m_prevMouseState.RightButton == ButtonState.Released);
        }

        public bool IsLeftMouseButtonUp()
        {
            return (m_currentMouseState.LeftButton == ButtonState.Released) &&
                   (m_prevMouseState.LeftButton == ButtonState.Pressed);
        }

        public bool IsRightMouseButtonUp()
        {
            return (m_currentMouseState.RightButton == ButtonState.Released) &&
                   (m_prevMouseState.RightButton == ButtonState.Pressed);
        }

        public bool IsLeftMouseButtonHoldDown()
        {
            return (m_prevMouseState.LeftButton == ButtonState.Pressed) &&
                   (m_currentMouseState.LeftButton == ButtonState.Pressed);
        }

        public bool IsRightMouseButtonHoldDown()
        {
            return (m_prevMouseState.RightButton == ButtonState.Pressed) &&
                   (m_currentMouseState.RightButton == ButtonState.Pressed);
        }

        public Vector2 GetMousePosition()
        {
            return new Vector2(m_currentMouseState.X, m_currentMouseState.Y);
        }
    }
}
