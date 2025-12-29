using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    enum COLLISIONTYPE
    {
        SPHERE_SPHERE,
        POINT_IN_SPHERE,
        POINT_IN_BOX,
        AABB_AABB,
        LINE_LINE
    }
    struct Collision
    {
        public Entity entity1;
        public Entity entity2;
        public COLLISIONTYPE collisionType;
    }
    abstract class CollisionManager
    {
        protected List<Collision> collisionManifold = new List<Collision>();
        public CollisionManager() { }
        public void ClearManifold() { collisionManifold.Clear(); }
        public void Collision(Entity entity1, Entity entity2, COLLISIONTYPE collisionType)
        {
            foreach(Collision col in collisionManifold)
            {
                if ((col.entity1 == entity1 && col.entity2 == entity2) ||
                   (col.entity1 == entity2 && col.entity2 == entity1))
                {
                    return;
                }
            }

            Collision collision;
            collision.entity1 = entity1;
            collision.entity2 = entity2;
            collision.collisionType = collisionType;
            collisionManifold.Add(collision);
        }

        public abstract void ProcessCollisions();
    }
}
