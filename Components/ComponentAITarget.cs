using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Components
{
    class ComponentAITarget : IComponent
    {
        Vector3 targetPosition;
        public ComponentAITarget(float x, float y, float z)
        {
            targetPosition = new Vector3(x, y, z);
        }

        public ComponentAITarget(Vector3 pos)
        {
            targetPosition = pos;
        }

        public Vector3 TargetPosition
        {
            get { return targetPosition; }
            set { targetPosition = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AI_TARGET; }
        }
    }
}
