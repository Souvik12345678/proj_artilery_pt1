using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Approaching base state
/// </summary>
public class ApproachingBaseState : State
{
    protected TankAIScript_pt1 tankAIScript;
    protected NewTankScript tankController;
    protected Transform selfTransform;

    float targetDistanceTolerance = 7;
    Vector2 dirToTarget;
    ArmyBaseScript targetBase;


    public ApproachingBaseState(StateMachine machine, TankAIScript_pt1 tankAIScript)
    {
        name = "APPR_BASE";
        stateMachineInstance = machine;
        this.tankAIScript = tankAIScript;
        selfTransform = tankAIScript.transform;
        tankController = tankAIScript.GetComponent<NewTankScript>();
        targetBase = tankAIScript.targetBase;
    }
 
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        DriveTank();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    bool DriveTank()
    {
        bool isMoving = false;
        float distanceToTarget = Vector2.Distance(selfTransform.position, targetBase.transform.position);

        //If target too far
        if (distanceToTarget > targetDistanceTolerance)
        {
            dirToTarget = (targetBase.transform.position - selfTransform.position).normalized;
            float dotProd = Vector2.Dot(selfTransform.up, dirToTarget);

            //move forward?
            if (dotProd > 0)
            {
                tankController.Move(1, 0);
            }
            //move backward?
            else
            {
                if (distanceToTarget > 2.5f)
                {
                    //Too far to reverse
                    tankController.Move(1, 0);
                }
                else
                {
                    tankController.Move(-1, 0);
                }
            }

            isMoving = true;
        }
        else//Reached target
        {
        }

        return isMoving;
    }

}

/// <summary>
/// Muzzle aim state
/// </summary>
public class MuzzleAimState : State
{
    protected TankAIScript_pt1 tankAIScript;
    NewTankScript tankController;
    Transform selfTransform;
    Vector2 faceDir, dirToTarget;

    public MuzzleAimState(StateMachine machine, TankAIScript_pt1 tankAIScript)
    {
        stateMachineInstance = machine;
        this.tankAIScript = tankAIScript;
        selfTransform = tankAIScript.transform;
        tankController = tankAIScript.GetComponent<NewTankScript>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Transform currTarget = tankAIScript.targetBase.transform;
        dirToTarget = (currTarget.transform.position - selfTransform.position).normalized;
        TryFaceTowardsDirection();
    }

    void TryFaceTowardsDirection()
    {
        float angle = Vector2.SignedAngle(dirToTarget, tankController.muzzleTransform.up);
        if (angle > 0)
        { tankController.MuzzleRotate(1); }
        else { tankController.MuzzleRotate(-1); }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}

public class ApproachingBaseAndMuzzleAimState : ApproachingBaseState
{
    Vector2 faceDir, dirToTarget;
    
    public ApproachingBaseAndMuzzleAimState(StateMachine stateMachine, TankAIScript_pt1 tankAIScript) : base(stateMachine, tankAIScript)
    {

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Transform currTarget = tankAIScript.targetBase.transform;
        dirToTarget = (currTarget.transform.position - selfTransform.position).normalized;
        TryFaceTowardsDirection();

        CheckForEnemy();
    }

    void CheckForEnemy()
    {
        if (tankAIScript.enemiesInSight.Count > 0)
        {
            stateMachineInstance.ChangeState("ATTK_ENEM");
        }
    
    }

    void TryFaceTowardsDirection()
    {
        float angle = Vector2.SignedAngle(dirToTarget, tankController.muzzleTransform.up);
        if (Mathf.Abs(angle) > 7)
        {
            if (angle > 0)
            { tankController.MuzzleRotate(1); }
            else { tankController.MuzzleRotate(-1); }
        }
    }

}

/// <summary>
/// Attacking enemy tanks
/// </summary>
public class AttackingTroopsState : State
{
    private TankAIScript_pt1 tankAIScript;
    NewTankScript tankController;
    Transform selfTransform;
    Vector2 faceDir, dirToTarget;

    public AttackingTroopsState(StateMachine machine, TankAIScript_pt1 tankAIScript)
    {
        stateMachineInstance = machine;
        this.tankAIScript = tankAIScript;
        selfTransform = tankAIScript.transform;
        tankController = tankAIScript.GetComponent<NewTankScript>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Transform currTarget = tankAIScript.enemiesInSight[0].transform;
        dirToTarget = (currTarget.transform.position - selfTransform.position).normalized;
        TryFaceTowardsDirection();
        TryShoot();

    }

    void TryFaceTowardsDirection()
    {
        float angle = Vector2.SignedAngle(dirToTarget, tankController.muzzleTransform.up);
        if (Mathf.Abs(angle) > 2)
        {
            if (angle > 0)
            { tankController.MuzzleRotate(1); }
            else { tankController.MuzzleRotate(-1); }
        }
    }

    void TryShoot()
    {
        if (IsFacingTarget(tankAIScript.enemiesInSight[0].transform))
        {
            tankController.Shoot();
        }
    
    }

    bool IsFacingTarget(Transform targTrans)
    {
        Vector2 dirToT = (targTrans.position - selfTransform.position).normalized;
        float angle = Vector2.Angle(dirToT, tankController.muzzleTransform.up);
        if (angle < 2)
        { return true; }
        return false;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}

