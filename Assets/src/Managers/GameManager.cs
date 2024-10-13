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
        /// <summary>
        /// Called after the ready callback, intended to allow managers to get references to each other after initialization finishes.
        /// </summary>
        public static Action postInit = delegate { };
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static GameManager Instance { get; private set; }

        #region managerRefs
        public static InputManager InputManager { get; private set; }
        [Export] PackedScene inputManPrefab;
        public static PlayerManager PlayerManager { get; private set; }
        [Export] PackedScene playerManPrefab;
        public static UIManager UIManager { get; private set; }
        [Export] PackedScene uiManPrefab;
        #endregion

        public override void _Ready()
        {
            SetGMSingleton();
            SetOtherSingletons();

            postInit();
        }

        private void SetOtherSingletons()
        {
            // Check other managers for their singleton instances, and if they don't exist, create them.
            if (PlayerManager.Instance != null) { PlayerManager = PlayerManager.Instance; }
            else { PlayerManager = playerManPrefab.Instantiate() as PlayerManager; AddChild(PlayerManager); }
            if (InputManager.Instance != null) { InputManager = InputManager.Instance; }
            else { InputManager = inputManPrefab.Instantiate() as InputManager; AddChild(InputManager); }
            if (UIManager.Instance != null) { UIManager = uiManPrefab.Instantiate() as UIManager; AddChild(UIManager); }
        }

        private void SetGMSingleton()
        {
            if (Instance == null) { Instance = this; }
            else { QueueFree(); return; }
        }

        /// <summary>
        /// We'll use this to start a shutdown sequence, allowing for persistent objects to be saved.
        /// </summary>
        internal void StartQuit()
        {
            throw new NotImplementedException();
        }
    }

    public enum GameState
    {
        MenuOnly,
        Gameplay,
        Pause
    }
}
