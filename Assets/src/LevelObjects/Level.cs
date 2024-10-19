using Godot;
using KeystoneUtils.Logging;
using RollABall.Assets.src.LevelObjects.Obstacles;
using System.Linq;

namespace RollABall.Assets.src.LevelObjects
{
    /// <summary>
    /// Manages all objects within a given level.
    /// </summary>
    internal partial class Level : Node3D
    {
        // Initialize with empty collection so we can .tolist later.
        public EndLevelTrigger[] triggers = [];
        public Checkpoint[] checkpoints = [];
        public MovingPlatform[] movingPlatforms = [];

        public override void _EnterTree()
        {
            SetRefs();

            void SetRefs()
            {
                var elts = triggers.ToList();
                var cps = checkpoints.ToList();
                var mps = movingPlatforms.ToList();

                foreach (var c in FindChildren("*"))
                {
                    Logger.StaticLogger.Write($"Node {c.Name} was detected. It has type {c.GetType()}.");
                    if (c is EndLevelTrigger) { elts.Add(c as EndLevelTrigger); }
                    else if (c is Checkpoint) { cps.Add(c as Checkpoint); }
                    else if (c is MovingPlatform) { mps.Add(c as MovingPlatform); }
                    else { Logger.StaticLogger.Write($"Node {c.Name} was not registered. It has type {c.GetType()}", LogLevel.info); }
                }

                triggers = elts.ToArray();
                checkpoints = cps.ToArray();
                movingPlatforms = mps.ToArray();
            }
        }
    }
}
