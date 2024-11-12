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

            // Create a new trigger if we don't already have one.
            if (trigger == null)
            {
                trigger = new();
                CollisionShape3D shape = new CollisionShape3D();
                shape.Shape = new BoxShape3D();
                trigger.AddChild(shape);
                trigger.Position = Vector3.Up;
                AddChild(trigger);
                Logger.StaticLogger.Write($"Shape {shape}, shape's shape?{shape.Shape}");
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

        /// <summary>
        /// When an object leaves the trigger, ensure we aren't tracking it anymore.
        /// </summary>
        /// <param name="body">The object that left the trigger.</param>
        private void OnTriggerExit(Node3D body)
        {
            Logger.StaticLogger.Write($"Triggered by object {body.Name}");
            // If the object is a movable physics object, track it.
            if (body is PhysicsBody3D pb && body is not StaticBody3D && body != this)
            {
                // Make a list so we can modify the array. 
                var aoList = attachedObjects.ToList();
                bool changed = false;

                // Iterate through the list, and if we find our object, remove it and note that our list has changed.
                foreach (PhysicsBody3D ao in aoList)
                {
                    if (pb.Equals(ao))
                    {
                        aoList.Remove(ao);
                        changed = true;
                        break;
                    }
                }

                // If list changed, update the array.
                if (changed) { attachedObjects = aoList.ToArray(); }
            }
        }

        /// <summary>
        /// If our trigger was entered by a movable physics object, grab it.
        /// </summary>
        /// <param name="body">The object that entered the trigger.</param>
        private void OnTriggerEnter(Node3D body)
        {
            Logger.StaticLogger.Write($"Triggered by object {body.Name}");
            if (body is PhysicsBody3D pb && body is not StaticBody3D && body != this)
            {
                var aoList = attachedObjects.ToList();
                aoList.Add(pb);
                attachedObjects = aoList.ToArray();
            }
        }
    }
}
