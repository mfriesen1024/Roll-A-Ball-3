using Godot;
using KeystoneUtils.Logging;
using RollABall.Assets.src.LevelObjects;
using RollABall.Assets.src.Managers.Helpers;
using RollABall.Assets.src.Player;
using System;

namespace RollABall.Assets.src.Managers
{
    /// <summary>
    /// Manages levels, loading, and ui crap.
    /// </summary>
    internal partial class LevelManager : Node
    {
        /// <summary>
        /// Singleton instance of the levelman.
        /// </summary>
        public static LevelManager Instance { get; private set; }

        LevelLoadHelper loadHelper = new();

        [Export] PackedScene[] levels;
        [Export] Texture2D[] levelTextures;

        public Level activeLevel { get; private set; }

        /// <summary>
        /// Used to set the level to be loaded on a load request.
        /// </summary>
        public int LevelIndex = 0;
        public int CheckpointIndex = 0;

        public override void _Ready()
        {
            base._Ready();

            // Set singleton
            if (Instance == null) { Instance = this; }
            else { QueueFree(); return; }
        }

        #region loading bunk
        /// <summary>
        /// Loads the level with index of LevelIndex, and spawns the player at the checkpoint with CheckpointIndex.
        /// </summary>
        public void Load()
        {
            // Set the load helper to load the level and set it's parent async.
            loadHelper.LoadLevelAsync(levels[LevelIndex], (Level level) => { activeLevel = level; AddChild(level); });
        }

        private void Assign(Level level)
        {
            activeLevel = level;
            level.TreeEntered += OnLoadFinish;
            AddChild(level);
        }

        private void OnLoadFinish()
        {
            // Unsubscribe because performance.
            activeLevel.TreeEntered -= OnLoadFinish;

            // Set the player's position to checkpoint position.
            Checkpoint checkpoint = activeLevel.checkpoints[CheckpointIndex];
            PlayerManager.Instance.Ball.Position = checkpoint.Position + checkpoint.offset;
            PlayerManager.Instance.Ball.LinearVelocity = Vector3.Zero;

            // Set the ELTs to end the level.
            foreach (EndLevelTrigger elt in activeLevel.triggers) { elt.BodyEntered += ELTCheck; }
        }

        /// <summary>
        /// Discards the current level.
        /// </summary>
        public void Discard()
        {
            activeLevel.QueueFree();
            activeLevel = null;
        }
        #endregion

        /// <summary>
        /// Checks if the object is the player ball, and if so, triggers a level win.
        /// </summary>
        private void ELTCheck(Node3D other)
        {
            if (PlayerManager.Instance.Equals(other))
            {
                CheckpointIndex = 0;
                Discard();

                var ex = new NotImplementedException("Aaa! Saving times/scores not implemented!");
                Logger.StaticLogger.Write($"{ex.Message}");
                throw ex;
            }
        }
    }
}
