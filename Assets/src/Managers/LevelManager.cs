using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.Managers
{
    /// <summary>
    /// Manages levels, loading, and ui crap.
    /// </summary>
    internal partial class LevelManager:Node
    {
        /// <summary>
        /// Singleton instance of the levelman.
        /// </summary>
        public LevelManager Instance { get; private set; }

        LevelLoadHelper loadHelper = new();

        [Export] PackedScene[] levels;
        public int LevelIndex = 0;
        public override void _Ready()
        {
            base._Ready();

            // Set singleton
            if (Instance == null) { Instance = this; }
            else { QueueFree(); return; }
        }

    }
}
