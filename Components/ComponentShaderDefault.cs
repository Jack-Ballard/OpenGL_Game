using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenTK.Mathematics;

namespace OpenGL_Game.Components
{
    class ComponentShaderDefault : ComponentShader
    {
        public int uniform_stex;
        public int uniform_modelviewproj;
        public int uniform_mmodel;
        public int uniform_diffuse;

        public ComponentShaderDefault() : base("Shaders/single-light.vert", "Shaders/single-light.frag")
        {
            uniform_stex = GL.GetUniformLocation(pgmID, "s_texture");
            uniform_modelviewproj = GL.GetUniformLocation(pgmID, "ModelViewProjMat");
            uniform_mmodel = GL.GetUniformLocation(pgmID, "ModelMat");
            uniform_diffuse = GL.GetUniformLocation(pgmID, "v_diffuse");
        }

        public override void ApplyShader(Scene scene, Matrix4 model, Geometry geometry)
        {
            GL.UseProgram(pgmID);

            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);

            GL.UniformMatrix4(uniform_mmodel, false, ref model);
            Matrix4 modelViewProjection = model * scene.camera.view * scene.camera.projection;
            GL.UniformMatrix4(uniform_modelviewproj, false, ref modelViewProjection);

            geometry.Render(uniform_diffuse);

            GL.UseProgram(0);
        }
    }
}
