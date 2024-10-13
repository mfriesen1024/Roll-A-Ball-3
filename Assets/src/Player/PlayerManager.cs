using Godot;
using KeystoneUtils.Logging;

namespace RollABall.Assets.src.Player
{
    /// <summary>
    /// Keeps track of and/or centralizes player stuff for encapsulation's sake.
    /// </summary>
    internal partial class PlayerManager : Node
    {
        #region Refs
        public static PlayerManager Instance { get; private set; }
        public Logger log;

        [Export] public PlayerController controller;
        public RigidBody3D Ball { get => ball; set => ball = value; }
        [Export] RigidBody3D ball;
        [Export] PlayerCam cam;
        #endregion
        #region Stats
        public int Lives { get => lives; set => lives = value; }
        private int lives = 3;
        #endregion

        public override void _Ready()
        {
            if (Instance != null) { QueueFree(); return; }

            Instance = this;
            log = new Logger(true, true, "logs\\", "playerLog", "txt", false);
        }

        /// <summary>
        /// Kills the player
        /// </summary>
        public void OnDamage()
        {
            lives--;
            if (lives >= 0) { Logger.StaticLogger.Write("Level should be reloaded, but levelman not implemented!", LogLevel.warn); }
            else { Logger.StaticLogger.Write("Should be gameover!", LogLevel.warn); }
        }
    }
}
