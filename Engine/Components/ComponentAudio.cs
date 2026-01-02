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

        public ComponentAudio(string fileName, bool loop = true)
        {
            // Setup Audio Source from the Audio Buffer
            audioBuffer = ResourceManager.LoadAudio(fileName);
            audioSource = AL.GenSource();
            AL.Source(audioSource, ALSourcei.Buffer, audioBuffer); // attach the buffer to a source
            AL.Source(audioSource, ALSourceb.Looping, loop); // source loops infinitely
            //sourcePosition = new Vector3(0.0f, 0.0f, 0.0f); // place the source at position
            AL.Source(audioSource, ALSource3f.Position, ref sourcePosition);
            if(loop) AL.SourcePlay(audioSource); // play the audio source
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }

        public void SetPosition(Vector3 emitterPosition)
        {
            sourcePosition = emitterPosition;
        }

        public void StopSound()
        {
            AL.SourceStop(audioSource);
            AL.Source(audioSource, ALSourceb.Looping, false);
            // Optionally, detach the buffer if you want to fully stop playback
            // AL.Source(audioSource, ALSourcei.Buffer, 0);
        }
        public void PlaySound(string fileName)
        {
            int newAudioBuffer = ResourceManager.LoadAudio(fileName);
            AL.SourceStop(audioSource); // Stop any current playback
            AL.Source(audioSource, ALSourcei.Buffer, newAudioBuffer); // Attach the new buffer
            AL.Source(audioSource, ALSourceb.Looping, false);
            AL.SourcePlay(audioSource); // Play the new sound
        }
        public void PlaySound()
        {
            AL.SourcePlay(audioSource);
        }
    }
}
