﻿using Godot;
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
        #region Refs
        /// <summary>
        /// Singleton instance of the levelman.
        /// </summary>
        public static LevelManager Instance { get; private set; }

        LevelLoadHelper loadHelper = new();

        [Export] PackedScene[] levels;
        [Export] Texture2D[] levelTextures;

        public Level activeLevel { get; private set; }
        #endregion
        #region updateRefs
        public DateTime startTime { get; private set; }
        public DateTime Timer { get { return GetTimerTime(); } }

        #endregion

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
            // If state isn't loading, set that now.
            if (UIManager.Instance.State != UIState.Loading) { UIManager.Instance.State = UIState.Loading; }

            // Set the load helper to load the level and set it's parent async.
            loadHelper.LoadLevelAsync(levels[LevelIndex], Assign);
        }

        private void Assign(Level level)
        {
            activeLevel = level;
            level.ProcessMode = ProcessModeEnum.Pausable;
            level.TreeEntered += OnLoadFinish;
            CallDeferred("add_child", level);
        }

        private void OnLoadFinish()
        {
            // Unsubscribe because performance.
            activeLevel.TreeEntered -= OnLoadFinish;

            // Set the player's position to checkpoint position.
            Checkpoint checkpoint = activeLevel.checkpoints[CheckpointIndex];
            PlayerManager.Instance.OnLoadCheckpoint(checkpoint);

            // Tell the UIMan to deactivate loading screen.
            UIManager.Instance.State = UIState.HUD;

            // Set the ELTs to end the level.
            foreach (EndLevelTrigger elt in activeLevel.triggers) { elt.BodyEntered += ELTCheck; }

            // Set the checkpoints to set our local checkpoint.
            foreach (Checkpoint cp in activeLevel.checkpoints)
            {
                cp.BodyEntered += (Node3D other) =>
                {
                    if (PlayerManager.Instance.Ball.Equals(other))
                    {
                        CheckpointIndex = cp.index;
                    }
                };
            }

            startTime = DateTime.Now;
        }

        /// <summary>
        /// Discards the current level, if it exists.
        /// </summary>
        public void Discard()
        {
            if (activeLevel != null)
            {
                activeLevel.QueueFree();
                activeLevel = null;
            }
        }
        #endregion

        /// <summary>
        /// Checks if the object is the player ball, and if so, triggers a level win.
        /// </summary>
        private void ELTCheck(Node3D other)
        {
            if (PlayerManager.Instance.Ball.Equals(other))
            {
                CheckpointIndex = 0;

                UIManager.Instance.State = UIState.LevelComplete;

                var ex = new NotImplementedException("Aaa! Saving times/scores not implemented!");
                Logger.StaticLogger.WriteAll($"{ex.Message}", LogLevel.warn);
                throw ex;
            }
        }

        internal void Reload()
        {
            UIManager.Instance.State = UIState.Loading;
            Discard();
            Load();
        }

        internal void Pause()
        {
            try
            {
                GetTree().Paused = true;
            }
            catch (NullReferenceException ignored) { }
        }

        internal void Unpause()
        {
            try
            {
                GetTree().Paused = false;
            }
            catch (NullReferenceException ignored) { }
        }

        private DateTime GetTimerTime()
        {
            return new DateTime(DateTime.Now.Ticks-startTime.Ticks);
        }
    }
}
