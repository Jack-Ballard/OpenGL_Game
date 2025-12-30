using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Engine.Components
{
    class ComponentHealth : IComponent
    {
        int health;

        public ComponentHealth(int healthValue)
        {
            health = healthValue;
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_HEALTH; }
        }
    }
}
