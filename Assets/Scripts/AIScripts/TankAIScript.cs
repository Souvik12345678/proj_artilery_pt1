using System;
using UnityEngine;

public class TankAIScript : MonoBehaviour
{
    public float targetDistanceTolerance;

    [SerializeField]
    ArmyBaseScript targetBase;
    [SerializeField]
    GameObject currentTarget;

    [SerializeField]
    TankScript tankController;
    //[SerializeField]
    //ObstacleCheckScript obsCheckScript;
    [SerializeField]
    FOVObsCheckScript obsCheckScript;

    Vector2 dirToTarget;
    bool hasReachedTarget;
    bool isTargetAlive;

    public bool isAlive;

    private void Awake()
    {
        isAlive = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateTargetProperties();
        MovementLogic();
        AttackLogic();

    }

    private void CalculateTargetProperties()
    {
        isTargetAlive = !targetBase.isDestroyed;//Check if target base alive
        float distanceToTarget = Vector2.Distance(transform.position, targetBase.transform.position);

        //If reached target
        if (distanceToTarget < targetDistanceTolerance && isTargetAlive)
        {
            TryShoot();
        }
    }

    private void MovementLogic()
    {
        if (targetBase && !obsCheckScript.isObstaclesInRange)
        {
            TryDriveTank();
        }

    }

    private void AttackLogic()
    {
        if (obsCheckScript.isObstaclesInRange)//If obstacles in range
        {
            foreach (var item in obsCheckScript.obstaclesInRange)
            {
                if (item != null)
                {
                    if (item.layer == LayerMask.NameToLayer("Tank"))//If obstacles are enemy tanks
                    {
                        if (IsFacingAnEnemy(item))//If facing the tank Shoot!
                        {
                            Attack();
                        }
                        else
                        {
                            Vector2 dir = (item.transform.position - transform.position).normalized;
                            TryFacingDirection(dir);
                        }
                    
                    }
                
                
                }
            } 
        
        
        }
    }

    private void Attack()
    {
        TryShoot();
    }

    void TryDriveTank()
    {
        DriveTank();
    }

    void TryFacingDirection(Vector2 dir)
    {
        tankController.FaceToDirection(dir);
    }

    void TryShoot()
    {
        tankController.Shoot();
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
            //hostCarScript.Accelerate(false);
            //hostCarScript.Reverse(false);
        }

        return isMoving;
    }

    bool IsFacingAnEnemy(GameObject target)
    {
        Vector2 dirToTarget = (target.transform.position - transform.position).normalized;
        return (Vector2.Angle(transform.up, dirToTarget) < 10);
    }

}
