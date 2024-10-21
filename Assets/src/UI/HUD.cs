using Godot;
using RollABall.Assets.src.Managers;
using RollABall.Assets.src.Player;
using System;

namespace RollABall.Assets.src.UI
{
    internal partial class HUD : Control
    {
        #region refs
        internal static HUD Instance { get; private set; }
        [Export] Label lives, time, score;
        #endregion

        public override void _Ready()
        {
            // Set singleton
            if (Instance == null) { Instance = this; }
            else { QueueFree(); return; }

            // TODO: sort out first update shenaniganry.
            Update();
        }
        public override void _Process(double delta)
        {
            DateTime timerTime = LevelManager.Instance.Timer;
            time.Text = $"{timerTime.Hour}:{timerTime.Minute}:{timerTime.Second}:{timerTime.Millisecond}";
        }
        // Call this to force player updates.
        public void Update()
        {
            lives.Text = $"x{PlayerManager.Instance.Lives}";            
        }
    }
}
