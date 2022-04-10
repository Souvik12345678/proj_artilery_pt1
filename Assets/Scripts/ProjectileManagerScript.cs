using System.Collections.Generic;
using UnityEngine;

public class ProjectileManagerScript : MonoBehaviour
{
    //public KeyValuePair<GameObject, float> ObjectLifeTimePair;

    public struct ObjLifetimeStruct
    { 
        public GameObject gmObject;
        public float lifeTime;
    };

    //Keyval pair stores an object along with its remaining lifetime, as a list
    public static List<ObjLifetimeStruct> projectileList = new List<ObjLifetimeStruct>();


    private void Update()
    {
        //Decrease lifetime of each projectile
        for (int i = 0; i < projectileList.Count; i++)
        {
            var temp = projectileList[i];
            temp.lifeTime -= Time.deltaTime;
            projectileList[i] = temp;
        }

        //Remove Objects with lifetime <=0
        
        for (int i = projectileList.Count-1; i >=0 ; i--)
        {
            var temp = projectileList[i];
            if (temp.lifeTime <= 0)
            {
                projectileList.RemoveAt(i);
                Destroy(temp.gmObject);
            }
        }

    }

}
