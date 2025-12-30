using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

namespace OpenGL_Game.Engine.Components
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
