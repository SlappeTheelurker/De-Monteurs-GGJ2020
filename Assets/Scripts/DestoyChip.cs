using UnityEngine;
using System.Collections;

public class DestoyChip : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Chip c = other.GetComponent<Chip>();
        if (c != null)
        {
            if (c.claw)
                c.claw.Ungrab();
            Destroy(c);
        }
    }
}
