using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Spawn), 1, 0.25f);
    }

    void Spawn()
    {
        GameObject gm = Instantiate(prefab);
        gm.transform.position = new Vector2(Random.Range(-0.001f, 0.001f), Random.Range(-0.001f, 0.001f));
       
    
    }
   
}
