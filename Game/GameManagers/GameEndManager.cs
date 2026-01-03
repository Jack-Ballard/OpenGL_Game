using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Game.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Game.GameManagers
{
    class GameEndManager
    {
        private EntityManager entityManager;
        private string _playerName;
        public GameEndManager(EntityManager EntityManager, string playerName)
        {
            entityManager = EntityManager;
            _playerName = playerName;
        }
        public bool CheckGameEnd(bool timer)
        {
            if (timer) { return true; }
            foreach (IComponent component in entityManager.FindEntity(_playerName).Components)
            {
                if (component.ComponentType == ComponentTypes.COMPONENT_HEALTH)
                {
                    ComponentHealth healthComponent = (ComponentHealth)component;
                    if(healthComponent.Health <= 0)
                    {
                        return true;
                    }
                }
            }
            foreach (IComponent component in entityManager.FindEntity("Enemy_1").Components)
            {
                if (component.ComponentType == ComponentTypes.COMPONENT_HEALTH)
                {
                    ComponentHealth healthComponent = (ComponentHealth)component;
                    if(healthComponent.Health <= 0)
                    {
                        foreach (IComponent component2 in entityManager.FindEntity("Enemy_2").Components)
                        {
                            if (component2.ComponentType == ComponentTypes.COMPONENT_HEALTH)
                            {
                                ComponentHealth healthComponent2 = (ComponentHealth)component2;
                                if(healthComponent2.Health <= 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            
            return false;
        }
    }
}
