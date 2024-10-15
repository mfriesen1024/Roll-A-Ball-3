using Godot;
using RollABall.Assets.src.Player;

namespace RollABall.Assets.src.UI
{
    internal partial class HUD : Control
    {
        #region refs
        [Export] Label lives, time, score;
        #endregion

        public override void _Ready()
        {
            // TODO: sort out first update shenaniganry.
        }
        // Call this to force player updates.
        public void Update()
        {
            lives.Text = $"x{PlayerManager.Instance.Lives}";
        }
    }
}
