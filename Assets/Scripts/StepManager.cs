using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : ManagerBase<StepManager> {

    public static event Action step;

    private void Awake()
    {
        step += StepManager.UpdateStep;
    }

    public static void InvokeStep()
    {
        step.Invoke();
    }


    public static void UpdateStep()
    {
        // Update All Managers
        //PlayerManager.UpdateAtStep();
    }
}
