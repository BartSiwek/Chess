using Microsoft.Xna.Framework;

namespace InputSubsystem
{
    public static class InputManagerFactory
    {
        private static IInputManager s_inputManager = null;

        public static IInputManager GetInputManager(Game game)
        {
            if(s_inputManager == null)
            {
                s_inputManager = new InputManager(game);
                game.Services.AddService(typeof(IInputManager), s_inputManager);
            }
            return s_inputManager;
        }
    }
}
