using System;
using UnityEngine;

namespace Zachclone {
    public class GameManager : MonoBehaviour {
        public float SimulateStepTime = .5f;
        public float AdvanceStepTime = .01f;
        public Chip chip;

        private int currentTick;

        private float nextActionTime;


        public bool IsRunning { get; protected set; }
        public bool IsRunningAdvance { get; protected set; }
        public bool IsRunningSimulation { get; protected set; }

        private void Initialize() {
            IsRunning = chip.Enable();
            currentTick = 0;
        }

        public void Stop() {
            chip.Disable();
            IsRunning = false;
            IsRunningAdvance = false;
            IsRunningSimulation = false;
        }

        public void Pause() {
            throw new NotImplementedException();
        }

        public void Step() {
            if (!IsRunning) {
                Initialize();
            } else {
                chip.Step();
                if (chip.IsSleeping) {
                    chip.Step();
                    currentTick++;
                }
            }
        }

        public void Advance() {
            if (!IsRunning) {
                Initialize();
            } else {
                IsRunningAdvance = true;
            }
        }

        public void Simulate() {
            throw new NotImplementedException();
        }
        
        private void Update() {
            if (!IsRunning) return;

            if (IsRunningAdvance) IsRunningAdvance = !AdvanceInternal(AdvanceStepTime);

            if (IsRunningSimulation) {
            }
        }
        
        /// <summary>
        ///     Advances chip if stepTime has passed since last advance.
        /// </summary>
        /// <param name="stepTime"></param>
        /// <returns> True if all chips are sleeping. </returns>
        private bool AdvanceInternal(float stepTime) {
            if (Time.time > nextActionTime) {
                nextActionTime = Time.time + AdvanceStepTime;
                chip.Step();
                return chip.IsSleeping;
            }

            return false;
        }
    }
}