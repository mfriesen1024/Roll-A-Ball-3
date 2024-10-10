﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.Managers
{
    internal partial class InputManager:Node
    {
        public static InputManager Instance { get; private set; }

        public override void _Ready()
        {
            if(Instance == null) { Instance = this; }
            else { QueueFree(); }
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

        void HandleMoveInput()
        {
            Vector2 newMove = Vector2.Zero;
            if (Gates.XOR(Input.IsKeyPressed(Key.S), Input.IsKeyPressed(Key.W))) { if (Input.IsKeyPressed(Key.S)) { newMove.X--; } else { newMove.X++; } }
            if (Gates.XOR(Input.IsKeyPressed(Key.A), Input.IsKeyPressed(Key.D))) { if (Input.IsKeyPressed(Key.A)) { newMove.Y--; } else { newMove.Y++; } }

            controller.OnMove(newMove);
        }
    }
}
