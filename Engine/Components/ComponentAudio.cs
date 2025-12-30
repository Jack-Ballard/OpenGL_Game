using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;
//using System.Resources; // NEW for Audio
using OpenGL_Game.Engine.Managers;

namespace OpenGL_Game.Engine.Components
{
    class ComponentAudio : IComponent
    {
        public Vector3 sourcePosition; // NEW for Audio
        public int audioBuffer;        // NEW for Audio
        public int audioSource;        // NEW for Audio

        public ComponentAudio()
        {
            // Setup Audio Source from the Audio Buffer
            audioBuffer = ResourceManager.LoadAudio("Audio/buzz.wav");
            audioSource = AL.GenSource();
            AL.Source(audioSource, ALSourcei.Buffer, audioBuffer); // attach the buffer to a source
            AL.Source(audioSource, ALSourceb.Looping, true); // source loops infinitely
            //sourcePosition = new Vector3(0.0f, 0.0f, 0.0f); // place the source at position
            AL.Source(audioSource, ALSource3f.Position, ref sourcePosition);
            AL.SourcePlay(audioSource); // play the audio source
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }

        public void SetPosition(Vector3 emitterPosition)
        {
            sourcePosition = emitterPosition;
        }
    }
}
