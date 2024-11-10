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

        bool hudEnabled = false;

        public override void _Ready()
        {
            if (Instance != null) { QueueFree(); return; }

            Instance = this;
            log = new Logger(true, true, "logs\\", "uiLog", "txt", true);
            GameManager.postInit += PostInit;

            hud = hudScene.Instantiate() as HUD;
        }

        public void PostInit()
        {
            gameManager = GameManager.Instance;
            log.Write("Initialization finished, setting state of MainMenu");
            State = UIState.Main;
        }

        private void OnSetState(UIState value)
        {
            log.WriteAll($"Got a state request of {value}!");

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
            if (hudEnabled) { RemoveChild(hud); hudEnabled = false; }
            if (pause != null) { pause.QueueFree(); pause = null; }
            if (levelComplete != null) { levelComplete.QueueFree(); levelComplete = null; }
            if (levelFailure != null) { levelFailure.QueueFree(); levelFailure = null; }
        }
        void MainMenuHelper()
        {
            mainMenu = mainMenuScene.Instantiate() as Control;
            AddChild(mainMenu);

            GameManager.Instance.State = GameState.MenuOnly;

            Button play = mainMenu.FindChild("play", true) as Button;
            play.Pressed += () => { State = UIState.LevelSelect; };
            Button load = mainMenu.FindChild("load", true) as Button;
            load.Pressed += () => { LevelManager.Instance.LoadCheckpoint(); };
            Button options = mainMenu.FindChild("options", true) as Button;
            options.Pressed += () => { State = UIState.Options; };
            Button exit = mainMenu.FindChild("exit", true) as Button;
            exit.Pressed += gameManager.StartQuit;
        }
        void OptionsHelper()
        {
            options = optionsScene.Instantiate() as Control;
            AddChild(options);

            Button back = options.FindChild("return", true) as Button;
            back.Pressed += () => { State = UIState.Main; };
        }
        void LevelSelectHelper()
        {
            levelSelect = levelSelectScene.Instantiate() as Control;
            AddChild(levelSelect);

            ShowLevel();

            Button left = levelSelect.FindChild("left", true) as Button;
            left.Pressed += () => { log.WriteAll($"Level select cycle left requested, but not implemented!", LogLevel.warn); };
            Button right = levelSelect.FindChild("right", true) as Button;
            right.Pressed += () => { log.WriteAll($"Level select cycle right requested, but not implemented!", LogLevel.warn); };
            Button go = levelSelect.FindChild("go", true) as Button;
            go.Pressed += () => { LevelManager.Instance.Load(); };
        }
        void LoadingHelper()
        {
            loading = loadingScene.Instantiate() as Control;
            AddChild(loading);
        }
        void HudHelper()
        {
            AddChild(hud); hudEnabled = true;

            GameManager.Instance.State = GameState.Gameplay;
        }
        void PauseHelper()
        {
            pause = pauseScene.Instantiate() as Control;
            AddChild(pause);

            GameManager.Instance.State = GameState.Pause;

            Button resume = pause.FindChild("resume", true) as Button;
            resume.Pressed += () => { State = UIState.HUD; };
            Button menu = pause.FindChild("menu", true) as Button;
            menu.Pressed += () => { State = UIState.Main; };
        }
        void LevelCompleteHelper()
        {
            GameManager.Instance.State = GameState.Pause;
            levelComplete = levelCompleteScene.Instantiate() as Control;
            AddChild(levelComplete);

            Button menu = levelComplete.FindChild("menu", true) as Button;
            menu.Pressed += () => { State = UIState.Main; };
        }
        void LevelFailureHelper()
        {
            GameManager.Instance.State = GameState.Pause;
            levelFailure = levelFailureScene.Instantiate() as Control;
            AddChild(levelFailure);

            Button menu = levelFailure.FindChild("menu", true) as Button;
            menu.Pressed += () => { State = UIState.Main; };
        }

        void ShowLevel()
        {
            int levelIndex = LevelManager.Instance.LevelIndex;

            Label text = levelSelect.FindChild("name", true) as Label;
            TextureRect preview = levelSelect.FindChild("preview",true) as TextureRect;

            text.Text = LevelManager.Instance.LevelNames[levelIndex];
            preview.Texture = LevelManager.Instance.LevelTextures[levelIndex];
        }
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
