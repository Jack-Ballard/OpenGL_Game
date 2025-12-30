using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL_Game.Game.Scenes.Scene;

namespace OpenGL_Game.Engine.Managers
{
    abstract class InputManager
    {
        protected bool[] keysPressed = new bool[512];
        protected bool[] mouseButtonsPressed = new bool[8]; // 8 covers typical mouse buttons

        public abstract void Update();

        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = true;
        }
        public void Keyboard_KeyUp(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = false;
        }

        public virtual void Mouse_BottonPressed(MouseButtonEventArgs e)
        {
            mouseButtonsPressed[(int)e.Button] = true;
        }

        public virtual void Mouse_ButtonReleased(MouseButtonEventArgs e)
        {
            mouseButtonsPressed[(int)e.Button] = false;
        }
    }
}
