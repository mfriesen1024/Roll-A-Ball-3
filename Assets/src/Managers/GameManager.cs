using Godot;
using KeystoneUtils.Logging;
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
        /// <summary>
        /// State of the game.
        /// </summary>
        public GameState State { get => state; set { OnSetState(value); state=value;  } }

        private GameState state;

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

        void OnSetState(GameState state)
        {
            // This should do things when state is set, which should only be done by the UIMan.
            switch (state)
            {
                case GameState.MenuOnly:
                    Input.MouseMode = Input.MouseModeEnum.Visible;
                    // unpause.
                    break;
                case GameState.Pause: 
                    Input.MouseMode = Input.MouseModeEnum.Visible;
                    // pause specific things here.
                    break;
                case GameState.Gameplay:
                    Input.MouseMode = Input.MouseModeEnum.Captured;
                    // unpause.
                    break;
                default:
                    string s = $"State {state} is not implemented in GM! This bad!";
                    Logger.StaticLogger.Write(s,LogLevel.error);
                    throw new NotImplementedException(s);
            }
        }

        #region helperMethods
        private void SetOtherSingletons()
        {
            // Check other managers for their singleton instances, and if they don't exist, create them.
            if (PlayerManager.Instance != null) { PlayerManager = PlayerManager.Instance; }
            else { PlayerManager = playerManPrefab.Instantiate() as PlayerManager; AddChild(PlayerManager); }
            if (InputManager.Instance != null) { InputManager = InputManager.Instance; }
            else { InputManager = inputManPrefab.Instantiate() as InputManager; AddChild(InputManager); }
            if (UIManager.Instance != null) { UIManager = UIManager.Instance; }
            else { UIManager = uiManPrefab.Instantiate() as UIManager; AddChild(UIManager); }
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
        #endregion
    }

    public enum GameState
    {
        MenuOnly,
        Gameplay,
        Pause
    }
}
