using Godot;
using KeystoneUtils.Logging;
using RollABall.Assets.src.UI;
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
        public UIState State { get => state; set { OnSetState(value); state = value; } }
        private UIState state;
        public Logger log;
        #endregion
        #region UIObjectRefs
        Control mainMenu, options, levelSelect, loading;
        public HUD hud { get; private set; }
        Control pause, levelComplete, levelFailure;
        [Export] PackedScene mainMenuScene, optionsScene, levelSelectScene, loadingScene, hudScene, pauseScene, levelCompleteScene, levelFailureScene;
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
            State = UIState.Main;
        }

        private void OnSetState(UIState value)
        {
            HideAll();
            switch (value)
            {
                case UIState.Main: MainMenuHelper(); break;
                case UIState.Options: OptionsHelper(); break;
                case UIState.LevelSelect: LevelSelectHelper(); break;
                case UIState.Loading: LoadingHelper(); break;
                case UIState.HUD: HudHelper(); break;
                case UIState.Pause: PauseHelper(); break;
                case UIState.LevelComplete: LevelCompleteHelper(); break;
                case UIState.LevelFailure: LevelFailureHelper(); break;
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
            if (hud != null) { hud.QueueFree(); hud = null; }
            if (levelComplete != null) { levelComplete.QueueFree(); levelComplete = null; }
            if (levelFailure != null) { levelFailure.QueueFree(); levelFailure = null; }
        }
        void MainMenuHelper()
        {
            mainMenu = mainMenuScene.Instantiate() as Control;
            AddChild(mainMenu);

            Button play = mainMenu.FindChild("play", true) as Button;
            // This should be used, but currently, we dont have level selection implemented.
            //play.Pressed += () => { State = UIState.LevelSelect; };
            play.Pressed += () => { State = UIState.HUD; };
            Button options = mainMenu.FindChild("options", true) as Button;
            options.Pressed += () => { State = UIState.Options; };
            Button exit = mainMenu.FindChild("exit", true) as Button;
            exit.Pressed += () => { gameManager.StartQuit(); };
        }
        void OptionsHelper() { options = optionsScene.Instantiate() as Control; }
        void LevelSelectHelper() { levelSelect = levelSelectScene.Instantiate() as Control; }
        void LoadingHelper() { loading = loadingScene.Instantiate() as Control; }
        void HudHelper() { hud = hudScene.Instantiate() as HUD; }
        void PauseHelper() { pause = pauseScene.Instantiate() as Control; }
        void LevelCompleteHelper() { levelComplete = levelCompleteScene.Instantiate() as Control; }
        void LevelFailureHelper() { levelFailure = levelFailureScene.Instantiate() as Control; }
    }

    public enum UIState
    {
        Main,
        Options,
        LevelSelect,
        Loading,
        HUD,
        Pause,
        LevelComplete,
        LevelFailure
    }
}
