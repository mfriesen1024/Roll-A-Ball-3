using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.Player
{
    /// <summary>
    /// Keeps track of and/or centralizes player stuff for encapsulation's sake.
    /// </summary>
    internal partial class PlayerManager:Node
    {
        #region Refs
        /// <summary>
        /// Singleton instance of the player.
        /// </summary>
        public static PlayerManager Instance {  get; private set; }

        [Export]PlayerController controller;
        #endregion
    }
}
