using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using System.Collections.Generic;

namespace OpenGL_Game.Systems
{
    abstract class System
    {
        public static IComponent GetComponent(Entity entity, ComponentTypes componentType)
        {
            List<IComponent> components = entity.Components;

            IComponent iComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == componentType;
            });

            return iComponent;
        }

        public static List<IComponent> GetComponentList(Entity entity, ComponentTypes componentType)
        {
            List<IComponent> components = new (entity.Components);

            for(int i = components.Count - 1; i >= 0; i--)
            {
                IComponent component = components[i];
                if (component.ComponentType != componentType)
                {
                    components.RemoveAt(i);
                }
            }

            return components;
        }


        public abstract void OnAction(List<Entity> entity);

        // Property signatures: 
        public string Name
        {
            get;
        }
    }
}
