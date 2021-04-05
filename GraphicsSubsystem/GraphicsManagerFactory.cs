using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphicsSubsystem
{
    public static class GraphicsManagerFactory
    {
        private static GraphicsManager s_graphicsManager;

        public static IGraphicsManager GetGraphicsManager(Game game)
        {
            if(s_graphicsManager == null)
            {
                s_graphicsManager = new GraphicsManager(game);
                game.Services.AddService(typeof(IGraphicsManager), s_graphicsManager);
            }
            return s_graphicsManager;
        }
    }
}
