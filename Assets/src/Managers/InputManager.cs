using Godot;
using KeystoneUtils.Logging;
using RollABall.Assets.src.Player;

namespace RollABall.Assets.src.Managers
{
    internal partial class InputManager : Node
    {
        #region refs
        public static InputManager Instance { get; private set; }
        Logger inputLog;

        // Components
        PlayerController controller;
        #endregion

        public override void _Ready()
        {
            // Set singleton
            if (Instance == null) { Instance = this; }
            else { QueueFree(); return; }

            // Set logger
            inputLog = new Logger(true, true, "logs\\", "inputLog", "txt", false);

            // Set references.
            controller = PlayerManager.Instance.controller;
        }

        public override void _Input(InputEvent ie)
        {
            inputLog.Write($"Got an input of type {ie.GetType()}.");
            if (ie is InputEventMouse) { MouseInput((InputEventMouse)ie); }
            if (ie is InputEventKey) { KeyInput((InputEventKey)ie); }
        }

        #region inputImplementation
        void MouseInput(InputEventMouse iem)
        {
            if (iem is InputEventMouseMotion iemm) { controller.OnLook(iemm.Relative); }
            if (iem is InputEventMouseButton iemb) { inputLog.Write("Mouse button input not implemented!", LogLevel.warn); }
        }

        void KeyInput(InputEventKey iek)
        {
            // For now, we'll hardcode inputs. No idea if I'll change it around later or not.
            switch (iek.PhysicalKeycode)
            {
                case Key.W:
                case Key.A:
                case Key.S:
                case Key.D: HandleMoveInput(); break;
                case Key.Escape: if (UIManager.Instance.State == UIState.HUD) { UIManager.Instance.State = UIState.Pause; } break;
                case Key.Space: HandleJumpInput(); break;
                default: break;
            }
        }

        private void HandleJumpInput()
        {
            inputLog.WriteAll($"Jump input!");
            if (Input.IsPhysicalKeyPressed(Key.Space)) { controller.OnJump(); }
        }

        // Get a relative vector for the player's motion.
        void HandleMoveInput()
        {
            Vector2 newMove = Vector2.Zero;
            // Only set an axis' value if and only if one of its keys is pressed.
            bool s = Input.IsKeyPressed(Key.S);
            bool w = Input.IsKeyPressed(Key.W);
            if (s||w && !(s&&w)) { if (s) { newMove.Y++; } if (w) { newMove.Y--; } }

            bool a = Input.IsKeyPressed(Key.A);
            bool d = Input.IsKeyPressed(Key.D);
            if (a||d && !(a&&d)) { if (a) { newMove.X--; } if (d) { newMove.X++; } }

            // Move the player by this but normalized. No idea if normalizing is redundant here.
            controller.OnMove(newMove.Normalized());

            inputLog.Write($"Handled move. W:{w}A:{a}S:{s}D:{d}, vector is {newMove}.");
        }
        #endregion
    }
}
