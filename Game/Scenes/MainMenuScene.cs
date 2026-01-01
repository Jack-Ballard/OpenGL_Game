using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Game.GameManagers;

namespace OpenGL_Game.Game.Scenes
{
    class MainMenuScene : Scene
    {
        GameInputManager inputManager;
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
            inputManager = new GameInputManager(this);
            // Set the title of the window
            sceneManager.Title = "Main Menu";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;

            sceneManager.mouseDelegate += inputManager.Mouse_BottonPressed;
            sceneManager.keyboardDownDelegate += inputManager.Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate += inputManager.Keyboard_KeyUp;

            GL.ClearColor(0.2f, 0.75f, 1.0f, 1.0f);
            HighscoreManager.LoadHighScores();
        }

        public override void Update(FrameEventArgs e)
        {
            inputManager.Update();
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
            GUI.DrawText("Main Menu", sceneManager.Size.X * 0.5f, 150, paint);
            paint.Color = SKColors.DarkBlue;
            paint.Style = SKPaintStyle.Stroke;
            GUI.DrawText("Main Menu", sceneManager.Size.X * 0.5f, 150, paint);
            GUI.Render();
        }

        public void ToGameScene()
        {
            sceneManager.ChangeScene(SceneTypes.SCENE_GAME);
        }
        public void ToHighScoreScene()
        {
            sceneManager.ChangeScene(SceneTypes.SCENE_HIGH_SCORE_DISPLAY);
        }

        public override void Close()
        {
            sceneManager.mouseDelegate -= inputManager.Mouse_BottonPressed;
            sceneManager.keyboardDownDelegate -= inputManager.Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate -= inputManager.Keyboard_KeyUp;
        }
    }
}