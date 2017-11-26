using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : ManagerBase<StepManager> {

    public static event Action step;
    public int stepCount = 0;

    public static void ReStart()
    {
        instance.stepCount = 0;
    }

    private void Awake()
    {
        step += StepManager.UpdateStep;
    }

    public static void InvokeStep()
    {
        //step.Invoke();
        UpdateStep();
    }


    public static void UpdateStep()
    {
        ++instance.stepCount;
        // Orders are important
        EnemyBehavior.instance.UpdateAtStep();
        PlayerManager.UpdateAtStep();
        SlimeBehaviourManger.instance.UpdateAtStep();
		SoundManager.instance.UpdateAtStep ();
        NPCManager.instance.UpdateAtStep();
    }
}
