using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using OpenGL_Game.Engine.Managers;

namespace OpenGL_Game.Game.Scenes
{
    class GameOverScene : Scene
    {
        private string playerName = "";
        private bool nameSubmitted = false;
        private int finalScore = 0; // Set this from your game logic

        public GameOverScene(SceneManager sceneManager) : base(sceneManager)
        {
            sceneManager.Title = "Game Over";
            sceneManager.renderer = Render;
            sceneManager.updater = Update;

            sceneManager.mouseDelegate += Mouse_BottonPressed;
            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate += Keyboard_KeyUp;

            GL.ClearColor(0.2f, 0.75f, 1.0f, 1.0f);
        }

        public override void Update(FrameEventArgs e)
        {
            // No update logic needed for name entry
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Size.X, sceneManager.Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Size.X, 0, sceneManager.Size.Y, -1, 1);

            SKPaint paint = new SKPaint();
            paint.TextSize = 100;
            paint.StrokeWidth = 2;
            paint.TextAlign = SKTextAlign.Center;
            paint.IsAntialias = true;
            paint.Color = SKColors.Yellow;
            paint.Style = SKPaintStyle.Fill;
            GUI.DrawText("Game Over", sceneManager.Size.X * 0.5f, 150, paint);
            paint.Color = SKColors.DarkBlue;
            paint.Style = SKPaintStyle.Stroke;
            GUI.DrawText("Game Over", sceneManager.Size.X * 0.5f, 150, paint);

            paint.TextSize = 50;
            paint.Color = SKColors.White;
            paint.Style = SKPaintStyle.Fill;
            GUI.DrawText($"Score: {HighscoreManager.HighScore.Item2}", sceneManager.Size.X * 0.5f, 300, paint);

            if (!nameSubmitted)
            {
                GUI.DrawText("Enter your name:", sceneManager.Size.X * 0.5f, 400, paint);
                GUI.DrawText(playerName + "_", sceneManager.Size.X * 0.5f, 470, paint);
                GUI.DrawText("Press Enter to submit", sceneManager.Size.X * 0.5f, 540, paint);
            }
            else
            {
                GUI.DrawText("Name submitted!", sceneManager.Size.X * 0.5f, 470, paint);
            }

            GUI.Render();
        }

        public void Mouse_BottonPressed(MouseButtonEventArgs e)
        {
            if (nameSubmitted)
            {
                switch (e.Button)
                {
                    case MouseButton.Left:
                        sceneManager.ChangeScene(SceneTypes.SCENE_MAIN_MENU);
                        break;
                }
            }
        }

        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            if (nameSubmitted)
                return;

            if (e.Key == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(playerName))
                {
                    HighscoreManager.AddNewName(playerName);
                    HighscoreManager.AddHighscore(HighscoreManager.HighScore);
                    nameSubmitted = true;
                }
            }
            else if (e.Key == Keys.Backspace)
            {
                if (playerName.Length > 0)
                    playerName = playerName.Substring(0, playerName.Length - 1);
            }
            else
            {
                // Accept only letters, numbers, and a few symbols
                char c = GetCharFromKey(e.Key, e.Shift);
                if (c != '\0' && playerName.Length < 16)
                    playerName += c;
            }
        }

        public void Keyboard_KeyUp(KeyboardKeyEventArgs e)
        {
            // No action needed
        }

        // Helper to convert Keys to char
        private char GetCharFromKey(Keys key, bool shift)
        {
            // Only handle A-Z, 0-9, and space for simplicity
            if (key >= Keys.A && key <= Keys.Z)
            {
                char c = (char)('A' + (key - Keys.A));
                return shift ? c : char.ToLower(c);
            }
            if (key == Keys.Space)
                return ' ';
            return '\0';
        }

        public override void Close()
        {
            sceneManager.mouseDelegate -= Mouse_BottonPressed;
            sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate -= Keyboard_KeyUp;
        }
    }
}