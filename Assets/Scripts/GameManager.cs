using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zachclone {
    public class GameManager : MonoBehaviour {
        public float SimulateStepTime = .5f;
        public float AdvanceStepTime = .01f;
        public List<Chip> chips;

        private int currentTick;

        private float nextActionTime;


        public bool IsRunning { get; protected set; }
        public bool IsRunningAdvance { get; protected set; }
        public bool IsRunningSimulation { get; protected set; }

        public void Start() {
            var wire1 = new Wire();
            var wire2 = new Wire();
            chips[0].WireRegister(wire1, Register.X0);
            chips[1].WireRegister(wire1, Register.X0);
            chips[0].WireRegister(wire2, Register.X1);
            chips[1].WireRegister(wire2, Register.X1);
        }

        private void Initialize() {
            IsRunning = chips.TrueForAll(chip => chip.Enable());
            if (!IsRunning) {
                chips.ForEach(chip => chip.Disable());
            }
            
            currentTick = 0;
        }

        public void Stop() {
            chips.ForEach(c => c.Disable());
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
                StepInternal();
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
            if (!(Time.time > nextActionTime)) return false;
            nextActionTime = Time.time + AdvanceStepTime;
            return StepInternal();
        }

        private bool StepInternal() {
            var allAsleep = true;
            
            foreach (var chip in chips) {
                if (!chip.IsSleeping) {
                    allAsleep = false;
                    chip.Step();
                }
            }

            if (allAsleep) {
                chips.ForEach(chip => chip.Step());
                currentTick++;
            }

            return allAsleep;
            
        }
    }
}