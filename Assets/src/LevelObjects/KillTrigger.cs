using Godot;
using KeystoneUtils.Logging;
using RollABall.Assets.src.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.LevelObjects
{
    /// <summary>
    /// Damages the player on contact
    /// </summary>
    internal partial class KillTrigger:Area3D
    {
        public override void _Ready()
        {
            AreaEntered += OnTriggerEnter;
            BodyEntered += OnTriggerEnter;
        }

        private void OnTriggerEnter(Node3D body)
        {
            if (body is RigidBody3D rb)
            {
                if(rb == PlayerManager.Instance.Ball) { PlayerManager.Instance.OnDamage(); }
            }
            Logger.StaticLogger.Write($"Trigger was entered by {body.Name}, it has type {body.GetType()}");
        }
    }
}
