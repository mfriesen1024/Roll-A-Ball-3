using Godot;

namespace RollABall.Assets.src.Player
{
    /// <summary>
    /// Keeps track of and/or centralizes player stuff for encapsulation's sake.
    /// </summary>
    internal partial class PlayerManager : Node
    {
        #region Refs
        public static PlayerManager Instance { get; private set; }

        [Export] public PlayerController controller;
        [Export] RigidBody3D ball;
        [Export] PlayerCam cam;
        #endregion
        #region Stats
        public int Lives { get => lives; set => lives = value; }
        private int lives = 3;
        #endregion

        public override void _Ready()
        {
            if (Instance == null) { Instance = this; }
            else { QueueFree(); }
        }

        /// <summary>
        /// Kills the player
        /// </summary>
        public void OnDamage()
        {
            lives--;
            if (lives >= 0) { log.Write("Level should be reloaded, but levelman not implemented!",LogLevel.warn); }
        }
    }
}
