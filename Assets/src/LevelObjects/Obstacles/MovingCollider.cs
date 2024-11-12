using Godot;
using KeystoneUtils.Logging;

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
        private float lerpFactor = 0;
        private bool returning = false;

        public override void _PhysicsProcess(double delta)
        {
            UpdateMoveFields(delta);
            Move(Unlocked, lerpFactor, delta);
        }

        /// <summary>
        /// Called every physics tick to move the object.
        /// </summary>
        /// <param name="unlocked">Whether or not the object is locked.</param>
        /// <param name="lerpFactor">The factor to be used for lerping its position.</param>
        protected virtual void Move(bool unlocked, float lerpFactor, double delta)
        {
            // Only run if unlocked.
            if (unlocked)
            {
                Logger.StaticLogger.Write($"2 {Name}: Old Position was {Position}.");
                Position = Start.Lerp(End, lerpFactor);
                Logger.StaticLogger.Write($"3 {Name}:New Position is {Position}.");
            }
        }

        private void UpdateMoveFields(double delta)
        {
            float deltaButFloat = (float)delta;
            // Set new lerp factor.
            float scaledDelta = deltaButFloat * speed; // This is constant, so its correct.
            float oldLerpFactor = lerpFactor; // For logging purposes.
            if (returning) { lerpFactor-=scaledDelta; }
            else { lerpFactor+=scaledDelta; }
            lerpFactor = Mathf.Clamp(lerpFactor, 0, 1);

            // Set returning bool.
            if (lerpFactor == 1) { returning = true; }
            if (lerpFactor == 0) { returning = false; }

            Logger.StaticLogger.Write($"1 {Name}: Delta is {delta}, Speed is {speed}, Scaled delta is {scaledDelta}, Old lerp is {oldLerpFactor}, " +
                $"New lerp is {lerpFactor} and Return state is {returning}.");
        }
    }
}
