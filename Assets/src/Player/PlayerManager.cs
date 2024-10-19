using Godot;
using KeystoneUtils.Logging;
using RollABall.Assets.src.Managers;

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
            log.WriteAll($"Player died. Player has {Lives} lives left.");
            Lives--;
            UIManager.Instance.hud.Update();
            if (Lives >= 0) { LevelManager.Instance.Reload(); }
            else { UIManager.Instance.State = UIState.LevelFailure; Lives = 3; }
        }
    }
}
