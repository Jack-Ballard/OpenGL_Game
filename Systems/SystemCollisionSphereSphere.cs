using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using OpenGL_Game.Managers;


namespace OpenGL_Game.Systems
{
    class SystemCollisionSphereSphere : System
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE);
        List<Entity> entities;
        public SystemCollisionSphereSphere(List<Entity> pEntities)
        {
            entities = pEntities;
        }
        public override void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition position = ((ComponentPosition)positionComponent);

                IComponent collisionComponent = GetComponent(entity, ComponentTypes.COMPONENT_COLLISION_SPHERE);
                ComponentCollisionSphere collision = ((ComponentCollisionSphere)collisionComponent);

                Collide(position, collision);
            }
        }

        public void Collide(ComponentPosition position, ComponentCollisionSphere collision)
        {
            foreach (Entity entity in entities)
            {
                if ((entity.Mask & MASK) == MASK)
                {
                    IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                    ComponentPosition entityPosition = ((ComponentPosition)positionComponent);

                    if (position == entityPosition)
                    {
                        continue;
                    }

                    IComponent collisionComponent = GetComponent(entity, ComponentTypes.COMPONENT_COLLISION_SPHERE);
                    ComponentCollisionSphere entityCollision = ((ComponentCollisionSphere)collisionComponent);

                    float distance = (position.Position - entityPosition.Position).Length;
                    if (distance < (collision.radius + entityCollision.radius))
                    {
                        Console.WriteLine("Collision Detected between spheres at positions " + position.Position + " and " + entityPosition.Position);
                    }

                }
                else if ((entity.Mask & ComponentTypes.COMPONENT_POSITION) != 0)
                {
                    IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                    ComponentPosition entityPosition = ((ComponentPosition)positionComponent);

                    if (entityPosition == position)
                    {
                        continue;
                    }
                    if ((entityPosition.Position - position.Position).Length < collision.radius)
                    {
                        Console.WriteLine("Collision Detected between sphere at position " + position.Position + " and point at position " + entityPosition.Position);
                    }
                }
            }
        }
    }
}
