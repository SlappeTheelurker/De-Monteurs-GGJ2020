using UnityEngine;
using System.Collections;

public class Chip : GrabbableObject
{
    public Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Update()
    {
            
    }

    public enum ChipTypes
    {
        Cube,
        Round
    }
    public ChipTypes type;
   
}
