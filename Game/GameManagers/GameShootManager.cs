using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Objects;
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
        private Camera camera;
        private string _playerName;
        private ComponentPosition playerEntityPosition;
        float shootDelay = 0.2f;
        public GameShootManager(EntityManager EntityManager) 
        {
            entityManager = EntityManager;
        }
        public void Update(Camera pCamera, string playerName, ComponentPosition playerEntityPosition)
        {
            camera = pCamera;
            _playerName = playerName;
            this.playerEntityPosition = playerEntityPosition;
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
                float distance = Vector3.Distance(position.Position, playerEntityPosition.Position);
                Vector3 targetPoint = playerEntityPosition.Position + distance * camera.cameraDirection.Normalized();
                ComponentAudio audio = Engine.Systems.System.GetComponent(entity, ComponentTypes.COMPONENT_AUDIO) as ComponentAudio;
                
                if (Vector3.Distance(targetPoint, position.Position) < 2 && (entity.Mask & ComponentTypes.COMPONENT_HEALTH) != 0)
                {
                    IComponent healthComponent = Engine.Systems.System.GetComponent(entity, ComponentTypes.COMPONENT_HEALTH);
                    ComponentHealth health = (ComponentHealth)healthComponent;
                    if(health.Health > 0)
                    {
                        health.Health--;
                    }
                    if (health.Health <= 0)
                    {
                        aiTarget.Behaviour = AIbehaviour.DEAD;
                        audio.StopSound();
                        audio.PlaySound("Game/Audio/shutdown.wav");
                    }
                }
                shootDelay = 20.5f;
            }
        }
    }
}
