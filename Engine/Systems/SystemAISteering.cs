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
        private static readonly Random random = new Random();

        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AI_TARGET | ComponentTypes.COMPONENT_COLLISION_LINE;

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

                    IComponent lineComponent = GetComponent(entity, ComponentTypes.COMPONENT_COLLISION_LINE);
                    ComponentCollisionLine line = (ComponentCollisionLine)lineComponent;

                    ComponentPosition targetComponentPosition = null;
                    foreach (Entity targetEntity in entities)
                    {
                        if (targetEntity.Name == aiTarget.TargetName)
                        {
                            IComponent targetPositionComponent = GetComponent(targetEntity, ComponentTypes.COMPONENT_POSITION);
                            targetComponentPosition = (ComponentPosition)targetPositionComponent;
                            if (aiTarget.Behaviour == AIbehaviour.CHASE)
                            {
                                aiTarget.Target = targetComponentPosition.Position;
                            }
                            break;
                        }
                    }
                    bool playerObstructed = false;
                    if(aiTarget.Behaviour == AIbehaviour.ROAM)
                    {
                        if(Vector3.Distance(position.Position, aiTarget.Target) < 5.0f)
                        {
                            aiTarget.Behaviour = AIbehaviour.IDLE;
                        }
                    }
                    if (aiTarget.Behaviour == AIbehaviour.IDLE)
                    {
                        List<Vector3> visiblePositions = new List<Vector3>();
                        foreach(Entity entity2 in entities)
                        {
                            if ((entity2.Mask & ComponentTypes.COMPONENT_COLLISION_AABB)==0)
                            {
                                continue;
                            }
                            IComponent positionComponentAABB = GetComponent(entity2, ComponentTypes.COMPONENT_POSITION);
                            ComponentPosition entitypositionAABB = (ComponentPosition)positionComponentAABB;

                            List<IComponent> collisionComponentsAABB = GetComponentList(entity2, ComponentTypes.COMPONENT_COLLISION_AABB);
                            List<ComponentCollisionAABB> entitycollisionsAABB = new List<ComponentCollisionAABB>();
                            foreach (IComponent collComp in collisionComponentsAABB)
                                entitycollisionsAABB.Add((ComponentCollisionAABB)collComp);

                            foreach(ComponentCollisionAABB collisionAABB in entitycollisionsAABB)
                            {
                                foreach(Vector3 value in aiTarget.Positions
                            .Where(pos => !SystemCollisionInAABB.LineSegmentAABBIntersect(position.Position, position.Position + line.line, collisionAABB, entitypositionAABB.Position))
                            .ToList())
                                {
                                    visiblePositions.Add(value);
                                }
                                if(SystemCollisionInAABB.LineSegmentAABBIntersect(position.Position, targetComponentPosition.Position, collisionAABB, entitypositionAABB.Position))
                                {
                                    playerObstructed = true;
                                }
                            }
                            
                        }
                        

                        if (visiblePositions.Count > 0)
                        {
                            aiTarget.Target = visiblePositions[random.Next(visiblePositions.Count)];
                            aiTarget.Behaviour = AIbehaviour.ROAM;
                        }
                        else
                        {
                            // Fallback: stay in place
                            aiTarget.Target = position.Position;
                        }
                    }
                    if(!playerObstructed)
                    {
                        aiTarget.Behaviour = AIbehaviour.CHASE;
                    }

                    // Simple steering behavior: Move towards the target position
                    Vector3 direction = aiTarget.Target - position.Position;
                    if (direction.LengthSquared > 0.0001f)
                        direction = Vector3.Normalize(direction);
                    else
                        direction = Vector3.Zero;

                    if ((entity.Mask & ComponentTypes.COMPONENT_COLLISION_LINE) != 0)
                    {
                        IComponent collisionLineComponent = GetComponent(entity, ComponentTypes.COMPONENT_COLLISION_LINE);
                        ComponentCollisionLine collisionLine = (ComponentCollisionLine)collisionLineComponent;
                        collisionLine.line = aiTarget.Target - position.Position;
                    }
                    if ((entity.Mask & ComponentTypes.COMPONENT_VELOCITY) != 0)
                    {
                        IComponent velocityComponent = GetComponent(entity, ComponentTypes.COMPONENT_VELOCITY);
                        ComponentVelocity velocity = (ComponentVelocity)velocityComponent;
                        float speed = 1.0f; // Define a speed for the entity
                        velocity.Velocity = direction * speed;
                    }
                    
                }
            }
        }
    }
}
