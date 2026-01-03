using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Objects;
using OpenGL_Game.Game.Scenes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Game.GameManagers
{
    class GameShootManager
    {
        private EntityManager entityManager;
        private string _playerName;
        float shootDelay = 0.2f;
        public int shootCooldown = 100;
        private GameScene gameScene;
        public GameShootManager(EntityManager EntityManager, GameScene gameScene, string playerName) 
        {
            entityManager = EntityManager;
            this.gameScene = gameScene;
            _playerName = playerName;
        }
        public void Update()
        {
            shootDelay--;
        }
        public void Shoot()
        {
            if (shootDelay > 0)
            {
                return;
            }
            foreach (Entity entity in entityManager.Entities())
            {
                if (entity.Name == _playerName)
                {
                    ComponentAudio playerShootAudio = Engine.Systems.System.GetComponent(entity, ComponentTypes.COMPONENT_AUDIO) as ComponentAudio;
                    playerShootAudio.PlaySound("Game/Audio/gunshot.wav");
                    continue;
                }
                if ((entity.Mask & ComponentTypes.COMPONENT_AI_TARGET) == 0) continue;

                IComponent positionComponent = Engine.Systems.System.GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition position = (ComponentPosition)positionComponent;

                IComponent aiComponent = Engine.Systems.System.GetComponent(entity, ComponentTypes.COMPONENT_AI_TARGET);
                ComponentAITarget aiTarget = (ComponentAITarget)aiComponent;
                float distance = Vector3.Distance(position.Position, gameScene.playerEntityPosition.Position);
                Vector3 targetPoint = gameScene.playerEntityPosition.Position + distance * gameScene.camera.cameraDirection.Normalized();
                ComponentAudio audio = Engine.Systems.System.GetComponent(entity, ComponentTypes.COMPONENT_AUDIO) as ComponentAudio;
                
                if (Vector3.Distance(targetPoint, position.Position) < 2 && (entity.Mask & ComponentTypes.COMPONENT_HEALTH) != 0)
                {
                    IComponent healthComponent = Engine.Systems.System.GetComponent(entity, ComponentTypes.COMPONENT_HEALTH);
                    ComponentHealth health = (ComponentHealth)healthComponent;
                    if(health.Health > 0)
                    {
                        health.Health--;
                        HighscoreManager.AddToScore((int)gameScene.CountdownTimer * 5);
                    }
                    if (health.Health <= 0)
                    {
                        aiTarget.Behaviour = AIbehaviour.DEAD;
                        audio.StopSound();
                        audio.PlaySound("Game/Audio/shutdown.wav");
                    }
                }
                shootDelay = shootCooldown;
            }
        }
    }
}
