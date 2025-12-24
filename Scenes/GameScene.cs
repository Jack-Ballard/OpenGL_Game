using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using SkiaSharp;

namespace OpenGL_Game.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class GameScene : Scene
    {
        public static float dt = 0;
        EntityManager entityManager;
        SystemManager systemManager;
        //public Camera camera;
        public ComponentPosition cameraPosition;
        public static GameScene gameInstance;
        bool[] keysPressed = new bool[512];



        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();

            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate += Keyboard_KeyUp;

            // Enable Depth Testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Set Camera
            camera = new Camera(new Vector3(0, 0.5f, 7), new Vector3(0, 0.5f, 0), (float)(sceneManager.Size.X) / (float)(sceneManager.Size.Y), 0.1f, 100f);

            CreateEntities();
            CreateSystems();
            AssignCameraToEntity("Wraith_Raider_Starship");

            // TODO: Add your initialization logic here
        }

        private void CreateEntities()
        {
            Entity newEntity;

            newEntity = new Entity("Moon");
            newEntity.AddComponent(new ComponentPosition(-2.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/moon.obj"));
            newEntity.AddComponent(new ComponentShaderDefault());
            //newEntity.AddComponent(new ComponentCollisionSphere(1.0f));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Wraith_Raider_Starship");
            newEntity.AddComponent(new ComponentPosition(2.0f, -0.0f, 0.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Wraith_Raider_Starship/wraith_raider_starship.obj"));
            newEntity.AddComponent(new ComponentVelocity(-0.5f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentShaderDefault());
            //newEntity.AddComponent(new ComponentAudio());
            //newEntity.AddComponent(new ComponentCollisionLine(new Vector3(-1.0f, 0.0f, 0.0f)));
            //newEntity.AddComponent(new ComponentCollisionAABB(1.0f, -1.0f, 1.0f, -1.0f, 1.0f, -1.0f));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Intergalactic_Spaceship");
            newEntity.AddComponent(new ComponentPosition(0.0f, -0.1f, 0.0f));
            newEntity.AddComponent(new ComponentGeometry(
            "Geometry/Intergalactic_Spaceship/Intergalactic_Spaceship.obj"));
            newEntity.AddComponent(new ComponentShaderDefault());
            //newEntity.AddComponent(new ComponentCollisionLine(new Vector3(-1.0f, 0.2f, 0.0f)));
            //newEntity.AddComponent(new ComponentCollisionAABB(1.0f, -1.0f, 1.0f, -1.0f, 1.0f, -1.0f));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Maze");
            newEntity.AddComponent(new ComponentPosition(29.0f, -1.0f, -22.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Maze/maze.obj"));
            newEntity.AddComponent(new ComponentShaderDefault());
            entityManager.AddEntity(newEntity);
        }

        private void CreateSystems()
        {
            Systems.System newSystem;

            newSystem = new SystemRender(this);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemAudio(this);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemCollisionSphereSphere(entityManager.Entities());
            systemManager.AddSystem(newSystem);

            newSystem = new SystemCollisionInAABB(entityManager.Entities());
            systemManager.AddSystem(newSystem);

            newSystem = new SystemCollisionLineLine(entityManager.Entities());
            systemManager.AddSystem(newSystem);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Update(FrameEventArgs e)
        {
            dt = (float)e.Time;
            //System.Console.WriteLine("fps=" + (int)(1.0/dt));

            if (keysPressed[(char)Keys.Up])
            {
                cameraPosition.Position += camera.cameraDirection * 0.1f;
            }
            if (keysPressed[(char)Keys.Down])
            {
                cameraPosition.Position += camera.cameraDirection * -0.1f;
            }
            if (keysPressed[(char)Keys.Left])
            {
                camera.RotateY(-0.01f);
            }
            if (keysPressed[(char)Keys.Right])
            {
                camera.RotateY(0.01f);
            }
            if (keysPressed[(char)Keys.M])
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            }

            //Console.WriteLine(cameraPosition.Position);
            camera.cameraPosition = cameraPosition.Position;
            camera.UpdateView();

            //foreach (IComponent component in entityManager.FindEntity("Wraith_Raider_Starship").Components)
            //{
            //    if (component.ComponentType == ComponentTypes.COMPONENT_POSITION)
            //    {
            //        ComponentPosition position = (ComponentPosition)component;
            //        camera.cameraPosition = position.Position;
            //        camera.UpdateView();
            //        cameraPosition = position;
            //    }
            //}

            // TODO: Add your update logic here
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Size.X, sceneManager.Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Action ALL systems
            systemManager.ActionSystems(entityManager);

            // Render score
            GUI.DrawText("Score: 000", 30, 70, 30, 255, 255, 255);
            GUI.Render();
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
            ResourceManager.RemoveAllAssets();
            systemManager.CloseSystems(entityManager);
            // Need to remove assets (except Text) from Resource Manager
        }

        //public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        //{
        //    switch (e.Key)
        //    {
        //        case Keys.Up:
        //            camera.MoveForward(0.1f);
        //            break;
        //        case Keys.Down:
        //            camera.MoveForward(-0.1f);
        //            keysPressed[(char)e.Key] = true;
        //            break;
        //        case Keys.Left:
        //            camera.RotateY(-0.01f);
        //            break;
        //        case Keys.Right:
        //            camera.RotateY(0.01f);
        //            break;
        //        case Keys.M:
        //            sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
        //            break;
        //    }
        //}
        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = true;
        }
        public void Keyboard_KeyUp(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = false;
        }

        public void AssignCameraToEntity(string entityName)
        {
            foreach (IComponent component in entityManager.FindEntity(entityName).Components)
            {
                if (component.ComponentType == ComponentTypes.COMPONENT_POSITION)
                {
                    ComponentPosition position = (ComponentPosition)component;
                    cameraPosition = position;
                }
            }
        }
    }
}