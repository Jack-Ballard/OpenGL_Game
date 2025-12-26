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

                    if ((entity.Mask & ComponentTypes.COMPONENT_VELOCITY) == 0)
                    {
                        if (TestCollision(collision, position, entityPosition.Position, entityCollision))
                        {
                            Console.WriteLine("Collision Detected between AABBs at positions " + position.Position + " and " + entityPosition.Position);
                        }
                    }
                    else
                    {
                        IComponent velocityComponent = GetComponent(entity, ComponentTypes.COMPONENT_VELOCITY);
                        ComponentVelocity entityVelocity = ((ComponentVelocity)velocityComponent);

                        Vector3 nextPosition = entityPosition.Position + new Vector3(entityVelocity.Velocity.X * 1.5f, 0, 0) * GameScene.dt;
                        if (TestCollision(collision, position, nextPosition, entityCollision))
                            entityVelocity.Velocity = new Vector3(-entityVelocity.Velocity.X, entityVelocity.Velocity.Y, entityVelocity.Velocity.Z);
                        else
                            entityVelocity.Velocity = new Vector3(0, entityVelocity.Velocity.Y, entityVelocity.Velocity.Z);

                        nextPosition = entityPosition.Position + new Vector3(0, entityVelocity.Velocity.Y * 1.5f, 0) * GameScene.dt;
                        if (TestCollision(collision, position, nextPosition, entityCollision))
                            entityVelocity.Velocity = new Vector3(entityVelocity.Velocity.X, -entityVelocity.Velocity.Y, entityVelocity.Velocity.Z);
                        else
                            entityVelocity.Velocity = new Vector3(entityVelocity.Velocity.X, 0, entityVelocity.Velocity.Z);

                        nextPosition = entityPosition.Position + new Vector3(0, 0, entityVelocity.Velocity.Z * 1.5f) * GameScene.dt;
                        if (TestCollision(collision, position, nextPosition, entityCollision))
                            entityVelocity.Velocity = new Vector3(entityVelocity.Velocity.X, entityVelocity.Velocity.Y, -entityVelocity.Velocity.Z);
                        else
                            entityVelocity.Velocity = new Vector3(entityVelocity.Velocity.X, entityVelocity.Velocity.Y, 0);
                        if(entityVelocity.Velocity != new Vector3(0,0,0))
                        {
                            Console.WriteLine("Collision Detected between AABBs at positions " + position.Position + " and " + nextPosition);
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
                    if(TestCollision(collision, position, entityPosition.Position))
                    {
                        Console.WriteLine("Collision Detected between AABB at position " + position.Position + " and point at position " + entityPosition.Position);
                    }
                }
            }
        }

        public bool TestCollision(ComponentCollisionAABB collision, ComponentPosition position, Vector3 entityPosition, ComponentCollisionAABB entityCollision = null)
        {
            if (entityCollision == null)
            {
                if (position.Position.X + collision.Xmax >= entityPosition.X &&
                position.Position.X + collision.Xmin <= entityPosition.X &&
                position.Position.Z + collision.Zmax >= entityPosition.Z &&
                position.Position.Z + collision.Zmin <= entityPosition.Z &&
                position.Position.Y + collision.Ymax >= entityPosition.Y &&
                position.Position.Y + collision.Ymin <= entityPosition.Y)
                {
                    return true;
                }
            }
            else if (position.Position.X + collision.Xmax >= entityPosition.X + entityCollision.Xmin &&
                       position.Position.X + collision.Xmin <= entityPosition.X + entityCollision.Xmax &&
                       position.Position.Z + collision.Zmax >= entityPosition.Z + entityCollision.Zmin &&
                       position.Position.Z + collision.Zmin <= entityPosition.Z + entityCollision.Zmax &&
                       position.Position.Y + collision.Ymax >= entityPosition.Y + entityCollision.Ymin &&
                       position.Position.Y + collision.Ymin <= entityPosition.Y + entityCollision.Ymax)
            {
                return true;
            }
            return false;
        }

    }
}
