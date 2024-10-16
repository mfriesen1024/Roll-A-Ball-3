using Godot;

namespace RollABall.Assets.src.LevelObjects.Obstacles
{
    /// <summary>
    /// Used to handle movement stuff for any object that needs to move within a level.
    /// </summary>
    internal partial class MovingCollider : AnimatableBody3D
    {
        /// <summary>
        /// Start position.
        /// </summary>
        [Export] protected Vector3 Start = Vector3.Forward;
        /// <summary>
        /// End position.
        /// </summary>
        [Export] protected Vector3 End = Vector3.Zero;
        /// <summary>
        /// Time in seconds it should take to go from start to end.
        /// </summary>
        [Export] float speed = 1;
        /// <summary>
        /// Whether or not the object should run.
        /// </summary>
        [Export] public bool Unlocked = true;

        // Protected refs for movement stuff.
        private float lerpFactor { get; set; } = 0;
        private bool returning { get; set; } = false;

        public override void _PhysicsProcess(double delta)
        {
            UpdateMoveFields(delta);
            Move(Unlocked, lerpFactor);
        }

        /// <summary>
        /// Called every physics tick to move the object.
        /// </summary>
        /// <param name="unlocked">Whether or not the object is locked.</param>
        /// <param name="lerpFactor">The factor to be used for lerping its position.</param>
        protected virtual void Move(bool unlocked, float lerpFactor)
        {
            // Only run if unlocked.
            if (unlocked)
            {
                Position = Start.Lerp(End,lerpFactor);
            }
        }

        private void UpdateMoveFields(double delta)
        {
            float newDelta = (float)delta;
            // Set new lerp factor.
            if (returning) { lerpFactor -= newDelta / speed; }
            else { lerpFactor += newDelta / speed; }
            lerpFactor = Mathf.Clamp(newDelta, 0, 1);

            // Set returning bool.
            if(lerpFactor == 1) { returning = true; }
            if(lerpFactor == 0) { returning = false; }
        }
    }
}
