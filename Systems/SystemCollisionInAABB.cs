using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace OpenGL_Game.Systems
{
    class SystemCollisionInAABB : System
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_AABB);
        List<Entity> entities;
        public SystemCollisionInAABB(List<Entity> pEntities)
        {
            entities = pEntities;
        }
        public override void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                ComponentPosition position = ((ComponentPosition)positionComponent);

                List<IComponent> collisionComponents = GetComponentList(entity, ComponentTypes.COMPONENT_COLLISION_AABB);
                List<ComponentCollisionAABB> collisions = new List<ComponentCollisionAABB>();
                foreach (IComponent collComp in collisionComponents)
                    collisions.Add((ComponentCollisionAABB)collComp);

                foreach (ComponentCollisionAABB collision in collisions)
                    Collide(position, collision);
            }
        }
        public void Collide(ComponentPosition position, ComponentCollisionAABB collision)
        {
            foreach (Entity entity in entities)
            {
                if ((entity.Mask & MASK) == MASK)
                {
                    IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                    ComponentPosition entityPosition = ((ComponentPosition)positionComponent);

                    if (entityPosition == position)
                    {
                        continue;
                    }

                    IComponent collisionComponent = GetComponent(entity, ComponentTypes.COMPONENT_COLLISION_AABB);
                    ComponentCollisionAABB entityCollision = ((ComponentCollisionAABB)collisionComponent);

                    if (position.Position.X + collision.Xmax >= entityPosition.Position.X + entityCollision.Xmin &&
                       position.Position.X + collision.Xmin <= entityPosition.Position.X + entityCollision.Xmax &&
                       position.Position.Z + collision.Zmax >= entityPosition.Position.Z + entityCollision.Zmin &&
                       position.Position.Z + collision.Zmin <= entityPosition.Position.Z + entityCollision.Zmax &&
                       position.Position.Y + collision.Ymax >= entityPosition.Position.Y + entityCollision.Ymin &&
                       position.Position.Y + collision.Ymin <= entityPosition.Position.Y + entityCollision.Ymax)
                    {
                        Console.WriteLine("Collision Detected between AABBs at positions " + position.Position + " and " + entityPosition.Position);
                        if ((entity.Mask & ComponentTypes.COMPONENT_VELOCITY) != 0)
                        {
                            IComponent velocityComponent = GetComponent(entity, ComponentTypes.COMPONENT_VELOCITY);
                            ComponentVelocity entityVelocity = ((ComponentVelocity)velocityComponent);
                            entityVelocity.Velocity = entityVelocity.Velocity;
                            entityPosition.Position = entityPosition.Position + entityVelocity.Velocity * GameScene.dt;

                        }
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
                    if (position.Position.X + collision.Xmax >= entityPosition.Position.X &&
                       position.Position.X + collision.Xmin <= entityPosition.Position.X &&
                       position.Position.Z + collision.Zmax >= entityPosition.Position.Z &&
                       position.Position.Z + collision.Zmin <= entityPosition.Position.Z &&
                       position.Position.Y + collision.Ymax >= entityPosition.Position.Y &&
                       position.Position.Y + collision.Ymin <= entityPosition.Position.Y)
                    {
                        Console.WriteLine("Collision Detected between AABB at position " + position.Position + " and point at position " + entityPosition.Position);
                    }
                }
            }
        }

        public Vector3 SnapToCardinal(Vector3 vector)
        {
            Vector3 snapped = new Vector3(0, 0, 0);
            if (Math.Abs(vector.X) >= Math.Abs(vector.Y) && Math.Abs(vector.X) >= Math.Abs(vector.Z))
            {
                snapped.X = MathF.Sign(vector.X);
            }
            else if (Math.Abs(vector.Y) >= Math.Abs(vector.X) && Math.Abs(vector.Y) >= Math.Abs(vector.Z))
            {
                snapped.Y = MathF.Sign(vector.Y);
            }
            else if (Math.Abs(vector.Z) >= Math.Abs(vector.X) && Math.Abs(vector.Z) >= Math.Abs(vector.Y))
            {
                snapped.Z = MathF.Sign(vector.Z);
            }
            return snapped;
        }
    }
}
