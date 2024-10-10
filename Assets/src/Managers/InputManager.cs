using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.Managers
{
    internal partial class InputManager:Node
    {
        public static InputManager instance { get; private set; }
    }
}
