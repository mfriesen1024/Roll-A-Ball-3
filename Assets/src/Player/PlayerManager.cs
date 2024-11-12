using Godot;
using KeystoneUtils.Logging;
using RollABall.Assets.src.LevelObjects;
using RollABall.Assets.src.Managers;
using RollABall.Assets.src.UI;

namespace RollABall.Assets.src.Player
{
    /// <summary>
    /// Keeps track of and/or centralizes player stuff for encapsulation's sake.
    /// </summary>
    internal partial class PlayerManager : Node
    {
        #region Refs
        public Logger log;

        GameManager gm = GameManager.Instance;

        [Export] public PlayerController controller;
        [Export] RigidBody3D ball;
        public RigidBody3D Ball { get => ball; set => ball = value; }
        #endregion
        #region Stats
        public int Lives { get => lives; set => lives = value; }
        private int lives = 3;
        #endregion

        public override void _Ready()
        {
            log = new Logger(true, true, "logs\\", "playerLog", "txt", false);
        }

        /// <summary>
        /// Kills the player
        /// </summary>
        public void OnDamage()
        {
            log.WriteAll($"Player died. Player has {Lives} lives left.");
            Lives--;
            gm.UIManager.HUD.Update();
            if (Lives >= 0) { gm.LevelManager.Reload(); }
            else { Lives = 3; GameManager.Instance.UIManager.HUD.Update(); gm.UIManager.State = UIState.LevelFailure; }
        }

        public void OnLoadCheckpoint(Checkpoint checkpoint)
        {
            controller.ResetTF(checkpoint);
        }
    }
}
