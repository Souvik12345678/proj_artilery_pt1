using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachingBaseState : State
{
    private TankAIScript_pt1 tankAIScript;
    NewTankScript tankController;

    public ApproachingBaseState(StateMachine machine, TankAIScript_pt1 tankAIScript)
    {
        stateMachineInstance = machine;
        this.tankAIScript = tankAIScript;
    }
 
    public override void OnEnter()
    {
        base.OnEnter();

        tankAIScript.
    }

    public override void OnUpdate()
    {
        base.OnUpdate();


    }

    public override void OnExit()
    {
        base.OnExit();
    }

}
