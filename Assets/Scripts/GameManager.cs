using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Chip chip;
    public float AdvanceStepTime = .01f;
    
    private float _waitTime = .5f;
    private bool runAdvance = false;
    private bool runSimulation = false;

    private float nextActionTime = 0f;
    
    void Update() {
        if (runAdvance) {
            if (Time.time > nextActionTime) {
                nextActionTime = Time.time + AdvanceStepTime;
                chip.Step();
                if (chip.IsSleeping) {
                    runAdvance = false;
                }
            }
            return;
        }

        if (runSimulation) {
            
        }
    }
}