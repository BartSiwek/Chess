using Microsoft.Xna.Framework;

namespace ResourceSubsystem
{
    public static class ResourceManagerFactory
    {
        private static IResourceManager s_resourceManager;

        public static IResourceManager GetResourceManager(Game game)
        {
            if (s_resourceManager == null)
            {
                s_resourceManager = new ResourceManager(game);
                game.Services.AddService(typeof(IResourceManager), s_resourceManager);
            }
            return s_resourceManager;
        }
    }
}
