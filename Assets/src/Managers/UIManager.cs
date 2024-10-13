using Godot;
using KeystoneUtils.Logging;
using System;

namespace RollABall.Assets.src.Managers
{
    /// <summary>
    /// Manages UI.
    /// </summary>
    internal partial class UIManager : Node
    {
        GameManager gameManager;
        public static UIManager Instance { get; private set; }
        public UIState State { get => state; set { state = value; OnSetState(value); } }
        private UIState state;
        public Logger log;

        public override void _Ready()
        {
            if (Instance != null) { QueueFree(); return; }

            Instance = this;
            log = new Logger(true, true, "logs\\", "uiLog", "txt", true);
            GameManager.postInit += PostInit;
        }

        public void PostInit()
        {
            gameManager = GameManager.Instance;
            log.Write("Initialization finished, setting state of MainMenu");
        }

        private void OnSetState(UIState value)
        {
            switch (value)
            {
                case UIState.Main: 
                case UIState.Options:
                case UIState.LevelSelect:
                case UIState.Loading:
                case UIState.HUD:
                case UIState.LevelComplete:
                case UIState.LevelFailure:
                default:
                    string s = $"State {value} is not implemented.";
                    log.Write(s); throw new NotImplementedException(s);
            }
        }
    }


    public enum UIState
    {
        Main,
        Options,
        LevelSelect,
        Loading,
        HUD,
        LevelComplete,
        LevelFailure
    }
}
