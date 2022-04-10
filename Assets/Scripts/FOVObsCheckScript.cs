using System;
using UnityEngine;

public class FOVObsCheckScript : MonoBehaviour
{

    public float radius;
    public float angle;
    [SerializeField]
    LayerMask targetMask;

    public bool isObstaclesInRange;

    public GameObject[] obstaclesInRange;

    private void Awake()
    {
        obstaclesInRange = new GameObject[3];
    }

    private void Start()
    {
        InvokeRepeating(nameof(CheckObstacle), 0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void CheckObstacle()
    {
        Array.Clear(obstaclesInRange, 0, obstaclesInRange.Length);

        isObstaclesInRange = false;

        Collider2D[] collArray = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if (collArray.Length != 0)//If targets available
        {
            /*
            for (int i = 0; i < obstaclesInRange.Length && i < collArray.Length; i++)
            {
                obstaclesInRange[i] = collArray[i].gameObject;
            }*/

            int j = 0;
            foreach (var item in collArray)
            {
                var directionToTarget = (item.transform.position - transform.position).normalized;
                if (Vector2.Angle(transform.up, directionToTarget) < angle)//If target in fov
                {
                    obstaclesInRange[j] = item.gameObject;//Add them to obstaclesinrange Array
                    j++;
                    isObstaclesInRange = true;
                }
            }
        }
    }


}
