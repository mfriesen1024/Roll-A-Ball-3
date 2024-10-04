using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.Player
{
    internal partial class PlayerController:CharacterBody3D
    {
        #region Refs
        [Export]Camera3D cam;
        #endregion
        #region Movement
        // Get the gravity from the project settings to be synced with RigidBody nodes.
        // This should eventually be moved to utils.
        float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

        [Export] float tempMouseSensitivity = 1, msDiv = 140;
        float speed = 5, jumpVelocity = 8, velocityLerpFactor = 0.75f;
        #endregion
        public override void _Process(double delta)
        {
            if (Input.IsActionJustPressed("pause")) { Input.MouseMode = Input.MouseModeEnum.Visible; }
        }

        public override void _PhysicsProcess(double delta)
        {
            UpdateMovement(delta);
        }

        private void UpdateMovement(double delta)
        {
            Vector3 velocity = Velocity;

            // Add the gravity.
            if (!IsOnFloor())
                velocity.Y -= gravity * (float)delta;

            // TODO: As good practice, you should replace UI actions with custom gameplay actions.
            // Handle Jump.
            if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
                velocity.Y = jumpVelocity;

            // Get the input direction and handle the movement/deceleration.
            Vector2 inputDir = Input.GetVector("moveLeft", "moveRight", "moveUp", "moveDown");
            Vector3 direction = ((cam.GetParent() as Node3D).Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
            if (direction != Vector3.Zero)
            {
                Vector3 newVelocity = velocity;
                velocity.X = direction.X * speed;
                velocity.Z = direction.Z * speed;
                newVelocity.X = Mathf.Lerp(velocity.X, newVelocity.X, velocityLerpFactor);
                newVelocity.Z = Mathf.Lerp(velocity.Z, newVelocity.Z, velocityLerpFactor);
                velocity = newVelocity;
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
                velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
            }

            Velocity = velocity;
            MoveAndSlide();
        }

        public override void _UnhandledInput(InputEvent inputEvent)
        {
            // Mouse movement, should probably move this to inputman when we make it.
            if (inputEvent is InputEventMouseMotion)
            {
                RotatePlayerAndCam(((InputEventMouseMotion)inputEvent).Relative);
            }
        }

        /// <summary>
        /// Rotates the camera's X rot and player's Y rot based on lookVector.
        /// </summary>
        /// <param name="lookVector">The 2d vector to be used for rotations, modified by sensitivity modifiers.</param>
        void RotatePlayerAndCam(Vector2 lookVector)
        {
            if (!GetTree().Paused)
            {
                Node3D camParent = (Node3D)cam.GetParent();
                lookVector *= (tempMouseSensitivity / msDiv);

                // Rotate the player obj.
                camParent.RotateY(-lookVector.X);

                // Rotate camera.
                cam.RotateX(-lookVector.Y);

                // Manual clamping because mathf is weird.
                Vector3 rot = cam.Rotation;
                if (rot.X > Mathf.Pi / 2)
                {
                    rot.X = Mathf.Pi / 2;
                }
                if (rot.X < -Mathf.Pi / 2)
                {
                    rot.X = -Mathf.Pi / 2;
                }
                cam.Rotation = rot;
            }
        }
    }
}
