using UnityEngine;
using System.Collections;

public class Chip : GrabbableObject
{
    public Rigidbody rigidbody;
    public ChipFormats fortmatType;
    public string type = "NONE"; 


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public enum ChipFormats
    {
        Size1,
        Size2,
        Size3
    }
   
}
