using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenTK.Mathematics;
using OpenGL_Game.Managers;
using System;

namespace OpenGL_Game.Components
{
    class ComponentCollisionAABB : IComponent
    {
        
        public float Xmax = 0;
        public float Xmin = 0;
        public float Ymax = 0;
        public float Ymin = 0;
        public float Zmax = 0;
        public float Zmin = 0;
        public ComponentCollisionAABB(float zmax, float zmin, float xmax, float xmin, float ymax, float ymin)
        {
            Zmax = zmax;
            Zmin = zmin;
            Xmax = xmax;
            Xmin = xmin;
            Ymax = ymax;
            Ymin = ymin;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_AABB; }
        }
    }
}
