using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : ManagerBase<StepManager> {

    public static event Action step;
    
    StepManager()
    {
        step = delegate ()
        {
            UpdateStep();
        };
    }

    ~StepManager()
    {

    }
    
    void UpdateStep()
    {
        // Update All Managers
        PlayerManager.instance.UpdateAtStep();
    }
}
