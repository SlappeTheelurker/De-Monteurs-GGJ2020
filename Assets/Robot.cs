using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public HingeJoint hinge;
    public Motherboard motherboard;
    public Transform motherboardPos;


    public void openHatch()
    {
        hinge.useSpring = true;
    }
}
