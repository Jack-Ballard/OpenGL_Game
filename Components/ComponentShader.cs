using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenTK.Mathematics;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Components
{
    abstract class ComponentShader : IComponent
    {
        public int pgmID;

        public ComponentShader(string vertexShader, string fragmentShader)
        {
            pgmID = GL.CreateProgram();
            GL.AttachShader(pgmID, ResourceManager.LoadShader(vertexShader, ShaderType.VertexShader));
            GL.AttachShader(pgmID, ResourceManager.LoadShader(fragmentShader, ShaderType.FragmentShader));
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));
        }
        public abstract void ApplyShader(Scene scene, Matrix4 model, Geometry geometry);
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SHADER; }
        }
    }
}
