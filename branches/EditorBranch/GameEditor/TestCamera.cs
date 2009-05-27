/*using System;
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
using GameEditor;

namespace GameEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TestCamera : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
		ReferenceGrid grid;
		Matrix cameraProjectionMatrix;
		Matrix cameraViewMatrix;

        private const float FLOOR_WIDTH = 8.0f;
        private const float FLOOR_HEIGHT = 8.0f;
        private const float FLOOR_TILE_U = 8.0f;
        private const float FLOOR_TILE_V = 8.0f;
        private const float CAMERA_FOV = 90.0f;
        private const float CAMERA_ZNEAR = 0.01f;
        private const float CAMERA_ZFAR = 100.0f;
        private const float CAMERA_OFFSET = 0.5f;
        private const float CAMERA_BOUNDS_MIN_X = -FLOOR_WIDTH / 2.0f;
        private const float CAMERA_BOUNDS_MAX_X = FLOOR_WIDTH / 2.0f;
        private const float CAMERA_BOUNDS_MIN_Y = CAMERA_OFFSET;
        private const float CAMERA_BOUNDS_MAX_Y = 4.0f;
        private const float CAMERA_BOUNDS_MIN_Z = -FLOOR_HEIGHT / 2.0f;
        private const float CAMERA_BOUNDS_MAX_Z = FLOOR_HEIGHT / 2.0f;

        private CameraComponent camera;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Effect effect;
        private Texture2D nullTexture;
        private Texture2D floorColorMap;
        private Texture2D floorNormalMap;
        private Texture2D floorHeightMap;
        private VertexDeclaration floorVertexDeclaration;
        private VertexBuffer floorVertexBuffer;
        private Vector2 scaleBias;
        private Vector2 fontPos;
        private Model model;
        private Quaternion modelOrientation;
        private Vector3 modelPosition;
        private Matrix[] modelTransforms;
        private KeyboardState currentKeyboardState;
        private KeyboardState prevKeyboardState;
        private int windowWidth;
        private int windowHeight;
        private int frames;
        private int framesPerSecond;
        private double accumElapsedTimeSec;
        private bool displayHelp;
        private bool disableColorMap;
        private bool disableParallax;

        int xMatrix = 0;
        int yMatrix = 0;
        int zMatrix = 0;
        float rot = 0;

        public TestCamera()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            camera = new CameraComponent(this);
            Components.Add(camera);

            Window.Title = "After Dark Editor";
            IsFixedTimeStep = false;
            IsMouseVisible = false;
        }

        private void ChangeCameraBehavior(Camera.Behavior behavior)
        {
            if (camera.CurrentBehavior == behavior)
                return;

            if (behavior == Camera.Behavior.Orbit)
            {
                modelPosition = camera.Position;
                modelOrientation = Quaternion.Inverse(camera.Orientation);
            }

            camera.CurrentBehavior = behavior;

            // Position the camera behind and 30 degrees above the target.
            if (behavior == Camera.Behavior.Orbit)
                camera.Rotate(0.0f, -30.0f, 0.0f);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            // Setup the window to be a quarter the size of the desktop.
            windowWidth = GraphicsDevice.DisplayMode.Width / 2;
            windowHeight = GraphicsDevice.DisplayMode.Height / 2;

            // Setup frame buffer. DELETe
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            InitCamera();

            // Initial position for text rendering.
            fontPos = new Vector2(1.0f, 1.0f);

            // Parallax mapping height scale and bias values. DELETE
            scaleBias = new Vector2(0.04f, -0.03f);

            // Setup the initial input states.
            currentKeyboardState = Keyboard.GetState();
        }

        private void InitCamera()
        {
            GraphicsDevice device = graphics.GraphicsDevice;
            float aspectRatio = (float)windowWidth / (float)windowHeight;

            camera.Perspective(CAMERA_FOV, aspectRatio, CAMERA_ZNEAR, CAMERA_ZFAR);
            camera.Position = new Vector3(0.0f, CAMERA_OFFSET, 0.0f);
            camera.Acceleration = new Vector3(4.0f, 4.0f, 4.0f);
            camera.Velocity = new Vector3(1.0f, 1.0f, 1.0f);
            camera.OrbitMinZoom = 1.5f;
            camera.OrbitMaxZoom = float.MaxValue;
            camera.OrbitOffsetDistance = camera.OrbitMinZoom;

            ChangeCameraBehavior(Camera.Behavior.Orbit);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>(@"Fonts\DemoFont");

            // Create an empty white texture. This will be bound to the
            // colorMapTexture shader parameter when the user wants to
            // disable the color map texture. This trick will allow the
            // same shader to be used for when textures are enabled and
            // disabled. DELETE

            nullTexture = new Texture2D(GraphicsDevice, 1, 1, 0,
                            TextureUsage.None, SurfaceFormat.Color);

            Color[] pixels = { Color.White };

            nullTexture.SetData(pixels);

            model = Content.Load<Model>(@"Tank");

            // Load grid
            grid = new ReferenceGrid(GraphicsDevice, 5, 100, Color.Green);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //DELETE?
        private void PerformCameraCollisionDetection()
        {
            if (camera.CurrentBehavior != Camera.Behavior.Orbit)
            {
                Vector3 newPos = camera.Position;

                if (camera.Position.X > CAMERA_BOUNDS_MAX_X)
                    newPos.X = CAMERA_BOUNDS_MAX_X;

                if (camera.Position.X < CAMERA_BOUNDS_MIN_X)
                    newPos.X = CAMERA_BOUNDS_MIN_X;

                if (camera.Position.Y > CAMERA_BOUNDS_MAX_Y)
                    newPos.Y = CAMERA_BOUNDS_MAX_Y;

                if (camera.Position.Y < CAMERA_BOUNDS_MIN_Y)
                    newPos.Y = CAMERA_BOUNDS_MIN_Y;

                if (camera.Position.Z > CAMERA_BOUNDS_MAX_Z)
                    newPos.Z = CAMERA_BOUNDS_MAX_Z;

                if (camera.Position.Z < CAMERA_BOUNDS_MIN_Z)
                    newPos.Z = CAMERA_BOUNDS_MIN_Z;

                camera.Position = newPos;
            }
        }

        private void ToggleFullScreen()
        {
            int newWidth = 0;
            int newHeight = 0;

            graphics.IsFullScreen = !graphics.IsFullScreen;

            if (graphics.IsFullScreen)
            {
                newWidth = GraphicsDevice.DisplayMode.Width;
                newHeight = GraphicsDevice.DisplayMode.Height;
            }
            else
            {
                newWidth = windowWidth;
                newHeight = windowHeight;
            }

            graphics.PreferredBackBufferWidth = newWidth;
            graphics.PreferredBackBufferHeight = newHeight;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            float aspectRatio = (float)newWidth / (float)newHeight;

            camera.Perspective(CAMERA_FOV, aspectRatio, CAMERA_ZNEAR, CAMERA_ZFAR);
        }

        private bool KeyJustPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key);
        }

        private void ProcessInput()
        {
            prevKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (KeyJustPressed(Keys.Escape))
                this.Exit();

            if (KeyJustPressed(Keys.Back))
            {
                switch (camera.CurrentBehavior)
                {
                    case Camera.Behavior.Flight:
                        camera.UndoRoll();
                        break;

                    case Camera.Behavior.Orbit:
                        if (!camera.PreferTargetYAxisOrbiting)
                            camera.UndoRoll();
                        break;

                    default:
                        break;
                }
            }

            if (KeyJustPressed(Keys.Space))
            {
                if (camera.CurrentBehavior == Camera.Behavior.Orbit)
                    camera.PreferTargetYAxisOrbiting = !camera.PreferTargetYAxisOrbiting;
            }

            if (KeyJustPressed(Keys.D1))
                ChangeCameraBehavior(Camera.Behavior.FirstPerson);

            if (KeyJustPressed(Keys.D2))
                ChangeCameraBehavior(Camera.Behavior.Spectator);

            if (KeyJustPressed(Keys.D3))
                ChangeCameraBehavior(Camera.Behavior.Flight);

            if (KeyJustPressed(Keys.D4))
                ChangeCameraBehavior(Camera.Behavior.Orbit);

            if (KeyJustPressed(Keys.H))
                displayHelp = !displayHelp;

            if (KeyJustPressed(Keys.P))
                disableParallax = !disableParallax;

            if (KeyJustPressed(Keys.T))
                disableColorMap = !disableColorMap;

            if (KeyJustPressed(Keys.Add))
            {
                camera.RotationSpeed += 0.01f;

                if (camera.RotationSpeed > 1.0f)
                    camera.RotationSpeed = 1.0f;
            }

            if (KeyJustPressed(Keys.Subtract))
            {
                camera.RotationSpeed -= 0.01f;

                if (camera.RotationSpeed <= 0.0f)
                    camera.RotationSpeed = 0.01f;
            }

            if (currentKeyboardState.IsKeyDown(Keys.LeftAlt) ||
                currentKeyboardState.IsKeyDown(Keys.RightAlt))
            {
                if (KeyJustPressed(Keys.Enter))
                    ToggleFullScreen();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!this.IsActive)
                return;

            base.Update(gameTime);

            ProcessInput();
           // PerformCameraCollisionDetection();
        }

        void UpdateFrameRate(GameTime gameTime)
        {
            accumElapsedTimeSec += gameTime.ElapsedRealTime.TotalSeconds;

            if (accumElapsedTimeSec > 1.0)
            {
                framesPerSecond = frames;
                frames = 0;
                accumElapsedTimeSec = 0.0;
            }
            else
            {
                ++frames;
            }
        }


        private void DrawText()
        {
            string text = null;

            if (displayHelp)
            {
                System.Text.StringBuilder buffer = new System.Text.StringBuilder();

                buffer.AppendLine("Press 1 to switch to first person behavior");
                buffer.AppendLine("Press 2 to switch to spectator behavior");
                buffer.AppendLine("Press 3 to switch to flight behavior");
                buffer.AppendLine("Press 4 to switch to orbit behavior");
                buffer.AppendLine();
                buffer.AppendLine("First Person and Spectator behaviors:");
                buffer.AppendLine("  Press W and S to move forwards and backwards");
                buffer.AppendLine("  Press A and D to strafe left and right");
                buffer.AppendLine("  Press E and Q to move up and down");
                buffer.AppendLine("  Move mouse to free look");
                buffer.AppendLine();
                buffer.AppendLine("Flight behavior:");
                buffer.AppendLine("  Press W and S to move forwards and backwards");
                buffer.AppendLine("  Press A and D to yaw left and right");
                buffer.AppendLine("  Press E and Q to move up and down");
                buffer.AppendLine("  Move mouse up and down to change pitch");
                buffer.AppendLine("  Move mouse left and right to change roll");
                buffer.AppendLine();
                buffer.AppendLine("Orbit behavior:");
                buffer.AppendLine("  Press SPACE to enable/disable target Y axis orbiting");
                buffer.AppendLine("  Move mouse to orbit the model");
                buffer.AppendLine("  Mouse wheel to zoom in and out");
                buffer.AppendLine();
                buffer.AppendLine("Press T to enable/disable floor textures");
                buffer.AppendLine("Press P to toggle between parallax normal mapping and normal mapping");
                buffer.AppendLine("Press BACKSPACE to level camera");
                buffer.AppendLine("Press NUMPAD +/- to change camera rotation speed");
                buffer.AppendLine("Press ALT + ENTER to toggle full screen");
                buffer.AppendLine("Press H to hide help");

                text = buffer.ToString();
            }
            else
            {
                System.Text.StringBuilder buffer = new System.Text.StringBuilder();

                buffer.AppendFormat("FPS: {0}\n", framesPerSecond);
                buffer.AppendFormat("Floor Technique: {0}\n\n",
                    (disableParallax ? "Normal mapping" : "Parallax normal mapping"));
                buffer.Append("Camera:\n");
                buffer.AppendFormat("  Behavior: {0}\n", camera.CurrentBehavior);
                buffer.AppendFormat("  Position: x:{0} y:{1} z:{2}\n",
                    camera.Position.X.ToString("g2"),
                    camera.Position.Y.ToString("g2"),
                    camera.Position.Z.ToString("g2"));
                buffer.AppendFormat("  Velocity: x:{0} y:{1} z:{2}\n",
                    camera.CurrentVelocity.X.ToString("g2"),
                    camera.CurrentVelocity.Y.ToString("g2"),
                    camera.CurrentVelocity.Z.ToString("g2"));
                buffer.AppendFormat("  Rotation speed: {0}\n",
                    camera.RotationSpeed.ToString("g2"));

                if (camera.PreferTargetYAxisOrbiting)
                    buffer.Append("  Target Y axis orbiting\n\n");
                else
                    buffer.Append("  Free orbiting\n\n");

                buffer.Append("Press H to display help");

                text = buffer.ToString();
            }

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
            spriteBatch.DrawString(spriteFont, text, fontPos, Color.Yellow);
            spriteBatch.End();
        }

        private void DrawModel()
        {
            if (modelTransforms == null)
            {
                modelTransforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            }

            Matrix world = Matrix.CreateFromQuaternion(modelOrientation);

            world.M41 = modelPosition.X;
            world.M42 = modelPosition.Y;
            world.M43 = modelPosition.Z;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.PreferPerPixelLighting = true;
                    effect.EnableDefaultLighting();
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                    effect.World = modelTransforms[mesh.ParentBone.Index] * world;
                }

                mesh.Draw();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (!this.IsActive)
                return;

            if (camera.CurrentBehavior == Camera.Behavior.Orbit)
                DrawModel();

            DrawText();

            // Draw grid
            grid.Draw(cameraViewMatrix, cameraProjectionMatrix);

            base.Draw(gameTime);
            UpdateFrameRate(gameTime);
        }

        void CreateViewMatrix()
        {
            cameraViewMatrix = Matrix.CreateLookAt(new Vector3(4000, 4000, 4000), Vector3.Zero, Vector3.Up);
        }

        void CreateProjectionMatrix()
        {
            // Projection matrix
            cameraProjectionMatrix =
                Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(45f),
                    graphics.GraphicsDevice.Viewport.AspectRatio,
                    1f, 12000f);
        }
    }
}*/