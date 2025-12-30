using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenGL_Game.Engine.OBJLoader;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Objects;
using OpenGL_Game.Game.Scenes;

namespace OpenGL_Game.Engine.Systems
{
    class SystemRender : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_SHADER;
        Scene scene;

        //protected int pgmID;
        //protected int vsID;
        //protected int fsID;
        //protected int uniform_stex;
        //protected int uniform_mmodelviewproj;
        //protected int uniform_mmodel;
        //protected int uniform_diffuse;

        public SystemRender(Scene scene)
        {
            this.scene = scene;
            //pgmID = GL.CreateProgram();
            //LoadShader("Shaders/single-light.vert", ShaderType.VertexShader, pgmID, out vsID);
            //LoadShader("Shaders/single-light.frag", ShaderType.FragmentShader, pgmID, out fsID);
            //GL.LinkProgram(pgmID);

            //GL.GetProgram(pgmID, GetProgramParameterName.LinkStatus, out int success);
            //if (success == 0)
            //{
            //    string infoLog = GL.GetProgramInfoLog(pgmID);
            //    Console.WriteLine(infoLog);
            //}

            //Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            //uniform_stex = GL.GetUniformLocation(pgmID, "s_texture");
            //uniform_mmodelviewproj = GL.GetUniformLocation(pgmID, "ModelViewProjMat");
            //uniform_mmodel = GL.GetUniformLocation(pgmID, "ModelMat");
            //uniform_diffuse = GL.GetUniformLocation(pgmID, "v_diffuse");
        }

        //void LoadShader(String filename, ShaderType type, int program, out int address)
        //{
        //    address = GL.CreateShader(type);
        //    using (StreamReader sr = new StreamReader(filename))
        //    {
        //        GL.ShaderSource(address, sr.ReadToEnd());
        //    }
        //    GL.CompileShader(address);


        //    GL.GetShader(address, ShaderParameter.CompileStatus, out int success);
        //    if (success == 0)
        //    {
        //        string infoLog = GL.GetShaderInfoLog(address);
        //        Console.WriteLine(infoLog);
        //    }

        //    GL.AttachShader(program, address);
        //}

        public string Name
        {
            get { return "SystemRender"; }
        }

        public override void OnAction(List<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                if ((entity.Mask & MASK) == MASK)
                {
                    List<IComponent> geometryComponent = GetComponentList(entity, ComponentTypes.COMPONENT_GEOMETRY);
                    List<Geometry> geometry = new List<Geometry>();
                    foreach (IComponent geoComp in geometryComponent)
                        geometry.Add(((ComponentGeometry)geoComp).Geometry());


                    IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                    Vector3 position = ((ComponentPosition)positionComponent).Position;
                    Matrix4 model = Matrix4.CreateTranslation(position);

                    IComponent shaderComponent = GetComponent(entity, ComponentTypes.COMPONENT_SHADER);
                    ComponentShader shader = (ComponentShader)shaderComponent;

                    foreach (Geometry geo in geometry)
                        Draw(model, geo, shader);
                }
            }

        }

        public void Draw(Matrix4 model, Geometry geometry, ComponentShader shaderComponent)
        {
            shaderComponent.ApplyShader(scene, model, geometry);

            //GL.UseProgram(pgmID);

            //GL.Uniform1(uniform_stex, 0);
            //GL.ActiveTexture(TextureUnit.Texture0);

            //GL.UniformMatrix4(uniform_mmodel, false, ref model);
            //Matrix4 modelViewProjection = model * GameScene.gameInstance.camera.view * GameScene.gameInstance.camera.projection;
            //GL.UniformMatrix4(uniform_mmodelviewproj, false, ref modelViewProjection);

            //geometry.Render(uniform_diffuse);

            //GL.UseProgram(0);
        }
    }
}
