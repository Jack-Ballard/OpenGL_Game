using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                IComponent collisionComponent = GetComponent(entity, ComponentTypes.COMPONENT_COLLISION_AABB);
                ComponentCollisionAABB collision = ((ComponentCollisionAABB)collisionComponent);

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

                    if(entityPosition == position)
                    {
                        continue;
                    }

                    IComponent collisionComponent = GetComponent(entity, ComponentTypes.COMPONENT_COLLISION_AABB);
                    ComponentCollisionAABB entityCollision = ((ComponentCollisionAABB)collisionComponent);

                    if(position.Position.X + collision.Xmax >= entityPosition.Position.X + entityCollision.Xmin &&
                       position.Position.X + collision.Xmin <= entityPosition.Position.X + entityCollision.Xmax &&
                       position.Position.Z + collision.Zmax >= entityPosition.Position.Z + entityCollision.Zmin &&
                       position.Position.Z + collision.Zmin <= entityPosition.Position.Z + entityCollision.Zmax &&
                       position.Position.Y + collision.Ymax >= entityPosition.Position.Y + entityCollision.Ymin &&
                       position.Position.Y + collision.Ymin <= entityPosition.Position.Y + entityCollision.Ymax )
                    {
                        Console.WriteLine("Collision Detected between AABBs at positions " + position.Position + " and " + entityPosition.Position);
                    }
                }
                else if ((entity.Mask & ComponentTypes.COMPONENT_POSITION) != 0)
                {
                    IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                    ComponentPosition entityPosition = ((ComponentPosition)positionComponent);

                    if(entityPosition == position)
                    {
                        continue;
                    }
                    if(position.Position.X + collision.Xmax >= entityPosition.Position.X &&
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
    }
}
