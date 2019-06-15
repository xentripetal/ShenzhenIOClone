using UnityEngine;
using UnityEngine.UI;
using Zachclone;

namespace UI {
    public class SimulationButtonManager : MonoBehaviour {
        public GameManager manager;
        public Button AdvanceButton;
        public Button PauseButton;

        public Button ResetButton;
        public Button StepButton;

        public void OnResetPressed() {
            manager.Stop();
            PauseButton.interactable = false;
            ResetButton.interactable = false;
            StepButton.interactable = true;
            AdvanceButton.interactable = true;
        }

        public void OnPausePressed() {
            manager.Pause();
            PauseButton.interactable = false;
            StepButton.interactable = true;
            AdvanceButton.interactable = true;
        }

        public void OnStepPressed() {
            manager.Step();
            ResetButton.interactable = manager.IsRunning;
        }

        public void OnAdvancePressed() {
            manager.Advance();
            if (manager.IsRunning) {
                ResetButton.interactable = true;
                PauseButton.interactable = false;
            }
        }

        public void OnSimulatePressed() {
            if (manager.IsRunningSimulation) {
                OnPausePressed();
                return;
            }
            manager.Simulate();
            if (manager.IsRunning) {
                ResetButton.interactable = true;
                PauseButton.interactable = true;
                StepButton.interactable = false;
                AdvanceButton.interactable = false;
            }
        }

        public void OnTimescaleSliderChanged(float value) {
            var time = 1 - value;
            time = time * time;
            manager.SimulateStepTime = time;
        }
    }
}