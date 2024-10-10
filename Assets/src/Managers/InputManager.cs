using Godot;
using KeystoneUtils.Logging;
using KeystoneUtils.Logic;
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
            inputLog = new Logger(true, true, "logs\\", "inputLog", "txt", true);

            // Set references.
            controller = PlayerManager.Instance.controller;
        }

        public override void _Input(InputEvent ie)
        {
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
                case Key.Escape:
                default: break;
            }
        }

        // Get a relative vector for the player's motion.
        void HandleMoveInput()
        {
            Vector2 newMove = Vector2.Zero;
            // Only set an axis' value if and only if one of its keys is pressed.
            if (Gates.XOR(Input.IsKeyPressed(Key.S), Input.IsKeyPressed(Key.W))) { if (Input.IsKeyPressed(Key.S)) { newMove.X--; } else { newMove.X++; } }
            if (Gates.XOR(Input.IsKeyPressed(Key.A), Input.IsKeyPressed(Key.D))) { if (Input.IsKeyPressed(Key.A)) { newMove.Y--; } else { newMove.Y++; } }

            // Move the player by this but normalized. No idea if normalizing is redundant here.
            controller.OnMove(newMove.Normalized());
        }
        #endregion
    }
}
