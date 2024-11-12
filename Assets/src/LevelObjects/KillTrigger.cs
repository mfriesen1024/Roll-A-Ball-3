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
            AreaEntered += OnTriggerEnter; // Idk why this is here.
            BodyEntered += OnTriggerEnter;
        }

        private void OnTriggerEnter(Node3D other)
        {
            // If the object is a rigidbody
            if (other is RigidBody3D rb)
            {
                // Check if rb and the player ball are the same thing, and if so, damage it.
                PlayerManager playerManager = GameManager.Instance.PlayerManager;
                if (rb == playerManager.Ball) { playerManager.OnDamage(); }
            }
            Logger.StaticLogger.Write($"Trigger was entered by {other.Name}, it has type {other.GetType()}");
        }
    }
}
