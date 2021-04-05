using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsSubsystem
{
    public interface IGraphicsManager : IGameComponent
    {
        //Properties
        GraphicsDevice GraphicsDevice { get; }
        GraphicsDeviceManager GraphicsDeviceManager { get; }
        int Width { get; }
        int Height { get; }
        ViewportSizePreset ViewportSize { get; set; }

        //Mouse cursor stuff
        bool IsMouseIn(Vector2 mousePosition);

        //Camera movement
        void ResetCamera();
        void CameraLeft(GameTime gameTime);
        void CameraRight(GameTime gameTime);
        void CameraUp(GameTime gameTime);
        void CameraDown(GameTime gameTime);
        bool MoveToWhiteViewport(GameTime gameTime);
        bool MoveToBlackViewport(GameTime gameTime);

        //Display stuff
        void PrepareBackBuffer();
        void ShowBackBuffer();
        Texture2D GetScreenContents();
        void UnloadScreenContents(Texture2D screen);
        void DrawModel(Model model, ref Matrix world, Effect effect);
        void DrawStringOnScreen(string message, SpriteFont font);

        //Load objects
        SpriteBatch GetSpriteBatch();

        //Unload objects
        void UnloadSpriteBatch(SpriteBatch batch);
    }
}
