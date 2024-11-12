using Godot;
using KeystoneUtils.Logging;
using RollABall.Assets.src.Managers;
using RollABall.Assets.src.Player;

namespace RollABall.Assets.src.LevelObjects
{
    /// <summary>
    /// Damages the player on contact
    /// </summary>
    internal partial class KillTrigger : Area3D
    {
        public override void _Ready()
        {
            AreaEntered += OnTriggerEnter;
            BodyEntered += OnTriggerEnter;
        }

        private void OnTriggerEnter(Node3D other)
        {
            if (other is RigidBody3D rb)
            {
                PlayerManager playerManager = GameManager.Instance.PlayerManager;
                if (rb == playerManager.Ball) { playerManager.OnDamage(); }
            }
            Logger.StaticLogger.Write($"Trigger was entered by {other.Name}, it has type {other.GetType()}");
        }
    }
}
