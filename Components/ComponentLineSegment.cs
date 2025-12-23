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

