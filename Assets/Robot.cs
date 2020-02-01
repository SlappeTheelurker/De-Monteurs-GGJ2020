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
        GetComponent<Rigidbody>().isKinematic = true;

        Invoke("EnableChipPhysics", 0.5f);
    }

    public void EnableChipPhysics()
    {
        foreach (Chip item in motherboard.chips)
        {
            item.rigidbody.isKinematic = false;
            item.rigidbody.constraints = RigidbodyConstraints.None;
        }
    }
}
