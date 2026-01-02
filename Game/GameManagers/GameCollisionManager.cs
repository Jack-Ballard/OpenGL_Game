using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Objects;
using OpenGL_Game.Engine.Systems;
using OpenGL_Game.Game.Scenes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Game.GameManagers
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
                        Entity entityPlayer;
                        Entity entityNotPlayer;
                        if (collision.entity1.Name == "Player")
                        { entityPlayer = collision.entity1; entityNotPlayer = collision.entity2; }

                        else if (collision.entity2.Name == "Player")
                        { entityPlayer = collision.entity2; entityNotPlayer = collision.entity1; }
                        else
                            break;

                        IComponent healthComponent = Engine.Systems.System.GetComponent(entityPlayer, ComponentTypes.COMPONENT_HEALTH);
                        ComponentHealth health = (ComponentHealth)healthComponent;
                        IComponent positionComponent = Engine.Systems.System.GetComponent(entityPlayer, ComponentTypes.COMPONENT_POSITION);
                        ComponentPosition position = (ComponentPosition)positionComponent;

                        if(entityNotPlayer.Name == "Item1")
                        {
                            ComponentAudio audio = Engine.Systems.System.GetComponent(entityNotPlayer, ComponentTypes.COMPONENT_AUDIO) as ComponentAudio;
                            audio.StopSound();
                            audio.PlaySound("Game/Audio/itemCollected.wav");
                            ComponentPosition componentPosition = Engine.Systems.System.GetComponent(entityNotPlayer, ComponentTypes.COMPONENT_POSITION) as ComponentPosition;
                            componentPosition.Position = new Vector3(-100.0f, -100.0f, -100.0f); // Move the powerup out of the way
                            // Powerup 1
                        }
                        else if(entityNotPlayer.Name == "Item2")
                        {
                            ComponentAudio audio = Engine.Systems.System.GetComponent(entityNotPlayer, ComponentTypes.COMPONENT_AUDIO) as ComponentAudio;
                            audio.StopSound();
                            audio.PlaySound("Game/Audio/itemCollected.wav");
                            ComponentPosition componentPosition = Engine.Systems.System.GetComponent(entityNotPlayer, ComponentTypes.COMPONENT_POSITION) as ComponentPosition;
                            componentPosition.Position = new Vector3(-100.0f, -100.0f, -100.0f); // Move the powerup out of the way
                            // Powerup 2
                        }
                        else
                        {
                            position.Position = (-8.0f, 0.5f, 6f);
                            health.Health--;
                            ComponentAudio audio = Engine.Systems.System.GetComponent(entityPlayer, ComponentTypes.COMPONENT_AUDIO) as ComponentAudio;
                            audio.PlaySound("Game/Audio/hurt.wav");
                            positionComponent = Engine.Systems.System.GetComponent(entityNotPlayer, ComponentTypes.COMPONENT_POSITION);
                            position = (ComponentPosition)positionComponent;
                            position.Position = (15.0f, 0.0f, -15.0f); // This is me hardcoding the respawn point for the player and enemy, this is bad don't do this at home kids

                        }

                        break;
                    case COLLISIONTYPE.POINT_IN_SPHERE:
                        // Handle point-in-sphere collision  
                        Console.WriteLine("Processing Point-In-Sphere Collision between Entity " + collision.entity1 + " and Entity " + collision.entity2);
                        break;
                    case COLLISIONTYPE.POINT_IN_AABB:
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

                        positionComponent = Engine.Systems.System.GetComponent(staticEntity, ComponentTypes.COMPONENT_POSITION);
                        ComponentPosition position1 = (ComponentPosition)positionComponent;
                        List<IComponent> collisionComponents = Engine.Systems.System.GetComponentList(staticEntity, ComponentTypes.COMPONENT_COLLISION_AABB);
                        List<ComponentCollisionAABB> collisions = new List<ComponentCollisionAABB>();

                        foreach (IComponent collComp in collisionComponents)
                            collisions.Add((ComponentCollisionAABB)collComp);

                        positionComponent = Engine.Systems.System.GetComponent(dynamicEntity, ComponentTypes.COMPONENT_POSITION);
                        ComponentPosition position2 = (ComponentPosition)positionComponent;
                        IComponent collisionComponent = Engine.Systems.System.GetComponent(dynamicEntity, ComponentTypes.COMPONENT_COLLISION_AABB);
                        ComponentCollisionAABB collision2 = (ComponentCollisionAABB)collisionComponent;
                        IComponent velocityComponent = Engine.Systems.System.GetComponent(dynamicEntity, ComponentTypes.COMPONENT_VELOCITY);
                        ComponentVelocity velocity = (ComponentVelocity)velocityComponent;

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
                    case COLLISIONTYPE.AABB_LINE:
                        // Handle AABB-line collision
                        if (collision.entity1.Name == "Maze")
                        {
                            staticEntity = collision.entity1;
                            dynamicEntity = collision.entity2;
                        }
                        else if (collision.entity2.Name == "Maze")
                        {
                            staticEntity = collision.entity2;
                            dynamicEntity = collision.entity1;
                        }
                        else
                        {
                            Console.WriteLine("No Maze Entity found in AABB-Line Collision");
                            break;
                        }

                        positionComponent = Engine.Systems.System.GetComponent(staticEntity, ComponentTypes.COMPONENT_POSITION);
                        position1 = (ComponentPosition)positionComponent;
                        collisionComponents = Engine.Systems.System.GetComponentList(staticEntity, ComponentTypes.COMPONENT_COLLISION_AABB);
                        collisions = new List<ComponentCollisionAABB>();

                        foreach (IComponent collComp in collisionComponents)
                            collisions.Add((ComponentCollisionAABB)collComp);

                        positionComponent = Engine.Systems.System.GetComponent(dynamicEntity, ComponentTypes.COMPONENT_POSITION);
                        position2 = (ComponentPosition)positionComponent;
                        collisionComponent = Engine.Systems.System.GetComponent(dynamicEntity, ComponentTypes.COMPONENT_COLLISION_LINE);
                        ComponentCollisionLine collision2line = (ComponentCollisionLine)collisionComponent;

                        if((dynamicEntity.Mask & ComponentTypes.COMPONENT_AI_TARGET) != 0)
                        {
                            IComponent AIcomponent = Engine.Systems.System.GetComponent(dynamicEntity, ComponentTypes.COMPONENT_AI_TARGET);
                            ComponentAITarget aiTarget = (ComponentAITarget)AIcomponent;
                            if(aiTarget.Behaviour == AIbehaviour.CHASE)
                            {
                                aiTarget.Behaviour = AIbehaviour.IDLE;
                            }
                        }

                        Console.WriteLine("Processing AABB-Line Collision between Entity " + collision.entity1 + " and Entity " + collision.entity2 + " at " + position2.Position);
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
