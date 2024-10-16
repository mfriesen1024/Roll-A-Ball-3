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
        public static int activeLoadTasks { get; private set; } = 0;
        public static int totalLoadTasks { get; private set; } = 0;
        static Logger log = new Logger(true, true, "logs\\", "levelLoader", "txt", true);

        public void LoadLevelAsync(PackedScene levelScene, Action<Level> callback)
        {
            Task.Run(() =>
            {
                // Loading screen crap.
                IncrementTasks();

                // Load the level.
                Level level = levelScene.Instantiate() as Level;
                callback(level);

                // Loading screen end stuff.
                DecrementTaskCount();
            });
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
            log.WriteAll($"Ended a loadtask, completing {totalLoadTasks-activeLoadTasks} of {totalLoadTasks}.");
            if (activeLoadTasks == 0) { totalLoadTasks = 0; }
        }
    }
}
