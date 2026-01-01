using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Engine.Systems
{
    class SystemCollisionLineAABB : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_LINE;
        CollisionManager _collisionManager;

        public SystemCollisionLineAABB(CollisionManager collisionManager)
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
                    ComponentPosition entity1position = (ComponentPosition)position1Component;

                    IComponent collision1Component = GetComponent(entity1, ComponentTypes.COMPONENT_COLLISION_LINE);
                    ComponentCollisionLine entity1collision = (ComponentCollisionLine)collision1Component;

                    foreach (Entity entity2 in entities)
                    {
                        if ((entity2.Mask & MASK) == MASK)
                        {
                            IComponent position2Component = GetComponent(entity2, ComponentTypes.COMPONENT_POSITION);
                            ComponentPosition entity2Position = (ComponentPosition)position2Component;

                            if (entity1position == entity2Position)
                            {
                                continue;
                            }

                            List<IComponent> collisionComponents = GetComponentList(entity2, ComponentTypes.COMPONENT_COLLISION_AABB);
                            List<ComponentCollisionAABB> entity2collisions = new List<ComponentCollisionAABB>();
                            foreach (IComponent collComp in collisionComponents)
                                entity2collisions.Add((ComponentCollisionAABB)collComp);

                            foreach (ComponentCollisionAABB collision in entity2collisions)
                            {

                            }
                        }
                    }
                }
            }
        }
    }
}
