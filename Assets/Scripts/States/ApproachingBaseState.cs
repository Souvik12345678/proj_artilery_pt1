using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachingBaseState : State
{
    private TankAIScript_pt1 tankAIScript;
    NewTankScript tankController;

    float targetDistanceTolerance = 7;


    public ApproachingBaseState(StateMachine machine, TankAIScript_pt1 tankAIScript)
    {
        stateMachineInstance = machine;
        this.tankAIScript = tankAIScript;
        tankController = tankAIScript.GetComponent<NewTankScript>();
    }
 
    public override void OnEnter()
    {
        base.OnEnter();
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

public class MuzzleRotateState : State
{
    private TankAIScript_pt1 tankAIScript;
    NewTankScript tankController;

    public MuzzleRotateState(StateMachine machine, TankAIScript_pt1 tankAIScript)
    {
        stateMachineInstance = machine;
        this.tankAIScript = tankAIScript;
        tankController = tankAIScript.GetComponent<NewTankScript>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
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
