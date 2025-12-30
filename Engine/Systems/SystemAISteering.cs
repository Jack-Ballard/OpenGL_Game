using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Objects;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Engine.Systems
{
    class SystemAISteering : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AI_TARGET;

        public override void OnAction(List<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                if ((entity.Mask & MASK) == MASK)
                {
                    IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                    ComponentPosition position = (ComponentPosition)positionComponent;

                    IComponent aiTargetComponent = GetComponent(entity, ComponentTypes.COMPONENT_AI_TARGET);
                    ComponentAITarget aiTarget = (ComponentAITarget)aiTargetComponent;

                    // Simple steering behavior: Move towards the target position
                    Vector3 direction = aiTarget.TargetPosition - position.Position;
                    direction = Vector3.Normalize(direction);



                    if ((entity.Mask & ComponentTypes.COMPONENT_VELOCITY) != 0)
                    {
                        IComponent velocityComponent = GetComponent(entity, ComponentTypes.COMPONENT_VELOCITY);
                        ComponentVelocity velocity = (ComponentVelocity)velocityComponent;
                        float speed = 1.0f; // Define a speed for the entity
                        velocity.Velocity = direction * speed;
                    }
                    else
                    {
                        // If no velocity component, just have the direction
                    }
                }
            }
        }
    }
}
