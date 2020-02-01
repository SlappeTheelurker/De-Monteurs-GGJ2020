using UnityEngine;
using System.Collections;

public class Motherboard : MonoBehaviour
{
    public ChipSocket[] sockets;
    public Chip[] chips;

    public Robot owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Chip>() != null)
        {
            if (other.GetComponent<Chip>().claw != null)
            {
                other.GetComponent<Chip>().claw.Ungrab();
            }

            other.transform.position = owner.OutOfBoundsSpawnpoint.position;
            //other.transform.rotation = owner.OutOfBoundsSpawnpoint.rotation;
        }
    }
}
