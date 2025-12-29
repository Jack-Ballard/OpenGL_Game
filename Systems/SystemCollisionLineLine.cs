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
    class SystemCollisionLineLine : System
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_LINE);
        CollisionManager _collisionManager;

        public SystemCollisionLineLine(CollisionManager collisionManager)
        {
            _collisionManager = collisionManager;
        }
        public override void OnAction(List<Entity> entities)
        {
            foreach(Entity entity1 in entities)
            {
                if ((entity1.Mask & MASK) == MASK)
                {
                    IComponent position1Component = GetComponent(entity1, ComponentTypes.COMPONENT_POSITION);
                    ComponentPosition position = ((ComponentPosition)position1Component);

                    IComponent collision1Component = GetComponent(entity1, ComponentTypes.COMPONENT_COLLISION_LINE);
                    ComponentCollisionLine collision = ((ComponentCollisionLine)collision1Component);

                    foreach (Entity entity2 in entities)
                    {
                        if ((entity2.Mask & MASK) == MASK)
                        {
                            IComponent position2Component = GetComponent(entity2, ComponentTypes.COMPONENT_POSITION);
                            ComponentPosition entityPosition = ((ComponentPosition)position2Component);

                            if (position == entityPosition)
                            {
                                continue;
                            }

                            IComponent collision2Component = GetComponent(entity2, ComponentTypes.COMPONENT_COLLISION_LINE);
                            ComponentCollisionLine entityCollision = ((ComponentCollisionLine)collision2Component);

                            Vector3 P1 = position.Position;
                            Vector3 D1 = collision.line;

                            Vector3 P2 = entityPosition.Position;
                            Vector3 D2 = entityCollision.line;

                            Vector3 planeNormal = Vector3.Cross(D1, D2);

                            //if (Vector3.Dot(planeNormal, planeNormal) < 1e-6f)
                            //{
                            //    // Parallel lines
                            //    continue;
                            //}

                            Vector3 N1 = Vector3.Normalize(Vector3.Cross(D1, planeNormal));


                            Vector3 V1 = P2 - P1;
                            Vector3 V2 = (P2 + D2) - P1;

                            float d1 = Vector3.Dot(V1, N1);
                            float d2 = Vector3.Dot(V2, N1);

                            if (d1 * d2 > 0)
                                continue;

                            Vector3 N2 = Vector3.Cross(D2, planeNormal);

                            Vector3 U1 = P1 - P2;
                            Vector3 U2 = (P1 + D1) - P2;

                            float e1 = Vector3.Dot(U1, N2);
                            float e2 = Vector3.Dot(U2, N2);

                            if (e1 * e2 > 0)
                                continue;

                            Console.WriteLine("Collision Detected between lines at positions " + position.Position + " and " + entityPosition.Position);
                            _collisionManager.Collision(entity1, entity2, COLLISIONTYPE.LINE_LINE);

                        }
                    }
                }
            }
        }

        //public void Collide(ComponentPosition position, ComponentCollisionLine collision)
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

        //            IComponent collisionComponent = GetComponent(entity, ComponentTypes.COMPONENT_COLLISION_LINE);
        //            ComponentCollisionLine entityCollision = ((ComponentCollisionLine)collisionComponent);

        //            Vector3 P1 = position.Position;
        //            Vector3 D1 = collision.line;

        //            Vector3 P2 = entityPosition.Position;
        //            Vector3 D2 = entityCollision.line;

        //            Vector3 planeNormal = Vector3.Cross(D1, D2);

        //            //if (Vector3.Dot(planeNormal, planeNormal) < 1e-6f)
        //            //{
        //            //    // Parallel lines
        //            //    continue;
        //            //}

        //            Vector3 N1 = Vector3.Normalize(Vector3.Cross(D1, planeNormal));


        //            Vector3 V1 = P2 - P1;
        //            Vector3 V2 = (P2 + D2) - P1;

        //            float d1 = Vector3.Dot(V1, N1);
        //            float d2 = Vector3.Dot(V2, N1);

        //            if (d1 * d2 > 0)
        //                continue;

        //            Vector3 N2 = Vector3.Cross(D2, planeNormal);

        //            Vector3 U1 = P1 - P2;
        //            Vector3 U2 = (P1 + D1) - P2;

        //            float e1 = Vector3.Dot(U1, N2);
        //            float e2 = Vector3.Dot(U2, N2);

        //            if (e1 * e2 > 0)
        //                continue;

        //            Console.WriteLine("Collision Detected between lines at positions " + position.Position + " and " + entityPosition.Position);
                    

        //        }
        //    }
        //}
    }
}
