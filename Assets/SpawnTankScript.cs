using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTankScript : MonoBehaviour
{
    public Texture2D btnTexture;
    public GameObject tankPrefab;
    public Transform frontFaceTransform;

    List<GameObject> spawnedTanks;

    int tanksSpawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnedTanks = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (!btnTexture)
        {
            Debug.LogError("Please assign a texture on the inspector");
            return;
        }

        if (GUI.Button(new Rect(10, 10, 50, 50), btnTexture))
        {
            SpawnATank(); 
        }

        if (GUI.Button(new Rect(10, 70, 50, 30), "Clear"))
        {
            foreach (var item in spawnedTanks)
            {
                Destroy(item);
            }
        }

        GUI.Label(new Rect(10, 50, 100, 20), spawnedTanks.Count.ToString());

    }

    void SpawnATank()
    {
        GameObject tank = Instantiate(tankPrefab);

        Vector3 pos = frontFaceTransform.position + new Vector3(0, Random.Range(-2.0f, 2.0f), 0);
        Quaternion rotation1 = Quaternion.LookRotation(Vector3.forward, frontFaceTransform.up);

        tank.transform.SetPositionAndRotation(pos, rotation1);

        spawnedTanks.Add(tank);

    }

}
