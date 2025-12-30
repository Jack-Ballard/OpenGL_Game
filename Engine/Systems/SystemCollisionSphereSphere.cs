using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Objects;


namespace OpenGL_Game.Engine.Systems
{
    class SystemCollisionSphereSphere : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE;
        CollisionManager _collisionManager;
        public SystemCollisionSphereSphere(CollisionManager collisionManager)
        {
            _collisionManager = collisionManager;
        }
        public override void OnAction(List<Entity> entities)
        {
            foreach (Entity entity1 in entities)
            {
                if ((entity1.Mask & MASK) == MASK)
                {
                    IComponent position1Component = GetComponent(entity1, ComponentTypes.COMPONENT_POSITION);
                    ComponentPosition position = (ComponentPosition)position1Component;

                    IComponent collision1Component = GetComponent(entity1, ComponentTypes.COMPONENT_COLLISION_SPHERE);
                    ComponentCollisionSphere collision = (ComponentCollisionSphere)collision1Component;

                    foreach (Entity entity2 in entities)
                    {
                        if ((entity2.Mask & MASK) == MASK)
                        {
                            IComponent position2Component = GetComponent(entity2, ComponentTypes.COMPONENT_POSITION);
                            ComponentPosition entityPosition = (ComponentPosition)position2Component;

                            if (position == entityPosition)
                            {
                                continue;
                            }

                            IComponent collision2Component = GetComponent(entity2, ComponentTypes.COMPONENT_COLLISION_SPHERE);
                            ComponentCollisionSphere entityCollision = (ComponentCollisionSphere)collision2Component;

                            float distance = (position.Position - entityPosition.Position).Length;
                            if (distance < collision.radius + entityCollision.radius)
                            {
                                //Console.WriteLine("Collision Detected between spheres at positions " + position.Position + " and " + entityPosition.Position);
                                _collisionManager.Collision(entity1, entity2, COLLISIONTYPE.SPHERE_SPHERE);
                            }

                        }
                        else if ((entity2.Mask & ComponentTypes.COMPONENT_POSITION) != 0)
                        {
                            IComponent positionComponent = GetComponent(entity2, ComponentTypes.COMPONENT_POSITION);
                            ComponentPosition entityPosition = (ComponentPosition)positionComponent;

                            if (entityPosition == position)
                            {
                                continue;
                            }
                            if ((entityPosition.Position - position.Position).Length < collision.radius)
                            {
                                //Console.WriteLine("Collision Detected between sphere at position " + position.Position + " and point at position " + entityPosition.Position);
                                _collisionManager.Collision(entity1, entity2, COLLISIONTYPE.POINT_IN_SPHERE);
                            }
                        }
                    }
                }
            }
        }

        //public void Collide(ComponentPosition position, ComponentCollisionSphere collision)
        //{
        //    foreach (Entity entity in entities)
        //    {
        //        if ((entity.Mask & MASK) == MASK)
        //        {
        //            IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
        //            ComponentPosition entityPosition = ((ComponentPosition)positionComponent);

        //            if (position == entityPosition)
        //            {
        //                continue;
        //            }

        //            IComponent collisionComponent = GetComponent(entity, ComponentTypes.COMPONENT_COLLISION_SPHERE);
        //            ComponentCollisionSphere entityCollision = ((ComponentCollisionSphere)collisionComponent);

        //            float distance = (position.Position - entityPosition.Position).Length;
        //            if (distance < (collision.radius + entityCollision.radius))
        //            {
        //                Console.WriteLine("Collision Detected between spheres at positions " + position.Position + " and " + entityPosition.Position);
        //            }

        //        }
        //        else if ((entity.Mask & ComponentTypes.COMPONENT_POSITION) != 0)
        //        {
        //            IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
        //            ComponentPosition entityPosition = ((ComponentPosition)positionComponent);

        //            if (entityPosition == position)
        //            {
        //                continue;
        //            }
        //            if ((entityPosition.Position - position.Position).Length < collision.radius)
        //            {
        //                Console.WriteLine("Collision Detected between sphere at position " + position.Position + " and point at position " + entityPosition.Position);
        //            }
        //        }
        //    }
        //}
    }
}
