using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ArmyBaseScript : MonoBehaviour
{
    public int baseId;

    [SerializeField]
    bool troopDeployActive;
    [SerializeField]
    float troopDeployPerSec = 1;

    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    HealthScript selfHealth;

    [SerializeField]
    ProgressBarScript progressBar;
    [SerializeField]
    SpriteRenderer armyBaseRenderer;
    [SerializeField]
    Sprite armyBaseDestroyedSp;
    [SerializeField]
    GameObject tankPrefab;
    [SerializeField]
    Transform smokePrefab;
    [SerializeField]
    GameObject whiteFlagPrefab;
    [SerializeField]
    List<GameObject> tanksList;

    public bool isDestroyed;

    private void OnEnable()
    {
        selfHealth.OnHealthDepleted += OnHealthZero;
        
    }

    private void OnDisable()
    {
        selfHealth.OnHealthDepleted -= OnHealthZero;
    }

    private void Awake()
    {
        isDestroyed = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        progressBar.barProgress = selfHealth.currentHP;

        StartCoroutine(nameof(TroopsDeployRoutine));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TroopsDeployRoutine()
    {
        for (; ; )
        {
            if (troopDeployActive)
                DeployTroop();
            yield return new WaitForSeconds(1 / troopDeployPerSec);
        }
    }

    private void DeployTroop()
    {
        GameObject tank = Instantiate(tankPrefab);
        tank.transform.position = spawnPoints[Random.Range(0, 2)].position;
        tanksList.Add(tank);
    }

    public void TakeDamage()
    {
        selfHealth.Decrement(20);
       // Debug.Log(selfHealth.currentHP);
        progressBar.barProgress = selfHealth.currentHP / 100.0f;
    }

    void OnHealthZero()
    {
        if (!isDestroyed)
        {
            armyBaseRenderer.sprite = armyBaseDestroyedSp;
            Instantiate(smokePrefab, transform.position, Quaternion.identity);

            Vector3 pos = transform.position + new Vector3(7,0);
            Instantiate(whiteFlagPrefab);

            isDestroyed = true;
        }
    }

}
