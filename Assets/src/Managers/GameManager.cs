using Godot;
using KeystoneUtils.Logging;
using RollABall.Assets.src.Player;
using System;

namespace RollABall.Assets.src.Managers
{
    internal partial class GameManager : Node
    {
        /// <summary>
        /// Called after the ready callback, intended to allow managers to get references to each other after initialization finishes.
        /// </summary>
        public static Action PostInit = delegate { };
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static GameManager Instance { get; private set; }
        /// <summary>
        /// State of the game.
        /// </summary>
        public GameState State { get => state; set { OnSetState(value); state = value; } }

        private GameState state;

        #region managerRefs
        public InputManager InputManager { get; private set; }
        [Export] PackedScene inputManPrefab;
        public PlayerManager PlayerManager { get; private set; }
        [Export] PackedScene playerManPrefab;
        public UIManager UIManager { get; private set; }
        [Export] PackedScene uiManPrefab;
        public LevelManager LevelManager { get; private set; }
        [Export] PackedScene levelManPrefab;
        public DataManager DataManager { get; private set; } = new();
        #endregion

        public override void _Ready()
        {
            // Set references.
            SetGMSingleton();
            SetOtherManagers();

            // Now start initializing things
            DataManager.OnStartup();

            Init();

            // Now run any post init operations.
            if (Instance == this) { PostInit(); }

            // Now that we're initialized, we'll handle our own quit requests.
            GetTree().AutoAcceptQuit = false;
        }

        public override void _Notification(int what)
        {
            // So far this is just to handle our own close requests to ensure we save things.
            if (what == NotificationWMCloseRequest) { try { StartQuit(); } catch { } }
        }

        void OnSetState(GameState state)
        {
            // This should do things when state is set, which should only be done by the UIMan.
            switch (state)
            {
                case GameState.MenuOnly:
                    Input.MouseMode = Input.MouseModeEnum.Visible;
                    // unpause.
                    LevelManager.Unpause();
                    LevelManager.Discard();
                    // record run.
                    DataManager.RecordPlaythrough(LevelManager.LevelIndex, LevelManager.CheckpointIndex, PlayerManager.Lives);
                    break;
                case GameState.Pause:
                    Input.MouseMode = Input.MouseModeEnum.Visible;
                    // pause specific things here.
                    LevelManager.Pause();
                    break;
                case GameState.Gameplay:
                    Input.MouseMode = Input.MouseModeEnum.Captured;
                    // unpause.
                    LevelManager.Unpause();
                    break;
                default:
                    string s = $"State {state} is not implemented in GM! This bad!";
                    Logger.StaticLogger.Write(s, LogLevel.error);
                    throw new NotImplementedException(s);
            }
        }

        #region helperMethods
        private void SetOtherManagers()
        {
            // Check other managers for their singleton instances, and if they don't exist, create them.
            PlayerManager = playerManPrefab.Instantiate() as PlayerManager; AddChild(PlayerManager);
            InputManager = inputManPrefab.Instantiate() as InputManager; AddChild(InputManager);
            UIManager = uiManPrefab.Instantiate() as UIManager; AddChild(UIManager);
            LevelManager = levelManPrefab.Instantiate() as LevelManager; AddChild(LevelManager);
        }

        private void SetGMSingleton()
        {
            if (Instance == null) { Instance = this; }
            else { QueueFree(); return; }
        }

        private void Init()
        {
            // Disable saving debugs to file.
            Logger.StaticLogger.allowDebug = false;

            // Load data from previous run. This could probably be encapsulated into another initialization action.
            PlayerManager.Lives = DataManager.RunData.lives;
            LevelManager.LevelIndex = DataManager.RunData.level;
            LevelManager.CheckpointIndex = DataManager.RunData.checkpoint;
        }

        /// <summary>
        /// We'll use this to start a shutdown sequence, allowing for persistent objects to be saved.
        /// </summary>
        internal void StartQuit()
        {
            Logger.StaticLogger.WriteAll($"Quitting!", LogLevel.info);

            // Handle data things.
            DataManager.OnShutdown();

            // Call this last.
            GetTree().Quit();
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
