using Godot;

namespace RollABall.Assets.src.Player
{
    /// <summary>
    /// Responsible for moving the player.
    /// </summary>
    internal partial class PlayerController : Node3D
    {
        #region refs
        [Export] RigidBody3D ball;
        [Export] PlayerCam cam;
        #endregion

        public override void _Process(double delta)
        {
            
        }

        /// <summary>
        /// Move the player.
        /// </summary>
        public void Move(Vector2 direction)
        {
            direction = direction.Normalized();

            // Compose the move direction.
            Vector2 moveVector = direction.Y*cam.Get2DForward();
            moveVector += direction.X * cam.Get2DRight();
        }

        public void Look(Vector2 direction)
        {

        }
    }
}
