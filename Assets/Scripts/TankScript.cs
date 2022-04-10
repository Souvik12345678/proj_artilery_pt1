using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour
{
    public Vector2 direction;
    public bool isActive;
    public float speed;
    public float rotateSpeed;
    public float targetDistanceTolerance;
    public float shootDelay;
    public float projectileSpeed;
    public float rotationSmoothing;

    public ArmyBaseScript targetBase;

    [SerializeField]
    Transform firePoint;
    [SerializeField]
    GameObject muzzleFlPrefab;
    [SerializeField]
    GameObject projectilePrefab;
    [SerializeField]
    AudioSource audioSrc;

    bool targetAvailable;
    Vector2 targetDirection;

    bool isShooting;


    // Start is called before the first frame update
    void Start()
    {
        targetAvailable = (targetBase != null);
    }

    // Update is called once per frame
    void Update()
    {
        // if (isActive)
        // {
        // UpdateDrive();
        //      UpdateDriveAI();
        //    UpdateShoot();
        //  }

        RotateLogic();

    }

    /*
    private void UpdateDrive()
    {
        int move = 0;

        if (Input.GetKey(KeyCode.W))
        {
            move = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            move = -1;
        }

        transform.Translate(move * speed * Time.deltaTime * transform.up, Space.World);

        int rotate = 0;//Rotate clockwise if -1 and counterclockwise if 1

        if (Input.GetKey(KeyCode.D))
        {
            rotate = -1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rotate = 1;
        }

        transform.Rotate(0, 0, rotate * Time.deltaTime * rotateSpeed);

    }

    private void UpdateDriveAI()
    {
        if (targetAvailable)
        {
            AccelerateLogic();
            TurnLogic();
        }
    }

    //-------------------------
    //Update logic functions
    //-------------------------

    bool AccelerateLogic()
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
                Move(1, 0);
            }
            //move backward?
            else
            {
                if (distanceToTarget > 2.5f)
                {
                    //Too far to reverse
                    Move(1, 0);
                }
                else
                {
                    Move(-1, 0);
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

    void TurnLogic()
    {
        //Turn logic
        float angleToDir = Vector2.SignedAngle(transform.up, dirToTarget);

        if (angleToDir < 0 && Mathf.Abs(angleToDir) > 10) { Move(0, -1); }
        else if (angleToDir > 0 && Mathf.Abs(angleToDir) > 10) { Move(0, 1); }
        else { }
    }

    private void UpdateShoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    
    }
    */


    //forward 1 -> move forward, forward -1 -> movebackward,turn-> Rotate clockwise if -1 and counterclockwise if 1
    public void Move(int forward,int turn)
    {
        transform.Translate(forward * speed * Time.deltaTime * transform.up, Space.World);
        transform.Rotate(0, 0, turn * Time.deltaTime * rotateSpeed);
    }

    public void Shoot()
    {
        if (!isShooting)
        { StartCoroutine(nameof(ShootRoutine)); }
    }

    void RotateLogic()
    {
        Quaternion intRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, targetDirection), Time.deltaTime * rotationSmoothing);
        transform.rotation = intRotation;
    }

    public void FaceToDirection(Vector2 dir)//Rotates the y axis towards the desired direction
    {
        targetDirection = dir;
    }

    IEnumerator ShootRoutine()
    {
        isShooting = true;

        //Instantiate projectile 
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = firePoint.up * projectileSpeed;
        Destroy(proj, 3.0f);//Destroy projectile after 3 seconds

        //Instantiate muzzle flash
        Quaternion rot = firePoint.rotation * Quaternion.Euler(new Vector3(0, 0, 90));
        GameObject mzlFlash = Instantiate(muzzleFlPrefab, firePoint.position, rot);
        float size = Random.Range(0.6f, 0.9f);
        mzlFlash.transform.localScale = new Vector2(size, size);
        Destroy(mzlFlash, 0.05f);

        //play shoot audio
        audioSrc.Play();
        //wait before shooting again
        yield return new WaitForSeconds(shootDelay + Random.Range(-1f, 1f));

        isShooting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tag_projectile"))
        {
            Debug.Log("Hit by projectile");
            
        }
    }

}
