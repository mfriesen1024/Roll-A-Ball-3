using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.Player
{
    // This should be used to fetch the player's forward direction, and it's parent should be rotated with mouse movement.
    internal partial class PlayerCam:Camera3D
    {
        public Vector2 Get2DForward()
        {
            Vector3 trueForward = Transform.Basis.Z;
            Vector2 forward2d = new Vector2(trueForward.X,trueForward.Z);
            return forward2d;
        }
    }
}
