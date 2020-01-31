using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    public RobotManager robotManager;


    private void OnCollisionEnter(Collision collision)
    {
        GameObject g = collision.gameObject;
        if (g.GetComponent<Claw>() != null)
        {
            robotManager.RobotDone();
        }
    }
}
