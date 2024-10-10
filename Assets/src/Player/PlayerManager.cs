using Godot;
using RollABall.Assets.src.Managers;

namespace RollABall.Assets.src.Player
{
    /// <summary>
    /// Keeps track of and/or centralizes player stuff for encapsulation's sake.
    /// </summary>
    internal partial class PlayerManager : Node
    {
        #region Refs
        public PlayerManager Instance { get; private set; }

        [Export] PlayerController controller;
        [Export] RigidBody3D ball;
        [Export] PlayerCam cam;
        #endregion

        public override void _Ready()
        {
            if (Instance == null) { Instance = this; }
            else { QueueFree(); }
        }
    }
}
