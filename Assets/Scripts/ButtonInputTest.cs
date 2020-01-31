using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Square"))
        {
            Debug.Log("Vierkantje!");
        }

        if (Input.GetButtonDown("X"))
        {
            Debug.Log("Iksje!");
        }

        if (Input.GetButtonDown("Circle"))
        {
            Debug.Log("Rondje!");
        }

        if (Input.GetButtonDown("Triangle"))
        {
            Debug.Log("Driehoekje!");
        }
    }
}
