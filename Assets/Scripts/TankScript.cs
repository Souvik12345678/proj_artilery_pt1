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


    public HealthScript healthScript;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    GameObject muzzleFlPrefab;
    [SerializeField]
    GameObject projectilePrefab;
    [SerializeField]
    AudioSource audioSrc;
    [SerializeField]
    GameObject smokePrefab;

    bool targetAvailable;
    Vector2 targetDirection;

    bool isShooting;
    bool isDestroyed;

    private void OnEnable()
    {
        healthScript.OnHealthDepleted += OnTankDestroyed;
    }

    private void OnDisable()
    {
        healthScript.OnHealthDepleted -= OnTankDestroyed;
    }

    private void Awake()
    {
        isDestroyed = false;
    }

    void Start()
    {
        targetDirection = transform.up;//Default target dir to up
    }

    void Update()
    {
        RotateLogic();
    }

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

    void TakeDamage()
    {
        healthScript.Decrement(25);
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

    void OnTankDestroyed()
    {
        if (!isDestroyed)
        {
            Instantiate(smokePrefab, transform.position, Quaternion.identity);
            isDestroyed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tag_projectile"))
        {
            TakeDamage();
        }
    }

}
