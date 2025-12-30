using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Systems;
using OpenTK.Windowing.Common;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenGL_Game.Game.GameManagers;

namespace OpenGL_Game.Game.Scenes
{
    class HighScoreDisplayScene : Scene
    {
        SystemManager systemManager;
        HighscoreManager highscoreManager;
        GameInputManager inputManager;

        bool singleFireTest = true;
        public HighScoreDisplayScene(SceneManager sceneManager) : base(sceneManager)
        {
            systemManager = new SystemManager();
            highscoreManager = new HighscoreManager();
            inputManager = new GameInputManager(this);

            // Set the title of the window
            sceneManager.Title = "Highscore Display";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;

            sceneManager.keyboardDownDelegate += inputManager.Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate += inputManager.Keyboard_KeyUp;

            CreateSystems();
        }

        private void CreateSystems()
        {
            Engine.Systems.System newSystem;

            newSystem = new SystemRender(this);
            systemManager.AddSystem(newSystem);
        }
        public override void Update(FrameEventArgs e)
        {
            if(singleFireTest)
            {
                //highscoreManager.AddHighscore("PlayerOne", 1500);
                //highscoreManager.AddHighscore("PlayerTwo", 2500);
                Console.WriteLine("High Scores from Server:");
                //Console.WriteLine(response);
                foreach (var score in HighscoreManager.HighScores)
                {
                    Console.WriteLine($"{score.Item1}: {score.Item2}");
                }
                singleFireTest = false;
            }
            inputManager.Update();
        }

        public void ToMainMenuScene()
        {
            HighscoreManager.Disconect();
            sceneManager.ChangeScene(SceneTypes.SCENE_MAIN_MENU);
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Size.X, sceneManager.Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Size.X, 0, sceneManager.Size.Y, -1, 1);

            //Display the Title using an outlined text
            SKPaint paint = new SKPaint();
            paint.TextSize = 100;
            paint.StrokeWidth = 2;
            paint.TextAlign = SKTextAlign.Center;
            paint.IsAntialias = true;
            paint.Color = SKColors.Yellow;
            paint.Style = SKPaintStyle.Fill;
            GUI.DrawText("High Scores:", sceneManager.Size.X * 0.5f, 150, paint);
            paint.Color = SKColors.DarkBlue;
            paint.Style = SKPaintStyle.Stroke;
            GUI.DrawText("High Scores:", sceneManager.Size.X * 0.5f, 150, paint);
            GUI.Render();
        }

        public override void Close()
        {
            sceneManager.keyboardDownDelegate -= inputManager.Keyboard_KeyDown;
            ResourceManager.RemoveAllAssets();
        }
    }
}
