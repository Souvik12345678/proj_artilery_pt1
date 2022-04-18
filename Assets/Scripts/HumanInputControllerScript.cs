using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInputControllerScript : MonoBehaviour
{
    public NewTankScript tankScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            tankScript.Move(1, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            tankScript.Move(-1, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            tankScript.Move(0, -1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            tankScript.Move(0, 1);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            tankScript.MuzzleRotate(-1);

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            tankScript.MuzzleRotate(1);
        }

    }
}
