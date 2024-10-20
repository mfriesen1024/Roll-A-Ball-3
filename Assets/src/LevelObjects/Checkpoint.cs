using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.LevelObjects
{
    /// <summary>
    /// Represents a safe location within a level, where the player can be respawned if they die.
    /// </summary>
    internal partial class Checkpoint:Area3D
    {
        [Export] public Vector3 offset = new Vector3(0,1,0);
        public int index;
    }
}
