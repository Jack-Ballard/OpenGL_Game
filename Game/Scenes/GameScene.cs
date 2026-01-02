using OpenTK.Graphics.OpenGL;
using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using SkiaSharp;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Objects;
using OpenGL_Game.Engine.Systems;
using OpenGL_Game.Game.GameManagers;
using System.Collections.Generic;

namespace OpenGL_Game.Game.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class GameScene : Scene
    {
        public static float dt = 0;
        EntityManager entityManager;
        SystemManager systemManager;
        CollisionManager collisionManager;
        InputManager inputManager;
        public GameShootManager shootManager;
        //public Camera camera;
        public ComponentPosition playerEntityPosition;
        public ComponentVelocity playerEntityVelocity;
        public static GameScene gameInstance;
        //bool[] keysPressed = new bool[512];
        private string _playerName = "Player";

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            collisionManager = new GameCollisionManager();
            inputManager = new GameInputManager(this);
            shootManager = new GameShootManager(entityManager);

            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.keyboardDownDelegate += inputManager.Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate += inputManager.Keyboard_KeyUp;

            // Enable Depth Testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Set Camera
            camera = new Camera(new Vector3(0, 0.5f, 7), new Vector3(0, 0.5f, 0), sceneManager.Size.X / (float)sceneManager.Size.Y, 0.1f, 100f);

            CreateEntities();
            CreateSystems();
            AssignCameraToEntity(_playerName);

            // TODO: Add your initialization logic here
        }

        private void CreateEntities()
        {
            Entity newEntity;

            newEntity = new Entity(_playerName);
            newEntity.AddComponent(new ComponentPosition(-8.0f, 0.5f, 6f));
            newEntity.AddComponent(new ComponentGeometry("Game/Geometry/Skybox/Skybox.obj"));
            newEntity.AddComponent(new ComponentVelocity(0.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentShaderDefault());
            newEntity.AddComponent(new ComponentCollisionAABB(0.5f, -0.5f, 0.5f, -0.5f, 1.0f, -1.0f));
            newEntity.AddComponent(new ComponentCollisionSphere(0.7f));
            newEntity.AddComponent(new ComponentHealth(3));
            newEntity.AddComponent(new ComponentAudio("Game/Audio/hurt.wav", false));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Item1");
            newEntity.AddComponent(new ComponentPosition(20.0f, 0.0f, 30.0f));
            newEntity.AddComponent(new ComponentGeometry("Game/Geometry/Moon/moon.obj"));
            newEntity.AddComponent(new ComponentShaderDefault());
            //newEntity.AddComponent(new ComponentCollisionAABB(1, -1, 1, -1, 1, -1));
            newEntity.AddComponent(new ComponentCollisionSphere(1f));
            newEntity.AddComponent(new ComponentAudio("Game/Audio/itemHover.wav"));


            entityManager.AddEntity(newEntity);

            List<Vector3> patrolPoints = new List<Vector3>{
                new Vector3(19.0f, 0.0f, 26.0f),
                new Vector3(-34.0f, 0.0f, 25.0f),
                new Vector3(-33.0f, 0.0f, -13.0f),
                new Vector3(19.0f, 0.0f, -12.0f),

                new Vector3(1.0f, 0.0f, 26.0f),
                new Vector3(-17.0f, 0.0f, 25.0f),
                new Vector3(1.0f, 0.0f, -12.0f),
                new Vector3(-17.0f, 0.0f, -13.0f),

                new Vector3(19.0f, 0.0f, 7.0f),
                new Vector3(1.0f, 0.0f, 7.0f),
                new Vector3(-17.0f, 0.0f, 6.0f),
                new Vector3(-33.0f, 0.0f, 6.0f),

            };

            newEntity = new Entity("Enemy_1");
            newEntity.AddComponent(new ComponentPosition(15.0f, 0.0f, -15.0f));
            newEntity.AddComponent(new ComponentGeometry("Game/Geometry/Wraith_Raider_Starship/Wraith_Raider_Starship.obj"));
            newEntity.AddComponent(new ComponentVelocity(-0.5f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentShaderDefault());
            newEntity.AddComponent(new ComponentAITarget(_playerName, patrolPoints));
            //newEntity.AddComponent(new ComponentAITarget(new Vector3(-60f, 0.0f, -15f)));
            newEntity.AddComponent(new ComponentCollisionSphere(1f)); 
            newEntity.AddComponent(new ComponentAudio("Game/Audio/buzz.wav"));
            newEntity.AddComponent(new ComponentCollisionLine(new Vector3(-1.0f, 0.0f, 0.0f)));
            newEntity.AddComponent(new ComponentHealth(3));
            //newEntity.AddComponent(new ComponentCollisionAABB(1.0f, -1.0f, 1.0f, -1.0f, 1.0f, -1.0f));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Enemy_2");
            newEntity.AddComponent(new ComponentPosition(-34.0f, 0.0f, 25.0f));
            newEntity.AddComponent(new ComponentGeometry("Game/Geometry/Wraith_Raider_Starship/Wraith_Raider_Starship.obj"));
            newEntity.AddComponent(new ComponentVelocity(-0.5f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentShaderDefault());
            newEntity.AddComponent(new ComponentAITarget(_playerName, patrolPoints));
            //newEntity.AddComponent(new ComponentAITarget(new Vector3(-60f, 0.0f, -15f)));
            newEntity.AddComponent(new ComponentCollisionSphere(1f));
            newEntity.AddComponent(new ComponentAudio("Game/Audio/buzz.wav"));
            newEntity.AddComponent(new ComponentHealth(3));
            newEntity.AddComponent(new ComponentCollisionLine(new Vector3(-1.0f, 0.0f, 0.0f)));
            //newEntity.AddComponent(new ComponentCollisionAABB(1.0f, -1.0f, 1.0f, -1.0f, 1.0f, -1.0f));
            entityManager.AddEntity(newEntity);

            //newEntity = new Entity("Intergalactic_Spaceship");
            //newEntity.AddComponent(new ComponentPosition(0.0f, -0.1f, 0.0f));
            //newEntity.AddComponent(new ComponentGeometry(
            //"Game/Geometry/Intergalactic_Spaceship/Intergalactic_Spaceship.obj"));
            //newEntity.AddComponent(new ComponentShaderDefault());
            //newEntity.AddComponent(new ComponentCollisionLine(new Vector3(-1.0f, 0.2f, 0.0f)));
            //newEntity.AddComponent(new ComponentCollisionAABB(1.0f, -1.0f, 1.0f, -1.0f, 1.0f, -1.0f));
            //entityManager.AddEntity(newEntity);

            newEntity = new Entity("Maze");
            newEntity.AddComponent(new ComponentPosition(29.0f, -1.0f, -22.0f));
            newEntity.AddComponent(new ComponentGeometry("Game/Geometry/Maze2/Maze2.obj"));
            newEntity.AddComponent(new ComponentGeometry("Game/Geometry/Maze2/Maze2Base.obj"));
            //newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/moon.obj"));
            newEntity.AddComponent(new ComponentShaderDefault());

            newEntity.AddComponent(new ComponentCollisionAABB(1f, 0f, 0f, -72f, 10, -10));
            newEntity.AddComponent(new ComponentCollisionAABB(57f, 0f, -71f, -72f, 10, -10));
            newEntity.AddComponent(new ComponentCollisionAABB(57f, 56f, 0f, -72f, 10, -10));
            newEntity.AddComponent(new ComponentCollisionAABB(57f, 0f, 0f, -1f, 10, -10));

            newEntity.AddComponent(new ComponentCollisionAABB(6f, 0f, -16f, -56f, 10, -10));
            newEntity.AddComponent(new ComponentCollisionAABB(41f, 16f, -66f, -72f, 10, -10));
            newEntity.AddComponent(new ComponentCollisionAABB(57f, 51f, -16f, -56f, 10, -10));
            newEntity.AddComponent(new ComponentCollisionAABB(41f, 16f, 0f, -6f, 10, -10));

            newEntity.AddComponent(new ComponentCollisionAABB(21f, 11f, -16f, -26f, 10, -10));
            newEntity.AddComponent(new ComponentCollisionAABB(26f, 16f, -11f, -21f, 10, -10));

            newEntity.AddComponent(new ComponentCollisionAABB(21f, 11f, -46f, -56f, 10, -10));
            newEntity.AddComponent(new ComponentCollisionAABB(26f, 16f, -51f, -61f, 10, -10));

            newEntity.AddComponent(new ComponentCollisionAABB(41f, 31f, -11f, -21f, 10, -10));
            newEntity.AddComponent(new ComponentCollisionAABB(46f, 36f, -16f, -26f, 10, -10));

            newEntity.AddComponent(new ComponentCollisionAABB(41f, 31f, -51f, -61f, 10, -10));
            newEntity.AddComponent(new ComponentCollisionAABB(46f, 36f, -46f, -56f, 10, -10));

            newEntity.AddComponent(new ComponentCollisionAABB(21f, 11f, -31f, -41f, 10, -10));
            newEntity.AddComponent(new ComponentCollisionAABB(46f, 36f, -31f, -41f, 10, -10));

            entityManager.AddEntity(newEntity);
        }

        private void CreateSystems()
        {
            Engine.Systems.System newSystem;

            newSystem = new SystemRender(this);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemAudio(this);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemCollisionSphereSphere(collisionManager);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemCollisionInAABB(collisionManager);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemCollisionLineLine(collisionManager);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemAISteering();
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

            //Console.WriteLine(cameraPosition.Position);
            camera.cameraPosition = playerEntityPosition.Position;
            camera.UpdateView();
            inputManager.Update();
            shootManager.Update(camera, _playerName, playerEntityPosition);
            collisionManager.ProcessCollisions();

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
            //GUI.DrawText("Score: 000", 30, 70, 30, 255, 255, 255);
            foreach (IComponent component in entityManager.FindEntity(_playerName).Components)
            {
                if (component.ComponentType == ComponentTypes.COMPONENT_HEALTH)
                {
                    ComponentHealth healthComponent = (ComponentHealth)component;
                    GUI.DrawText("Lives: " + healthComponent.Health, 30, 70, 30, 255, 255, 255);
                }
            }
            foreach (IComponent component in entityManager.FindEntity("Enemy_1").Components)
            {
                if (component.ComponentType == ComponentTypes.COMPONENT_HEALTH)
                {
                    ComponentHealth healthComponent = (ComponentHealth)component;
                    GUI.DrawText("Enemy 1 Health: " + healthComponent.Health, 30, 100, 30, 255, 255, 255);
                }
            }
            foreach (IComponent component in entityManager.FindEntity("Enemy_2").Components)
            {
                if (component.ComponentType == ComponentTypes.COMPONENT_HEALTH)
                {
                    ComponentHealth healthComponent = (ComponentHealth)component;
                    GUI.DrawText("Enemy 2 Health: " + healthComponent.Health, 30, 130, 30, 255, 255, 255);
                }
            }
            GUI.Render();
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            sceneManager.keyboardDownDelegate -= inputManager.Keyboard_KeyDown;
            //ResourceManager.RemoveAllAssets();
            systemManager.CloseSystems(entityManager);
            // Need to remove assets (except Text) from Resource Manager
        }

        public void ToMainMenuScene()
        {
            sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
        }

        public void AssignCameraToEntity(string entityName)
        {
            foreach (IComponent component in entityManager.FindEntity(entityName).Components)
            {
                if (component.ComponentType == ComponentTypes.COMPONENT_POSITION)
                {
                    ComponentPosition position = (ComponentPosition)component;
                    playerEntityPosition = position;
                }
                if (component.ComponentType == ComponentTypes.COMPONENT_VELOCITY)
                {
                    ComponentVelocity velocity = (ComponentVelocity)component;
                    playerEntityVelocity = velocity;
                }
            }
        }

        
    }
}