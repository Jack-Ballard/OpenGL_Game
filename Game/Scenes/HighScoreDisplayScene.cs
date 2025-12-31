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
        GameInputManager inputManager;

        bool singleFireTest = true;
        List<(string, int)> highScores = new List<(string, int)>();
        public HighScoreDisplayScene(SceneManager sceneManager) : base(sceneManager)
        {
            systemManager = new SystemManager();
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
            if(highScores.Count == 0)
            {
                highScores = HighscoreManager.GetHighScores();
            }
            inputManager.Update();
        }

        public void ToMainMenuScene()
        {
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
            paint.TextSize = 80;
            paint.Color = SKColors.Yellow;
            paint.Style = SKPaintStyle.Fill;
            for(int i = 0; i < 2; i++)
            {
                foreach (var item in highScores)
                {
                    GUI.DrawText(item.Item1 + " - " + item.Item2.ToString(), sceneManager.Size.X * 0.5f, 200 + (1+highScores.IndexOf(item)) * 75, paint);
                }
                paint.Color = SKColors.DarkBlue;
                paint.Style = SKPaintStyle.Stroke;
            }
            
            
            GUI.Render();
        }

        public override void Close()
        {
            sceneManager.keyboardDownDelegate -= inputManager.Keyboard_KeyDown;
            //ResourceManager.RemoveAllAssets();
        }
    }
}
