using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        Drive();
    }

    private void Drive()
    {
        int move = 0;

        if (Input.GetKey(KeyCode.W))
        {
            move = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            move = -1;
        }

        transform.Translate(move * speed * Time.deltaTime * transform.up, Space.World);

        int rotate = 0;//Rotate clockwise if -1 and counterclockwise if 1

        if (Input.GetKey(KeyCode.D))
        {
            rotate = -1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rotate = 1;
        }

        transform.Rotate(0, 0, rotate * Time.deltaTime * rotateSpeed);

    }
}
