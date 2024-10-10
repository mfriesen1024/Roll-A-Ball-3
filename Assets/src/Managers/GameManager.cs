using Godot;
using RollABall.Assets.src.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.Managers
{
    internal partial class GameManager : Node
    {
        public static GameManager Instance { get; private set; }

        #region managerRefs
        public static InputManager InputManager { get; private set; }
        [Export] PackedScene inputManPrefab;
        public static PlayerManager PlayerManager { get; private set; }
        [Export] PackedScene playerManPrefab;
        #endregion

        public override void _Ready()
        {
            SetGMSingleton();
            SetOtherSingletons();
        }

        private void SetOtherSingletons()
        {
            // Check other managers for their singleton instances, and if they don't exist, create them.
            if (PlayerManager.Instance != null) { PlayerManager = PlayerManager.Instance; }
            else { PlayerManager = playerManPrefab.Instantiate() as PlayerManager; AddChild(PlayerManager); }
            if (InputManager.Instance != null) { InputManager = InputManager.Instance; }
            else { InputManager = inputManPrefab.Instantiate() as InputManager; AddChild(InputManager); }
        }

        private void SetGMSingleton()
        {
            if (Instance == null) { Instance = this; }
            else { QueueFree(); }
        }
    }
}
