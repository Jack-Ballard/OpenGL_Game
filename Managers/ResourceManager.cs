using System;
using System.Collections.Generic;
using SkiaSharp;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.OBJLoader;
using System.Net;

namespace OpenGL_Game.Managers
{
    static class ResourceManager
    {
        static Dictionary<string, Geometry> geometryDictionary = new Dictionary<string, Geometry>();
        static Dictionary<string, int> textureDictionary = new Dictionary<string, int>();
        static Dictionary<string, int> shaderDictionary = new Dictionary<string, int>();

        public static void RemoveAllAssets()
        {
            foreach(var geometry in geometryDictionary)
            {
                geometry.Value.RemoveGeometry();
            }
            geometryDictionary.Clear();
            foreach(var texture in textureDictionary)
            {
                GL.DeleteTexture(texture.Value);
            }
            textureDictionary.Clear();
        }

        public static Geometry LoadGeometry(string filename)
        {
            Geometry geometry;
            geometryDictionary.TryGetValue(filename, out geometry);
            if (geometry == null)
            {
                geometry = new Geometry();
                geometry.LoadObject(filename);
                geometryDictionary.Add(filename, geometry);
            }

            return geometry;
        }

        public static int LoadTexture(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int texture;
            textureDictionary.TryGetValue(filename, out texture);
            if (texture == 0)
            {
                texture = GL.GenTexture();
                textureDictionary.Add(filename, texture);
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, texture);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                SKBitmap bmp = SKBitmap.Decode(filename);
                if (bmp.ColorType == SKColorType.Bgra8888)
                {
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0,
                                  OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp.GetPixels());
                }

                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.Disable(EnableCap.Texture2D);
            }

            return texture;
        }

        public static int LoadShader(string filename, ShaderType type)
        {
            int shader;
            shaderDictionary.TryGetValue(filename, out shader);
            if(shader == 0)
            {
                shader = GL.CreateShader(type);

                using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
                {
                    GL.ShaderSource(shader, sr.ReadToEnd());
                }
                GL.CompileShader(shader);
                Console.WriteLine(GL.GetShaderInfoLog(shader));
                shaderDictionary.Add(filename, shader);
            }
            
            return shader;
        }
    }
}
