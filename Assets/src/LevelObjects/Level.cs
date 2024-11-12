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
        // I really dont remember why these exist.
        public EndLevelTrigger[] triggers = [];
        public Checkpoint[] checkpoints = [];
        public MovingPlatform[] movingPlatforms = [];

        public override void _EnterTree()
        {
            SetRefs();
            SetCheckpointIndices();

            // Helper method for array stuff.
            void SetRefs()
            {
                // Create list instances so we can add things without index iteration. I'm lazy, get over it.
                var elts = triggers.ToList();
                var cps = checkpoints.ToList();
                var mps = movingPlatforms.ToList();

                // For every child object...
                foreach (var c in FindChildren("*"))
                {
                    // Debug log its type
                    Logger.StaticLogger.WriteAll($"Node {c.Name} was detected. It has type {c.GetType()}.");

                    if (c.Name == "ELT") { Logger.StaticLogger.WriteAll($"Found ELT, is it real? {c is EndLevelTrigger}."); }

                    // If it matches a type we want to track, add it to the respective list.
                    if (c is EndLevelTrigger) { elts.Add(c as EndLevelTrigger); }
                    else if (c is Checkpoint) { cps.Add(c as Checkpoint); }
                    else if (c is MovingPlatform) { mps.Add(c as MovingPlatform); }
                    else { Logger.StaticLogger.Write($"Node {c.Name} was not registered. It has type {c.GetType()}"); }
                }

                // Update our lists.
                triggers = elts.ToArray();
                checkpoints = cps.ToArray();
                movingPlatforms = mps.ToArray();
            }

            // Helper method to set checkpoint indices so we can spawn at them, and save that spawn point.
            void SetCheckpointIndices()
            {
                for (int i = 0; i < checkpoints.Length; i++)
                {
                    Checkpoint cp = checkpoints[i];
                    cp.index = i;
                }
            }
        }
    }
}
