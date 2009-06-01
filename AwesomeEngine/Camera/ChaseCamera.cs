using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace AwesomeEngine.Camera
{
    public class ChaseCamera// : Camera
    {
        private Vector3 chasePosition;
        private Vector3 chaseDirection;
        private Vector3 up = Vector3.Up;
        private Vector3 desiredPositionOffset = new Vector3(0, 2.0f, 2.0f);
        private Vector3 desiredPosition;
        private Vector3 lookAtOffset = new Vector3(0, 2.8f, 0);
        private Vector3 lookAt;
        private float stiffness = 1800.0f;
        private float damping = 600.0f;
        private float mass = 50.0f;
        private Vector3 position;
        private Vector3 velocity;
        private float aspectRatio = 4.0f / 3.0f;
        private float fieldOfView = MathHelper.ToRadians(45.0f);
        private float nearPlaneDistance = 1.0f;
        private float farPlaneDistance = 10000.0f;
        private Matrix view;
        private Matrix projection;

        //public ChaseCamera()
        {
        }

        public Vector3 ChasePosition
        {
            get { return chasePosition; }
            set { chasePosition = value; }
        }

        public Vector3 ChaseDirection
        {
            get { return chaseDirection; }
            set { chaseDirection = value; }
        }
        
        public Vector3 Up
        {
            get { return up; }
            set { up = value; }
        }

        public Vector3 DesiredPositionOffset
        {
            get { return desiredPositionOffset; }
            set { desiredPositionOffset = value; }
        }        

        public Vector3 DesiredPosition
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return desiredPosition;
            }
        }
        
        public Vector3 LookAtOffset
        {
            get { return lookAtOffset; }
            set { lookAtOffset = value; }
        }        

        public Vector3 LookAt
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return lookAt;
            }
        }

        public float Stiffness
        {
            get { return stiffness; }
            set { stiffness = value; }
        }
        
        public float Damping
        {
            get { return damping; }
            set { damping = value; }
        }
        
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }        

        public Vector3 Position
        {
            get { return position; }
        }
        
        public Vector3 Velocity
        {
            get { return velocity; }
        }
        
        public float AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value; }
        }
        
        public float FieldOfView
        {
            get { return fieldOfView; }
            set { fieldOfView = value; }
        }
        
        public float NearPlaneDistance
        {
            get { return nearPlaneDistance; }
            set { nearPlaneDistance = value; }
        }
        
        public float FarPlaneDistance
        {
            get { return farPlaneDistance; }
            set { farPlaneDistance = value; }
        }
        
        public Matrix View
        {
            get { return view; }
        }
        
        public Matrix Projection
        {
            get { return projection; }
        }
        
        private void UpdateWorldPositions()
        {
            // Construct a matrix to transform from object space to worldspace
            Matrix transform = Matrix.Identity;
            transform.Forward = ChaseDirection;
            transform.Up = Up;
            transform.Right = Vector3.Cross(Up, ChaseDirection);

            // Calculate desired camera properties in world space
            desiredPosition = ChasePosition +
                Vector3.TransformNormal(DesiredPositionOffset, transform);
            lookAt = ChasePosition +
                Vector3.TransformNormal(LookAtOffset, transform);
        }

        private void UpdateMatrices()
        {
            view = Matrix.CreateLookAt(this.Position, this.LookAt, this.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView,
                AspectRatio, NearPlaneDistance, FarPlaneDistance);
        }

        public void Reset()
        {
            UpdateWorldPositions();

            // Stop motion
            velocity = Vector3.Zero;

            // Force desired position
            position = desiredPosition;

            UpdateMatrices();
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime == null)
                throw new ArgumentNullException("gameTime");

            UpdateWorldPositions();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate spring force
            Vector3 stretch = position - desiredPosition;
            Vector3 force = -stiffness * stretch - damping * velocity;

            // Apply acceleration
            Vector3 acceleration = force / mass;
            velocity += acceleration * elapsed;

            // Apply velocity
            position += velocity * elapsed;

            UpdateMatrices();
        }
    }
}