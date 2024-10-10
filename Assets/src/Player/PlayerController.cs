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

        #region speeds
        [Export] float maxMoveSpeed = 10;
        [Export] float moveLerpMod = 1;
        [Export] float mouseSensitivityMod = 1;
        #endregion

        #region moveFields
        Vector3 moveVector3D = Vector3.Zero;
        #endregion

        public override void _PhysicsProcess(double delta)
        {
            Vector3 moveVelocityGoal = moveVector3D * maxMoveSpeed;
            Vector3 newVelocity = ball.LinearVelocity.Lerp(moveVelocityGoal, (float)delta*moveLerpMod);
        }

        /// <summary>
        /// Move the player.
        /// </summary>
        public void OnMove(Vector2 direction)
        {
            direction = direction.Normalized();

            // Compose the move direction.
            Vector2 moveVector2D = direction.Y*cam.Get2DForward();
            moveVector2D += direction.X * cam.Get2DRight();
            moveVector3D = new(moveVector2D.X,0,moveVector2D.Y);
        }

        public void OnLook(Vector2 direction)
        {

        }

        void UpdateOurPosition()
        {
            Position=ball.Position;
        }
    }
}
