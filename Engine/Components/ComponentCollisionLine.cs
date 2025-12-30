using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

namespace OpenGL_Game.Engine.Components
{
    class ComponentCollisionLine : IComponent
    {

        public Vector3 line;
        public ComponentCollisionLine(Vector3 Line)
        {
            line = Line;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_LINE; }
        }
    }
}

