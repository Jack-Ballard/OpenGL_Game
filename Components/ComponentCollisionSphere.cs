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
    class ComponentCollisionSphere : IComponent
    {
        public float radius;
        public ComponentCollisionSphere(float radius)
        {
            this.radius = radius;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_SPHERE; }
        }
    }
}
