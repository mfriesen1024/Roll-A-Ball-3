using Godot;
using Godot.Collections;
using RollABall.Assets.src.LevelObjects;
using System;

namespace RollABall.Assets.src.Player
{
    /// <summary>
    /// Responsible for moving the player.
    /// </summary>
    internal partial class PlayerController : Node3D
    {
        #region refs
        [Export] PlayerManager playerManager;
        [Export] RigidBody3D ball;
        [Export] PlayerCam cam;
        [Export] Area3D groundCheck;
        Node3D camParent;
        #endregion
        #region speeds
        [Export] float maxMoveSpeed = 10;
        [Export] float moveLerpMod = 1;
        [Export] float mouseSensitivityMod = 1f;
        [Export] float jumpForce = 75;
        #endregion
        #region moveFields
        private float groundCheckDistance = 1.2f;
        Vector3 moveVector3D = Vector3.Zero;
        Vector2 lookVector = Vector2.Zero;

        bool grounded;
        bool shouldJump;
        #endregion

        public override void _Ready()
        {
            // Set refs.
            camParent = (Node3D)cam.GetParent();
        }

        public override void _PhysicsProcess(double delta)
        {
            SetBallVelocity();
            SetLookRotations();
            UpdateOurPosition();
            GroundCheck();

            void SetBallVelocity()
            {
                Vector3 moveVelocityGoal = moveVector3D * maxMoveSpeed;
                Vector3 newVelocity = ball.LinearVelocity.Lerp(moveVelocityGoal, (float)delta * moveLerpMod);
                ball.LinearVelocity = newVelocity; // TODO: use a different velocity set method because threads.
                if(shouldJump) { ball.ApplyCentralForce(Vector3.Up*jumpForce); }
            }

            void SetLookRotations()
            {
                // Clamp X rot so the player can't do silly things with it.
                float oldX = camParent.Basis.GetEuler().X;
                float xDelta = -(float)delta * lookVector.Y * mouseSensitivityMod;
                playerManager.log.Write($"Trying to look up/down, old is {oldX}, delta is {xDelta}");
                float newX = oldX + xDelta;
                float toRadians = Mathf.Pi / 180;
                newX = Mathf.Clamp(newX, -74 * toRadians, 14 * toRadians);
                // Reassign xDelta based on our clamping.
                xDelta = newX - oldX;

                // Now execute the rotations
                camParent.RotateX(xDelta);
                RotateY(-(float)delta * lookVector.X * mouseSensitivityMod);

                lookVector = Vector2.Zero; // Apparently we dont get an event when relative is zero.
            }

            void GroundCheck()
            {
                var objects = groundCheck.GetOverlappingBodies();

                bool grounded = false;

                foreach(Node3D node in objects)
                {
                    if (node is StaticBody3D)
                    {
                        grounded = true;
                    }
                }

                if (!grounded) { shouldJump = false; }
                this.grounded = grounded;
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
            playerManager.log.Write($"Cam forward is {cam.Get2DForward()}, cam right is {cam.Get2DRight()}");
            moveVector3D = new(moveVector2D.X, 0, moveVector2D.Y);
        }

        public void OnLook(Vector2 direction)
        {
            lookVector = direction;
        }

        internal void OnJump()
        {
            shouldJump = grounded;
        }

        internal void ResetTF(Checkpoint checkpoint)
        {
            ball.Position = checkpoint.Position + checkpoint.offset;
            ball.LinearVelocity = Vector3.Zero;
            ball.AngularVelocity = Vector3.Zero;
            camParent.Basis = Basis.Identity;
            Basis = Basis.Identity;
        }
    }
}
