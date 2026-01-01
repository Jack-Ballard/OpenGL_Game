using OpenGL_Game.Engine.Objects;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Engine.Components
{
    enum AIbehaviour
    {
        CHASE,
        ROAM,
        IDLE
    }
    class ComponentAITarget : IComponent
    {
        private AIbehaviour behaviour;
        string playerName; 
        private List<Vector3> positions = new List<Vector3>();
        private Vector3 target;
        private Vector3 playerPosition;

        public ComponentAITarget(string name, List<Vector3> positions)
        {
            playerName = name;
            this.positions = positions;
        }

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }
        public Vector3 PlayerPosition
        {
            get { return playerPosition;  }
            set { playerPosition = value; }
        }
        public Vector3 Target
        {
            get { return target; }
            set { target = value; }
        }
        public List<Vector3> Positions
        {
            get { return positions; }
            set { positions = value; }
        }
        public AIbehaviour Behaviour
        {
            get { return behaviour; }
            set { behaviour = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AI_TARGET; }
        }
    }
}
