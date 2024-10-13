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
        #region refs
        GameManager gameManager;
        public static UIManager Instance { get; private set; }
        public UIState State { get => state; set { state = value; OnSetState(value); } }
        private UIState state;
        public Logger log;
        #endregion
        #region UIObjectRefs
        Control mainMenu, options, levelSelect, loading;
        //HUD hud;
        Control levelComplete, levelFailure;
        [Export] PackedScene mainMenuScene, optionsScene, levelSelectScene, loadingScene, hudScene, levelCompleteScene, levelFailureScene;
        #endregion

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

        void HideAll()
        {
            if (mainMenu != null) { mainMenu.QueueFree(); mainMenu = null; }
            if (options != null) { options.QueueFree(); options = null; }
            if (levelSelect != null) { levelSelect.QueueFree(); levelSelect = null; }
            if (loading != null) { loading.QueueFree(); loading = null; }
            //if (hud != null) { hud.QueueFree(); hud = null; }
            if (levelComplete != null) { levelComplete.QueueFree(); levelComplete = null; }
            if (levelFailure != null) { levelFailure.QueueFree(); levelFailure = null; }
        }
        void MainMenuHelper()
        {
            mainMenu = mainMenuScene.Instantiate() as Control;
        }
        void OptionsHelper()
        {
            options = optionsScene.Instantiate() as Control;
        }
        void LevelSelectHelper()
        {
            levelSelect = levelSelectScene.Instantiate() as Control;
        }
        void LoadingHelper()
        {
            loading = loadingScene.Instantiate() as Control;
        }
        void HudHelper()
        {
            //hud = hudScene.Instantiate() as Control;
        }
        void LevelCompleteHelper()
        {
            levelComplete = levelCompleteScene.Instantiate() as Control;
        }
        void LevelFailureHelper()
        {
            levelFailure = levelFailureScene.Instantiate() as Control;
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
