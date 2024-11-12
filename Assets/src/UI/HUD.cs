using Godot;
using RollABall.Assets.src.Managers;
using System;

namespace RollABall.Assets.src.UI
{
    internal partial class HUD : Control
    {
        #region refs
        GameManager gm;
        internal static HUD Instance { get; private set; }
        [Export] Label lives, time, score;
        #endregion

        public override void _Ready()
        {
            // Set singleton
            if (Instance == null) { Instance = this; }
            else { QueueFree(); return; }

            gm = GameManager.Instance;

            // TODO: sort out first update shenaniganry.
            Update();
        }
        public override void _Process(double delta)
        {
            DateTime timerTime = gm.LevelManager.Timer;
            time.Text = $"{ForceStringLength(timerTime.Hour)}:" +
                $"{ForceStringLength(timerTime.Minute)}:" +
                $"{ForceStringLength(timerTime.Second)}:" +
                $"{ForceStringLength(timerTime.Millisecond)}";
        }
        // Call this to force player updates.
        public void Update()
        {
            lives.Text = $"x{gm.PlayerManager.Lives}";
        }

        private string ForceStringLength(object o, int length = 2)
        {
            string s = o.ToString();
            while (s.Length < length)
            {
                s = "0" + s;
            }
            return s;
        }
    }
}
