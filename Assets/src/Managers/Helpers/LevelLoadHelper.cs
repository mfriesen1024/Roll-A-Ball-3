using Godot;
using KeystoneUtils.Logging;
using RollABall.Assets.src.LevelObjects;
using System;
using System.Threading.Tasks;

namespace RollABall.Assets.src.Managers.Helpers
{
    /// <summary>
    /// Loads levels for the levelmanager.
    /// </summary>
    internal class LevelLoadHelper
    {
        /// <summary>
        /// The number of load tasks we have left.
        /// </summary>
        public static int activeLoadTasks { get; private set; } = 0;
        /// <summary>
        /// The number of load tasks since we last finished all operations.
        /// </summary>
        public static int totalLoadTasks { get; private set; } = 0;
        static Logger log = new Logger(true, true, "logs\\", "levelLoader", "txt", true);

        /// <summary>
        /// Loaded level asynchronously... at least, before I disabled it due to being too lazy to learn Godot multithreading.
        /// </summary>
        /// <param name="levelScene">The scene to be loaded</param>
        /// <param name="callback">What should I do once its loaded?</param>
        public void LoadLevelAsync(PackedScene levelScene, Action<Level> callback)
        {
            // Disabled async loading for now. We have a loading screen anyway.
            Task.Run(() =>
            {
                //Load(levelScene, callback);
            });

            Load(levelScene, callback);

            void Load(PackedScene levelScene, Action<Level> callback)
            {
                // Update counter, in case we ever need it for load screens.
                IncrementTasks();

                // Load the level.
                Level level = (Level)levelScene.Instantiate();
                callback(level);

                // Update counter again.
                DecrementTaskCount();
            }
        }

        private void IncrementTasks()
        {
            activeLoadTasks++;
            totalLoadTasks++;
            log.WriteAll($"Starting a loadtask, this is task {activeLoadTasks}");
        }

        private void DecrementTaskCount()
        {
            activeLoadTasks--;
            log.WriteAll($"Ended a loadtask, completing {totalLoadTasks - activeLoadTasks} of {totalLoadTasks}.");
            if (activeLoadTasks == 0) { totalLoadTasks = 0; }
        }
    }
}
