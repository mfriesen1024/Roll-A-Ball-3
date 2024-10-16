using System;
using System.Collections.Generic;
﻿using Godot;
using KeystoneUtils.Logging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollABall.Assets.src.LevelObjects.Obstacles
{
    /// <summary>
    /// A platform the player can ride on.
    /// </summary>
    internal partial class MovingPlatform : MovingCollider
    {
        [Export] Area3D trigger;

        PhysicsBody3D[] attachedObjects = [];

        public override void _Ready()
        {
            base._Ready();

            if (trigger == null)
            {
                trigger = new();
                CollisionShape3D shape = new CollisionShape3D();
                shape.Shape = new BoxShape3D();
                trigger.AddChild(shape);
                trigger.Position = Vector3.Up;
                AddChild(trigger);
            }

            trigger.BodyEntered += OnTriggerEnter;
            trigger.BodyExited += OnTriggerExit;
        }

        protected override void Move(bool unlocked, float lerpFactor, double delta)
        {
            // Capture velocity before and after our move.
            Vector3 oldPos = Position;
            base.Move(unlocked, lerpFactor,delta);
            Vector3 newPos = Position;

            Vector3 posDelta = newPos - oldPos;

            foreach(PhysicsBody3D pb in attachedObjects)
            {
                pb.Position += posDelta;
            }
        }

        private void OnTriggerExit(Node3D body)
        {
            if (body is PhysicsBody3D pb)
            {
                var aoList = attachedObjects.ToList();
                bool changed = false;
                foreach (PhysicsBody3D ao in aoList)
                {
                    if (pb.Equals(ao))
                    {
                        aoList.Remove(ao);
                        changed = true;
                        break;
                    }
                }
                if (changed) { attachedObjects = aoList.ToArray(); }
            }
        }

        private void OnTriggerEnter(Node3D body)
        {
            if (body is PhysicsBody3D pb)
            {
                var aoList = attachedObjects.ToList();
                aoList.Add(pb);
                attachedObjects = aoList.ToArray();
            }
        }
    }
}
