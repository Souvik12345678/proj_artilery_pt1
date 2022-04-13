using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTankAIScript : MonoBehaviour
{
    //Inspector
    public float targetDistanceTolerance;
    public ArmyBaseScript targetBase;

    public bool IsTargetAvailable
    {
        get { return targetBase != null; }
    }

    [SerializeField]
    FOVObsCheckScript obsCheckScript;
    [SerializeField]
    TankScript tankController;

    public enum STATE { NONE, NO_TARGET, APPROACHING_BASE, ENEMY_IN_SIGHT, ATTACKING_TROOPS, ATTACKING_BASE, TARGET_DESTROYED };
    public STATE currentState;

    Vector2 dirToTarget;
    [SerializeField]
    List<TankScript> enemyList = new List<TankScript>();

    // Start is called before the first frame update
    void Start()
    {
        if (IsTargetAvailable)
        { currentState = STATE.NO_TARGET; }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateState();
        if (IsTargetAvailable)
        {
            CalculateTargetProperties();
            MovementLogic();
            AttackLogic();
        }
    }

    void CalculateState()
    {
        if (!IsTargetAvailable)
        {
            currentState = STATE.NO_TARGET;
        }
        if (StateCheckForIsAppBase())
        {
            currentState = STATE.APPROACHING_BASE;
        }
        if (StateCheckForIsEneInSight())
        {
            currentState = STATE.ENEMY_IN_SIGHT;
        }
        if (StateCheckForAttackingTroops())
        {
            currentState = STATE.ATTACKING_TROOPS;
        }
        if (StateCheckForAttBase())
        {
            currentState = STATE.ATTACKING_BASE;

        }
        if (StateCheckForTargDest())
        {
            currentState = STATE.TARGET_DESTROYED;
        }
    }

    //----------
    //State change functions
    //----------

    bool StateCheckForIsAppBase()
    {
        if (IsTargetAvailable)
        {
            if (enemyList.Count == 0)
            {
                return true;
            }
        }
        return false;
    }
    bool StateCheckForIsEneInSight()
    {
        return enemyList.Count > 0;
    }
    bool StateCheckForAttackingTroops()
    {
        if (enemyList.Count > 0 && IsFacingTowardsEnemy(enemyList[0]))
        {
            return true;
        }
        return false;
    }
    bool StateCheckForAttBase()
    {
        float dist = Vector2.Distance(targetBase.transform.position, transform.position);
        if (dist < targetDistanceTolerance && IsFacingTowardsObject(targetBase.transform))
        { return true; }
        return false;
    }
    bool StateCheckForTargDest()
    {
        return targetBase.isDestroyed;
    }

    //----------
    //State change functions -end
    //----------

    private void CalculateTargetProperties()
    {
        enemyList.Clear();//Clear enemy list
        //Add enemies to enemy list from obstacle check script
        if (obsCheckScript.isObstaclesInRange)//If obstacles in range
        {
            foreach (var item in obsCheckScript.obstaclesInRange)
            {
                if (item != null && item.layer == LayerMask.NameToLayer("Tank"))//If obstacles are enemy tanks
                {
                    if (item.GetComponent<TankScript>().healthScript.currentHP > 0)
                    {
                        if (!item.GetComponent<NewTankAIScript>().CompareTag(tag))//If target is not in our team
                        { enemyList.Add(item.GetComponent<TankScript>()); }
                    }
                }
            }
        }
    }

    void MovementLogic()
    {
        if (currentState == STATE.APPROACHING_BASE)
        {
            TryDriveTank();
            TryFaceTowardsEnemy(targetBase.transform.position);
        }
        else if (currentState == STATE.ENEMY_IN_SIGHT)
        {
            TryFaceTowardsEnemy();
        }
        else if (currentState == STATE.ATTACKING_BASE)
        {
        }

    }

    void AttackLogic()
    {
        if (currentState == STATE.ATTACKING_TROOPS)
        {
            TryShoot();
        }
        else if (currentState == STATE.ATTACKING_BASE)
        {
            TryShoot();
        }
    }

    void TryDriveTank()
    {
        DriveTank();
        RotateTank();
    }

    bool DriveTank()
    {
        bool isMoving = false;
        float distanceToTarget = Vector2.Distance(transform.position, targetBase.transform.position);

        //If target too far
        if (distanceToTarget > targetDistanceTolerance)
        {
            dirToTarget = (targetBase.transform.position - transform.position).normalized;
            float dotProd = Vector2.Dot(transform.up, dirToTarget);

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

    void RotateTank()
    {
        //Turn logic
        float angleToDir = Vector2.SignedAngle(transform.up, dirToTarget);

        if (angleToDir < 0 && Mathf.Abs(angleToDir) > 10) { tankController.Move(0, -1); }
        else if (angleToDir > 0 && Mathf.Abs(angleToDir) > 10) { tankController.Move(0, 1); }
        else { }
    }

    void TryShoot()
    {
        tankController.Shoot();
    }

    void TryFaceTowardsEnemy()
    {
        if (enemyList.Count > 0)
        {
            Transform currTarget = enemyList[0].gameObject.transform;
            Vector2 dirToTarget1 = (currTarget.transform.position - transform.position).normalized;
            tankController.FaceToDirection(dirToTarget1);
        }
    }

    bool IsFacingTowardsEnemy(TankScript tank)
    {
        Vector2 dirToTarget = (tank.transform.position - transform.position).normalized;
        return (Vector2.Angle(transform.up, dirToTarget) < 5);
    }

    bool IsFacingTowardsObject(Transform target)
    {
        Vector2 dirToTarget = (target.position - transform.position).normalized;
        return (Vector2.Angle(transform.up, dirToTarget) < 5);
    }

    void TryFaceTowardsEnemy(Vector3 targPos)
    {
        Vector2 dirToTarget1 = (targPos - transform.position).normalized;
        tankController.FaceToDirection(dirToTarget1);
    }
}
