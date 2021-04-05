using System;
using GraphicsSubsystem.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsSubsystem
{
    public class GraphicsManager : DrawableGameComponent, IGraphicsManager
    {
        private const float s_HorizontalDegreesPerSecond = 10.0f;
        private const float s_VerticalDegreesPerSecond = 10.0f;
        private const float s_TransferHorizontalDegreesPerSecond = 20.0f;
        private const float s_TransferVerticalDegreesPerSecond = 20.0f;
        private const float s_WhitePlayerPhi = 90.0f;
        private const float s_WhitePlayerTheta = 45.0f;
        private const float s_BlackPlayerPhi = 270.0f;
        private const float s_BlackPlayerTheta = 45.0f;

        private readonly GraphicsDeviceManager m_graphicsDeviceManager;
        private readonly Camera m_camera;
        private GraphicsDevice m_graphicsDevice;
        private RenderTarget2D m_renderTarget;
        private SpriteBatch m_renderSpriteBatch;
        private SpriteBatch m_messageSpriteBatch;
        private bool m_disposed;

        public GraphicsManager(Game game)
            : base(game)
        {
            m_graphicsDeviceManager = (GraphicsDeviceManager)game.Services.GetService(typeof(IGraphicsDeviceManager));
            m_camera = new Camera(this);

            m_graphicsDeviceManager.IsFullScreen = true;
            m_graphicsDeviceManager.PreferMultiSampling = true;

            Game.IsFixedTimeStep = true;
            Game.IsMouseVisible = true;

            ViewportSizePreset viewportSizePreset = (ViewportSizePreset)Settings.Default.VieportSize;
            switch (viewportSizePreset)
            {
                case ViewportSizePreset.Size800x600:
                    m_graphicsDeviceManager.PreferredBackBufferWidth = 800;
                    m_graphicsDeviceManager.PreferredBackBufferHeight = 600;
                    break;
                case ViewportSizePreset.Size1024x768:
                    m_graphicsDeviceManager.PreferredBackBufferWidth = 1024;
                    m_graphicsDeviceManager.PreferredBackBufferHeight = 768;
                    break;
                case ViewportSizePreset.Size1280x960:
                    m_graphicsDeviceManager.PreferredBackBufferWidth = 1280;
                    m_graphicsDeviceManager.PreferredBackBufferHeight = 960;
                    break;
            }
        }

        public override void Initialize()
        {
            //Get the device
            m_graphicsDevice = Game.GraphicsDevice;
            m_graphicsDevice.RenderState.MultiSampleAntiAlias = true;
            m_graphicsDeviceManager.ApplyChanges();
            
            //Initialize the camera
            m_camera.Initialize();

            //Go with the base initialization
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            m_renderTarget = new RenderTarget2D(
                m_graphicsDevice,
                m_graphicsDevice.Viewport.Width,
                m_graphicsDevice.Viewport.Height,
                1,
                m_graphicsDevice.DisplayMode.Format,
                m_graphicsDevice.PresentationParameters.MultiSampleType,
                m_graphicsDevice.PresentationParameters.MultiSampleQuality,
                RenderTargetUsage.PreserveContents
            );
            m_renderSpriteBatch = new SpriteBatch(m_graphicsDevice);
            m_messageSpriteBatch = new SpriteBatch(m_graphicsDevice);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();

            m_renderTarget.Dispose();
            m_renderSpriteBatch.Dispose();
            m_messageSpriteBatch.Dispose();

            m_graphicsDevice = null;
            m_renderTarget = null;
            m_renderSpriteBatch = null;
            m_messageSpriteBatch = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !m_disposed)
            {
                //Save settings
                Settings.Default.Save();

                //Do the disposing
                m_disposed = true;
            }
        }

        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get
            {
                return m_graphicsDeviceManager;
            }
        }

        public int Width
        {
            get
            {
                CheckDisposed();
                return m_graphicsDevice.Viewport.Width;
            }
        }

        public int Height
        {
            get
            {
                CheckDisposed();
                return m_graphicsDevice.Viewport.Height;
            }
        }

        public ViewportSizePreset ViewportSize
        {
            set
            {
                Settings.Default.VieportSize = (int)value;
            }
            get
            {
                return (ViewportSizePreset) Settings.Default.VieportSize;
            }
        }

        public bool IsMouseIn(Vector2 mousePosition)
        {
            CheckDisposed();

            if (mousePosition.X < 0 || mousePosition.X > Width)
                return false;
            if (mousePosition.Y < 0 || mousePosition.Y > Height)
                return false;
            return true;
        }

        public void ResetCamera()
        {
            m_camera.Theta = s_WhitePlayerTheta;
            m_camera.Phi = s_WhitePlayerPhi;
        }

        public void CameraLeft(GameTime gameTime)
        {
            m_camera.Phi += (float)(s_HorizontalDegreesPerSecond * gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void CameraRight(GameTime gameTime)
        {
            m_camera.Phi -= (float)(s_HorizontalDegreesPerSecond * gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void CameraUp(GameTime gameTime)
        {
            m_camera.Theta += (float)(s_VerticalDegreesPerSecond * gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void CameraDown(GameTime gameTime)
        {
            m_camera.Theta -= (float)(s_VerticalDegreesPerSecond * gameTime.ElapsedGameTime.TotalSeconds);
        }

        public bool MoveToWhiteViewport(GameTime gameTime)
        {
            if(m_camera.Theta == s_WhitePlayerTheta && m_camera.Phi == s_WhitePlayerPhi)
                return true;

            float thetaDiff = s_WhitePlayerTheta - m_camera.Theta;
            float phiDiff = s_WhitePlayerPhi - m_camera.Phi;

            float thetaChangeOverTime = (float)(s_TransferHorizontalDegreesPerSecond * gameTime.ElapsedGameTime.TotalSeconds);
            float phiChangeOverTime = (float)(s_TransferVerticalDegreesPerSecond * gameTime.ElapsedGameTime.TotalSeconds);

            float thetaChange = Math.Min(Math.Abs(thetaDiff), thetaChangeOverTime);
            float phiChange = Math.Min(Math.Abs(phiDiff), phiChangeOverTime);

            if (thetaDiff > 0)
                m_camera.Theta += thetaChange;
            else
                m_camera.Theta -= thetaChange;

            if (phiDiff > 0)
                m_camera.Phi += phiChange;
            else
                m_camera.Phi -= phiChange;

            return false;
        }

        public bool MoveToBlackViewport(GameTime gameTime)
        {
            if (m_camera.Theta == s_BlackPlayerTheta && m_camera.Phi == s_BlackPlayerPhi)
                return true;

            float thetaDiff = s_BlackPlayerTheta - m_camera.Theta;
            float phiDiff = s_BlackPlayerPhi - m_camera.Phi;

            float thetaChangeOverTime = (float)(s_TransferHorizontalDegreesPerSecond * gameTime.ElapsedGameTime.TotalSeconds);
            float phiChangeOverTime = (float)(s_TransferVerticalDegreesPerSecond * gameTime.ElapsedGameTime.TotalSeconds);

            float thetaChange = Math.Min(Math.Abs(thetaDiff), thetaChangeOverTime);
            float phiChange = Math.Min(Math.Abs(phiDiff), phiChangeOverTime);

            if (thetaDiff > 0)
                m_camera.Theta += thetaChange;
            else
                m_camera.Theta -= thetaChange;

            if (phiDiff > 0)
                m_camera.Phi += phiChange;
            else
                m_camera.Phi -= phiChange;

            return false;
        }

        public void PrepareBackBuffer()
        {
            CheckDisposed();

            //Enable drawing to backbuffer
            m_graphicsDevice.SetRenderTarget(0, m_renderTarget);
            m_graphicsDevice.Clear(Color.Black);
        }

        public void ShowBackBuffer()
        {
            CheckDisposed();

            //Endble drawing on screen
            m_graphicsDevice.SetRenderTarget(0, null);
            m_graphicsDevice.Clear(Color.Black);

            m_renderSpriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            m_renderSpriteBatch.Draw(m_renderTarget.GetTexture(), Vector2.Zero, Color.White);
            m_renderSpriteBatch.End();
        }

        public Texture2D GetScreenContents()
        {
            CheckDisposed();

            m_graphicsDevice.SetRenderTarget(0, null);

            ResolveTexture2D retValue = new ResolveTexture2D(m_graphicsDevice, m_renderTarget.Width, m_renderTarget.Height, 1, m_renderTarget.Format);
            m_graphicsDevice.ResolveBackBuffer(retValue);

            m_graphicsDevice.SetRenderTarget(0, m_renderTarget);
            m_graphicsDevice.Clear(Color.Black);

            return retValue;
        }

        public void UnloadScreenContents(Texture2D screen)
        {
            CheckDisposed();

            screen.Dispose();
        }

        public void DrawModel(Model model, ref Matrix world, Effect effect)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                if (effect != null)
                {
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        meshPart.Effect = effect;
                    }
                }

                foreach (Effect currentEffect in mesh.Effects)
                {
                    if (currentEffect is BasicEffect)
                    {
                        BasicEffect currentBasicEffect = (BasicEffect) currentEffect;
                        currentBasicEffect.World = transforms[mesh.ParentBone.Index] * world;
                        currentBasicEffect.Projection = m_camera.Projection;
                        currentBasicEffect.View = m_camera.View;
                    }
                    else
                    {
                        if(currentEffect.Parameters["World"] != null)
                        {
                            currentEffect.Parameters["World"].SetValue(
                                transforms[mesh.ParentBone.Index] * world
                            );
                        }
                        if(currentEffect.Parameters["Projection"] != null)
                        {
                            currentEffect.Parameters["Projection"].SetValue(
                                m_camera.Projection
                            );
                        }
                        if(currentEffect.Parameters["View"] != null)
                        {
                            currentEffect.Parameters["View"].SetValue(
                                m_camera.View
                            );
                        }
                        if(currentEffect.Parameters["WorldViewProjection"] != null)
                        {
                            currentEffect.Parameters["WorldViewProjection"].SetValue(
                                world * m_camera.View * m_camera.Projection
                            );
                        }
                        if(currentEffect.Parameters["WorldInverseTranspose"] != null)
                        {
                            currentEffect.Parameters["WorldInverseTranspose"].SetValue(
                                Matrix.Transpose(Matrix.Invert(world))
                            );
                        }
                        if(currentEffect.Parameters["ViewInverse"] != null)
                        {
                            currentEffect.Parameters["ViewInverse"].SetValue(
                                Matrix.Invert(m_camera.View)
                            );
                        }
                    }
                }

                mesh.Draw();
            }
        }

        public void DrawStringOnScreen(string message, SpriteFont font)
        {
            //Measure a string
            Vector2 messagePosition = font.MeasureString(message);
            Vector2 shift = new Vector2(3.0f, 3.0f);

            //Calcualte the position
            messagePosition.X = (Width - messagePosition.X)/2;
            messagePosition.Y = (Height - messagePosition.Y) / 2;

            //Do the drawing
            m_messageSpriteBatch.Begin();
            m_messageSpriteBatch.DrawString(font, message, messagePosition - shift, Color.Black);
            m_messageSpriteBatch.DrawString(font, message, messagePosition, Color.White);
            m_messageSpriteBatch.End();
        }

        public SpriteBatch GetSpriteBatch()
        {
            CheckDisposed();

            return new SpriteBatch(m_graphicsDevice);
        }

        public void UnloadSpriteBatch(SpriteBatch batch)
        {
            if (m_disposed)
                return;

            batch.Dispose();
        }

        private void CheckDisposed()
        {
            if (m_disposed)
                throw new ObjectDisposedException("The graphics manager has been disposed");
        }
    }
}