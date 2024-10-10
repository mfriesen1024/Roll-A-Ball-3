using Godot;
using KeystoneUtils.Logging;

namespace RollABall.Assets.src.Player
{
    /// <summary>
    /// Responsible for moving the player.
    /// </summary>
    internal partial class PlayerController : Node3D
    {
        #region refs
        Logger playerLog;

        [Export] RigidBody3D ball;
        [Export] PlayerCam cam;
        Node3D camParent;
        #endregion

        #region speeds
        [Export] float maxMoveSpeed = 10;
        [Export] float moveLerpMod = 1;
        [Export] float mouseSensitivityMod = 1f;
        #endregion

        #region moveFields
        Vector3 moveVector3D = Vector3.Zero;
        Vector2 lookVector = Vector2.Zero;
        #endregion

        public override void _Ready()
        {
            playerLog = new Logger(true, true, "logs\\", "playerLog", "txt", false);

            // Set refs.
            camParent = (Node3D)cam.GetParent();
        }

        public override void _PhysicsProcess(double delta)
        {
            SetBallVelocity();
            SetLookRotations();
            UpdateOurPosition();

            void SetBallVelocity()
            {
                Vector3 moveVelocityGoal = moveVector3D * maxMoveSpeed;
                Vector3 newVelocity = ball.LinearVelocity.Lerp(moveVelocityGoal, (float)delta * moveLerpMod);
                ball.LinearVelocity = newVelocity;
            }

            void SetLookRotations()
            {
                // Clamp X rot so the player can't do silly things with it.
                float oldX = camParent.Basis.GetEuler().X;
                float xDelta = -(float)delta * lookVector.Y * mouseSensitivityMod;
                playerLog.Write($"Trying to look up/down, old is {oldX}, delta is {xDelta}");
                float newX = oldX + xDelta;
                float toRadians = Mathf.Pi / 180;
                newX = Mathf.Clamp(newX, -75 * toRadians, 15 * toRadians);
                // Reassign xDelta based on our clamping.
                xDelta = newX - oldX;

                // Now execute the rotations
                camParent.RotateX(xDelta);
                RotateY(-(float)delta * lookVector.X * mouseSensitivityMod);

                lookVector = Vector2.Zero; // Apparently we dont get an event when relative is zero.
            }

            void UpdateOurPosition()
            {
                Position = ball.Position;
            }
        }

        /// <summary>
        /// Move the player.
        /// </summary>
        public void OnMove(Vector2 direction)
        {
            direction = direction.Normalized();

            // Compose the move direction.
            Vector2 moveVector2D = direction.Y * cam.Get2DForward();
            moveVector2D += direction.X * cam.Get2DRight();
            moveVector3D = new(moveVector2D.X, 0, moveVector2D.Y);
        }

        public void OnLook(Vector2 direction)
        {
            lookVector = direction;
        }
    }
}
