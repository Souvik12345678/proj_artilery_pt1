﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthScript))]
public class NewTankScript : MonoBehaviour
{
    public float driveSpeed;
    public float rotateSpeed;
    public float muzzRotateSpeed;
    public float shootDelay;
    public float projectileSpeed;
    public bool isDestroyed;

    public Transform tankBodyTransform;
    public Transform muzzleTransform;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    CommonAssetScrObj commonAsset;
    AudioSource audioSrc;

    Vector2 nextPosition;
    float nextRotation;
    HealthScript healthScript;
    bool isShooting;
    Collider2D selfCollider;
    Rigidbody2D rBody;

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
        tankBodyTransform = transform.GetChild(0);
        muzzleTransform = transform.Find("tank_body/muzzle");
        healthScript = GetComponent<HealthScript>();
        audioSrc = GetComponent<AudioSource>();
        selfCollider = GetComponent<BoxCollider2D>();
        rBody = GetComponent<Rigidbody2D>();

        nextPosition = transform.position;
        nextRotation = rBody.rotation;
    }

    private void FixedUpdate()
    {
        rBody.MovePosition(nextPosition);
        rBody.MoveRotation(nextRotation);
    }

    /*
    /// <summary>
    /// forward 1 -> move forward, forward -1 -> movebackward,turn-> Rotate clockwise if -1 and counterclockwise if 1
    /// </summary>
    public void Move(int forward, int turn)
    {
        if (!isDestroyed)
        {
            transform.Translate(forward * driveSpeed * Time.deltaTime * transform.up, Space.World);
            transform.Rotate(0, 0, turn * Time.deltaTime * rotateSpeed);
        }
    }*/

    /// <summary>
    /// forward 1 -> move forward, forward -1 -> movebackward,turn-> Rotate clockwise if -1 and counterclockwise if 1
    /// </summary>
    public void Move(int forward, int turn)
    {
        if (!isDestroyed)
        {
            //rBody.MovePosition(transform.position+ (forward * driveSpeed * transform.up));
            nextPosition = transform.position + (forward *0.01f* driveSpeed * transform.up);
            nextRotation = rBody.rotation + turn * 0.01f * rotateSpeed;
        }
    }

    /// <summary>
    /// turn == 1 clockwise, turn == -1 anticlockwise
    /// </summary>
    public void MuzzleRotate(int turn)
    {
        if (!isDestroyed)
        {
            //float angle = Vector2.SignedAngle(tankBodyTransform.up, muzzleTransform.up);
            //float newAngle = angle + (-turn * Time.deltaTime * muzzRotateSpeed);
            //muzzleTransform.Rotate(0, 0, -turn * Time.deltaTime * muzzRotateSpeed, Space.Self);
            //Debug.Log(angle);

            muzzleTransform.Rotate(0, 0, -turn * Time.deltaTime * muzzRotateSpeed, Space.Self);
            float newAngle = Vector2.SignedAngle(tankBodyTransform.up, muzzleTransform.up);
            if (newAngle > 90)
            {
                muzzleTransform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            if (newAngle < -90)
            {
                muzzleTransform.localRotation = Quaternion.Euler(0, 0, -90);
            }
        }
    }

    /// <summary>
    /// Shoot!
    /// </summary>
    public void Shoot()
    {
        if (!isShooting && !isDestroyed)
        { StartCoroutine(nameof(ShootRoutine)); }
    }

    public HealthScript GetHealthScript()
    { return healthScript; }

    void TakeDamage()
    {
        healthScript.Decrement(25);
    }

    void OnTankDestroyed()
    {
        if (!isDestroyed)
        {
            Instantiate(commonAsset.SmokePrefab, transform.position, Quaternion.identity, transform);
            selfCollider.enabled = false;
            isDestroyed = true;
            StartCoroutine(nameof(DissolveRoutine));
        }
    }

    IEnumerator ShootRoutine()
    {
        isShooting = true;

        //Instantiate projectile 
        GameObject proj = Instantiate(commonAsset.ProjectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = firePoint.up * projectileSpeed;
        Destroy(proj, 3.0f);//Destroy projectile after 3 seconds

        //Instantiate muzzle flash
        Quaternion rot = firePoint.rotation * Quaternion.Euler(new Vector3(0, 0, 90));
        GameObject mzlFlash = Instantiate(commonAsset.MuzzleFlashPrefab, firePoint.position, rot);
        float size = Random.Range(0.6f, 0.9f);
        mzlFlash.transform.localScale = new Vector2(size, size);
        Destroy(mzlFlash, 0.05f);

        //play shoot audio
        audioSrc.Play();
        //wait before shooting again
        yield return new WaitForSeconds(shootDelay + Random.Range(-1f, 1f));

        isShooting = false;

    }

    IEnumerator DissolveRoutine()
    {
        yield return new WaitForSeconds(5);
        float scale = transform.localScale.x;
        while (scale > 0.001f)
        {
            scale *= 0.93f;
            transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
        Destroy(gameObject);//Self destroy 0 secs.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tag_projectile"))
        {
            Destroy(collision.gameObject);//Destroy the projectile
            TakeDamage();
        }
    }

}
