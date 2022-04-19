using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIScript_pt1 : MonoBehaviour
{
    public enum STATE { NONE, NO_TARGET, APPROACHING_BASE };
    public STATE currentState;


    private void Update()
    {
        CalculateState();
    }

    void CalculateState()
    {
        
    
    }

}
