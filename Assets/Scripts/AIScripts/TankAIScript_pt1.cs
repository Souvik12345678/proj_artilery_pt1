using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIScript_pt1 : MonoBehaviour
{
    public ArmyBaseScript targetBase;

    [SerializeField]
    FOVObsCheckScript obsCheckScript;

    TankAIStateMachine stateMachine;
    public List<NewTankScript> enemiesInSight;

    public string currentStateName;

    NewTankScript tankController;

    private void Awake()
    {
        
    }

    private void Start()
    {
        InitStateMachine();
    }

    private void Update()
    {
        CalculateState();
        CalculateTargetProperties();

        currentStateName = stateMachine.GetCurrentStateName();

        stateMachine.Update();
    }

    private void OnDestroy()
    {
        //stateMachine.Exit();
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
        stateMachine = new TankAIStateMachine(this);

        stateMachine.Initialize("APPR_BASE");

        stateMachine.Start();

    }

}
