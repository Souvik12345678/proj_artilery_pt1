using System.Collections;
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


    HealthScript healthScript;
    bool isShooting;

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
    }

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

    void OnTankDestroyed()
    {
        Debug.Log("Tank Destroyed");   
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

}
