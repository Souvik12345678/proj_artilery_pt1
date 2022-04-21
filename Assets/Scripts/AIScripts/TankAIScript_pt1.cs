using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIScript_pt1 : MonoBehaviour
{
    public enum STATE { NONE, NO_TARGET, APPROACHING_BASE };
    public STATE currentState;

    StateMachine stateMachine;


    private void Start()
    {
        InitStateMachine();
    }

    private void Update()
    {
        CalculateState();

        stateMachine.Update();
    }

    private void OnDestroy()
    {
        stateMachine.Exit();
    }

    void CalculateState()
    {
        
    
    }

    void InitStateMachine()
    {
        stateMachine = new StateMachine();

        var state1 = new ApproachingBaseState(stateMachine, this);
        var state2 = new MuzzleRotateState(stateMachine, this);

        stateMachine.Initialize(state1);
        stateMachine.AddState(state2);

        stateMachine.Start();

    }

}
