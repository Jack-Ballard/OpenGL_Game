using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenGL_Game.Systems;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Managers
{
    class GameCollisionManager : CollisionManager
    {

        public GameCollisionManager() : base()
        {
        }

        public override void ProcessCollisions()
        {
            foreach (Collision collision in collisionManifold)
            {
                // Process each collision based on its type  
                switch (collision.collisionType)
                {
                    case COLLISIONTYPE.SPHERE_SPHERE:
                        // Handle sphere-sphere collision  
                        Console.WriteLine("Processing Sphere-Sphere Collision between Entity " + collision.entity1 + " and Entity " + collision.entity2);
                        Entity entityWithHealth;
                        if ((collision.entity1.Mask & ComponentTypes.COMPONENT_HEALTH) != 0)
                            entityWithHealth = collision.entity1;
                        else
                            entityWithHealth = collision.entity2;
                        
                        IComponent healthComponent = Systems.System.GetComponent(entityWithHealth, ComponentTypes.COMPONENT_HEALTH);
                        ComponentHealth health = ((ComponentHealth)healthComponent);
                        health.Health -= 1;
                        IComponent positionComponent = Systems.System.GetComponent(entityWithHealth, ComponentTypes.COMPONENT_POSITION);
                        ComponentPosition position = ((ComponentPosition)positionComponent);
                        position.Position = (-8.0f, 0.5f, 6f);
                        
                        break;
                    case COLLISIONTYPE.POINT_IN_SPHERE:
                        // Handle point-in-sphere collision  
                        Console.WriteLine("Processing Point-In-Sphere Collision between Entity " + collision.entity1 + " and Entity " + collision.entity2);
                        break;
                    case COLLISIONTYPE.POINT_IN_BOX:
                        // Handle point-in-box collision  
                        Console.WriteLine("Processing Point-In-Box Collision between Entity " + collision.entity1 + " and Entity " + collision.entity2);
                        break;
                    case COLLISIONTYPE.AABB_AABB:
                        // Handle AABB-AABB collision  
                        Console.WriteLine("Processing AABB-AABB Collision between Entity " + collision.entity1 + " and Entity " + collision.entity2);

                        Entity staticEntity;
                        Entity dynamicEntity;
                        if ((collision.entity1.Mask & ComponentTypes.COMPONENT_VELOCITY) == 0)
                        {
                            staticEntity = collision.entity1;
                            dynamicEntity = collision.entity2;
                        }
                        else
                        {
                            staticEntity = collision.entity2;
                            dynamicEntity = collision.entity1;
                        }

                        positionComponent = Systems.System.GetComponent(staticEntity, ComponentTypes.COMPONENT_POSITION);
                        ComponentPosition position1 = ((ComponentPosition)positionComponent);
                        List<IComponent> collisionComponents = Systems.System.GetComponentList(staticEntity, ComponentTypes.COMPONENT_COLLISION_AABB);
                        List<ComponentCollisionAABB> collisions = new List<ComponentCollisionAABB>();

                        foreach (IComponent collComp in collisionComponents)
                            collisions.Add((ComponentCollisionAABB)collComp);

                        positionComponent = Systems.System.GetComponent(dynamicEntity, ComponentTypes.COMPONENT_POSITION);
                        ComponentPosition position2 = ((ComponentPosition)positionComponent);
                        IComponent collisionComponent = Systems.System.GetComponent(dynamicEntity, ComponentTypes.COMPONENT_COLLISION_AABB);
                        ComponentCollisionAABB collision2 = ((ComponentCollisionAABB)collisionComponent);
                        IComponent velocityComponent = Systems.System.GetComponent(dynamicEntity, ComponentTypes.COMPONENT_VELOCITY);
                        ComponentVelocity velocity = ((ComponentVelocity)velocityComponent);

                        foreach (ComponentCollisionAABB collide in collisions)
                        {
                            Vector3 repulsionVelocity = new Vector3(0, 0, 0);
                            Vector3 nextPosition = position2.Position + new Vector3(velocity.Velocity.X * 2, 0, 0) * GameScene.dt;

                            if (SystemCollisionInAABB.TestCollision(collide, position1, nextPosition, collision2))
                                repulsionVelocity.X = -velocity.Velocity.X;
                            else
                                repulsionVelocity.X = 0;

                            nextPosition = position2.Position + new Vector3(0, velocity.Velocity.Y * 2, 0) * GameScene.dt;
                            if (SystemCollisionInAABB.TestCollision(collide, position1, nextPosition, collision2))
                                repulsionVelocity.Y = -velocity.Velocity.Y;
                            else
                                repulsionVelocity.Y = 0;

                            nextPosition = position2.Position + new Vector3(0, 0, velocity.Velocity.Z * 2) * GameScene.dt;
                            if (SystemCollisionInAABB.TestCollision(collide, position1, nextPosition, collision2))
                                repulsionVelocity.Z = -velocity.Velocity.Z;
                            else
                                repulsionVelocity.Z = 0;

                            if (repulsionVelocity != new Vector3(0, 0, 0))
                            {
                                position2.Position = position2.Position + repulsionVelocity * 1.005f * GameScene.dt;
                            }
                        }

                        break;
                    default:
                        Console.WriteLine("Unknown Collision Type");
                        break;
                }
            }
            // Clear the manifold after processing  
            ClearManifold();
        }
        
    }
}
