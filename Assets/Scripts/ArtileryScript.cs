using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtileryScript : MonoBehaviour
{
    public float projectileSpeed;
    public float shootDelay;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    FOVObsCheckScript obsCheckScript;
    [SerializeField]
    CommonAssetScrObj commonAsset;
    AudioSource audioSrc;

    public string currentStateName;

    ArtileryStateMachine stateMachine;
    public List<NewTankScript> enemiesInSight;

    bool isShooting;
    bool isDestroyed;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitStateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForEnemies();
        stateMachine.Update();
        currentStateName = stateMachine.GetCurrentStateName();
    }

    void InitStateMachine()
    {
        stateMachine = new ArtileryStateMachine(this);

        stateMachine.Initialize("IDLE");

        stateMachine.Start();
    }

    void CheckForEnemies()
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

    public void Shoot()
    {
        if (!isShooting && !isDestroyed)
        { StartCoroutine(nameof(ShootRoutine)); }
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
