using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Objects;
using OpenGL_Game.Game.Scenes;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Engine.Systems
{
    internal class SystemAudio : System
    {
        const ComponentTypes MASK = ComponentTypes.COMPONENT_AUDIO | ComponentTypes.COMPONENT_POSITION;

        protected Scene scene;
        public SystemAudio(Scene pScene)
        {
            scene = pScene;
        }

        public override void OnAction(List<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                if ((entity.Mask & MASK) == MASK)
                {
                    IComponent audioComponent = GetComponent(entity, ComponentTypes.COMPONENT_AUDIO);
                    ComponentAudio audio = (ComponentAudio)audioComponent;

                    IComponent positionComponent = GetComponent(entity, ComponentTypes.COMPONENT_POSITION);
                    ComponentPosition position = (ComponentPosition)positionComponent;

                    PlaySound(ref audio, ref position);
                }
            }
        }

        public void PlaySound(ref ComponentAudio audio, ref ComponentPosition position)
        {
            // Update OpenAL Listener Position and Orientation based on the camera
            AL.Listener(ALListener3f.Position, ref scene.camera.cameraPosition);
            AL.Listener(ALListenerfv.Orientation, ref scene.camera.cameraDirection, ref scene.camera.cameraUp);

            audio.SetPosition(position.Position);

            AL.Source(audio.audioSource, ALSource3f.Position, ref audio.sourcePosition);
        }

        public void OnClose(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                IComponent audioComponent = GetComponent(entity, ComponentTypes.COMPONENT_AUDIO);
                ComponentAudio audio = (ComponentAudio)audioComponent;

                Cleanup(ref audio);
            }
        }

        public void Cleanup(ref ComponentAudio audio)
        {
            AL.SourceStop(audio.audioSource);     // NEW for Audio
            AL.DeleteSource(audio.audioSource);   // NEW for Audio
            AL.DeleteBuffer(audio.audioBuffer);   // NEW for Audio
        }
    }
}
