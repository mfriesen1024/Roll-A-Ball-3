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
                Logger.StaticLogger.WriteAll($"Shape{shape}, shape's shape?{shape.Shape}");
            }

            trigger.BodyEntered += OnTriggerEnter;
            trigger.BodyExited += OnTriggerExit;
        }

        protected override void Move(bool unlocked, float lerpFactor, double delta)
        {
            // Confirmed working.
            // Capture velocity before and after our move.
            Vector3 oldPos = Position;
            base.Move(unlocked, lerpFactor, delta);
            Vector3 newPos = Position;

            Vector3 posDelta = newPos - oldPos;

            foreach (PhysicsBody3D pb in attachedObjects)
            {
                pb.Position += posDelta;
                //Logger.StaticLogger.WriteAll($"Have captured obj of name {pb.Name}");
            }
        }

        private void OnTriggerExit(Node3D body)
        {
            Logger.StaticLogger.WriteAll($"Triggered by object {body.Name}");
            if (body is PhysicsBody3D pb && body != this)
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
            Logger.StaticLogger.WriteAll($"Triggered by object {body.Name}");
            if (body is PhysicsBody3D pb && body != this)
            {
                var aoList = attachedObjects.ToList();
                aoList.Add(pb);
                attachedObjects = aoList.ToArray();
            }
        }
    }
}
