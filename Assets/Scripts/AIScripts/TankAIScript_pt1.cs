using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIScript_pt1 : MonoBehaviour
{
    public ArmyBaseScript targetBase;

    [SerializeField]
    FOVObsCheckScript obsCheckScript;

    StateMachine stateMachine;
    List<NewTankScript> enemiesInSight;

    Dictionary<string, State> behaviourStates;

    string currentStateName;

    private void Awake()
    {
        enemiesInSight = new List<NewTankScript>();
        behaviourStates = new Dictionary<string, State>();

        behaviourStates.Add("APPR_BASE", new ApproachingBaseState(stateMachine, this));
        behaviourStates.Add("ATTK_ENEMIES", new AttackingTroopsState(stateMachine, this));
        behaviourStates.Add("NONE", new State());
    }

    private void Start()
    {
        InitStateMachine();
    }

    private void Update()
    {
        CalculateState();
        CalculateTargetProperties();

        if (enemiesInSight.Count > 0)
        {
            ChangeState("ATTK_ENEMIES");
        }

        stateMachine.Update();
    }

    private void OnDestroy()
    {
        stateMachine.Exit();
    }

    private void CalculateTargetProperties()
    {
        enemiesInSight.Clear();//Clear enemy list
        //Add enemies to enemy list from obstacle check script
        if (obsCheckScript.isObstaclesInRange)//If obstacles in range
        {
            foreach (var item in obsCheckScript.obstaclesInRange)
            {
                if (item != null && item.layer == LayerMask.NameToLayer("Tank"))//If obstacles are enemy tanks
                {
                    if (item.GetComponent<NewTankScript>().GetHealthScript().currentHP > 0)
                    {
                        if (!item.GetComponent<NewTankScript>().CompareTag(tag))//If target is not in our team
                        { enemiesInSight.Add(item.GetComponent<NewTankScript>()); }
                    }
                }
            }
        }
    }

    void CalculateState()
    {
        
    
    }

    void InitStateMachine()
    {
        stateMachine = new StateMachine();

        //var state1 = new ApproachingBaseState(stateMachine, this);
        var state1 = new ApproachingBaseAndMuzzleAim(stateMachine, this);


        stateMachine.Initialize(state1);

        stateMachine.Start();

    }

    void ChangeState(string stateName)
    {
        stateMachine.ChangeState(behaviourStates[stateName]);
    }

}
