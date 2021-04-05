using System;
using Microsoft.Xna.Framework;

namespace GraphicsSubsystem
{
    public class Camera
    {
        #region Storage

        private readonly IGraphicsManager m_graphicsManager;
        private Matrix m_projection;
        private Matrix m_view;
        private float m_radius = 70.0f;
        private float m_phi = 90.0f;
        private float m_theta = 45.0f;
        private Vector3 m_cameraPosition = Vector3.One;
        private Vector3 m_cameraTarget = Vector3.Zero;
        private Vector3 m_cameraUpVector = Vector3.Up;

        #endregion

        #region Constuction

        public Camera(IGraphicsManager graphicsManager)
        {
            m_graphicsManager = graphicsManager;
        }

        #endregion

        #region Properties

        public Matrix View
        {
            get { return m_view; }
        }

        public Matrix Projection
        {
            get { return m_projection; }
        }

        public Vector3 CameraPosition
        {
            get { return m_cameraPosition; }
        }

        public float Radius
        {
            get { return m_radius; }
            set { m_radius = value; }
        }

        public float Phi
        {
            get { return m_phi; }
            set
            {
                m_phi = value;
                if (m_phi >= 360.0)
                    m_phi -= 360.0f;
                if (m_phi < 0)
                    m_phi += 360.0f;
                GenerateLookAt();
            }
        }

        public float Theta
        {
            get { return m_theta; }
            set
            {
                m_theta = value;
                if (m_theta > 65.0f)
                    m_theta = 65.0f;
                if (m_theta < 10.0f)
                    m_theta = 10.0f;
                GenerateLookAt();
            }
        }

        #endregion

        #region Methods

        public void Initialize()
        {
            float aspectRation = (float) m_graphicsManager.Width/(float) m_graphicsManager.Height;
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRation, 1.0f, 10000.0f, out m_projection);
            GenerateLookAt();
        }

        private void GenerateLookAt()
        {
            float thetaRadians = MathHelper.ToRadians(m_theta);
            float phiRadians = MathHelper.ToRadians(m_phi);

            m_cameraPosition = new Vector3(
                (float)(m_radius * Math.Cos(thetaRadians) * Math.Cos(phiRadians)),
                (float)(m_radius * Math.Sin(thetaRadians)),
                (float)(m_radius * Math.Cos(thetaRadians) * Math.Sin(phiRadians))
            );
            Matrix.CreateLookAt(ref m_cameraPosition, ref m_cameraTarget, ref m_cameraUpVector, out m_view);
        }

        #endregion
    }
}
