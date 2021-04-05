using Microsoft.Xna.Framework;

namespace GameStateManagmentSubsystem
{
    public static class GameStateManagerFactory
    {
        private static GameStateManager s_gameStateManager;

        public static IGameStateManager GetGameStateManager(Game game)
        {
            if(s_gameStateManager == null)
            {
                s_gameStateManager = new GameStateManager(game); 
                game.Services.AddService(typeof(IGameStateManager), s_gameStateManager);
            }
            return s_gameStateManager;
        }
    }
}
