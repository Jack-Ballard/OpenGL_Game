using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Objects;
using OpenGL_Game.Game.Scenes;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL_Game.Game.Scenes.Scene;

namespace OpenGL_Game.Game.GameManagers
{
    class GameInputManager : InputManager
    {
        GameScene gameScene = null;
        MainMenuScene mainMenuScene = null;
        HighScoreDisplayScene highScoreDisplayScene = null;
        public GameInputManager(GameScene pScene)
        {
            gameScene = pScene;
        }
        public GameInputManager(MainMenuScene pMainMenuScene)
        {
            mainMenuScene = pMainMenuScene;
        }
        public GameInputManager(HighScoreDisplayScene pHighScoreDisplayScene)
        {
            highScoreDisplayScene= pHighScoreDisplayScene;
        }

        public override void Update()
        {
            if (gameScene != null)
                GameSceneAction(gameScene);
            if(mainMenuScene != null)
                MainMenuSceneAction(mainMenuScene);
            if(highScoreDisplayScene != null)
                HighScoreDisplaySceneAction(highScoreDisplayScene);

        }

        public void GameSceneAction(GameScene scene)
        {
            bool keyPressed = false;
            Vector3 CameraVector = (0, 0, 0);
            if (keysPressed[(char)Keys.Up])
            {
                //cameraPosition.Position += camera.cameraDirection * 0.1f;
                CameraVector = scene.camera.cameraDirection * scene.camera.cameraSpeed;
                //scene.playerEntityVelocity.Velocity = scene.camera.cameraDirection * 5f;
                keyPressed = true;
            }
            if (keysPressed[(char)Keys.Down])
            {
                //cameraPosition.Position += camera.cameraDirection * -0.1f;
                CameraVector = scene.camera.cameraDirection * -scene.camera.cameraSpeed;
                //scene.playerEntityVelocity.Velocity = scene.camera.cameraDirection * -5f;
                keyPressed = true;
            }
            scene.playerEntityVelocity.Velocity = CameraVector;
            if (keysPressed[(char)Keys.Left])
            {
                scene.camera.RotateY(-0.01f);
            }
            if (keysPressed[(char)Keys.Right])
            {
                scene.camera.RotateY(0.01f);
            }
            if (keysPressed[(char)Keys.Space])
            {
                gameScene.shootManager.Shoot();
            }
            if (keysPressed[(char)Keys.M])
            {
                scene.ToGameOverScene();
            }
            if (keysPressed[(char)Keys.C])
            {
                (scene.collisionManager as GameCollisionManager).mazeCollisionEnabled = false;
            }
            else if((scene.collisionManager as GameCollisionManager).mazeCollisionEnabled == false)
            {
                (scene.collisionManager as GameCollisionManager).mazeCollisionEnabled = true;
            }
            if (!keyPressed)
            {
                //playerEntityVelocity.Velocity = new Vector3(0, 0, 0);
                scene.playerEntityVelocity.Velocity = scene.playerEntityVelocity.Velocity * new Vector3(0.2f, 0.2f, 0.2f);
            }
        }

        public void MainMenuSceneAction(MainMenuScene scene)
        {
            if (mouseButtonsPressed[(int)MouseButton.Left])
            {
                // Handle left mouse button click
                scene.ToGameScene();
            }
            if(keysPressed[(char)Keys.H])
            {
                scene.ToHighScoreScene();
            }
        }

        public void HighScoreDisplaySceneAction(HighScoreDisplayScene scene)
        {
            if(keysPressed[(char)Keys.M])
            {
                scene.ToMainMenuScene();
            }
        }

    }
}
