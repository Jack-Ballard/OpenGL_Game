using OpenGL_Game.Scenes;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL_Game.Scenes.Scene;

namespace OpenGL_Game.Managers
{
    class GameInputManager : InputManager
    {
        GameScene scene;
        public GameInputManager(GameScene pScene)
        {
            scene = pScene;
        }

        public override void Update()
        {
            bool keyPressed = false;
            if (keysPressed[(char)Keys.Up])
            {
                //cameraPosition.Position += camera.cameraDirection * 0.1f;
                scene.playerEntityVelocity.Velocity = scene.camera.cameraDirection * 5f;
                keyPressed = true;
            }
            if (keysPressed[(char)Keys.Down])
            {
                //cameraPosition.Position += camera.cameraDirection * -0.1f;
                scene.playerEntityVelocity.Velocity = scene.camera.cameraDirection * -5f;
                keyPressed = true;
            }
            if (keysPressed[(char)Keys.Left])
            {
                scene.camera.RotateY(-0.01f);
            }
            if (keysPressed[(char)Keys.Right])
            {
                scene.camera.RotateY(0.01f);
            }
            if (keysPressed[(char)Keys.M])
            {
                scene.GameOver();
            }
            if (!keyPressed)
            {
                //playerEntityVelocity.Velocity = new Vector3(0, 0, 0);
                scene.playerEntityVelocity.Velocity = scene.playerEntityVelocity.Velocity * new Vector3(0.2f, 0.2f, 0.2f);
            }
        }
    }
}
