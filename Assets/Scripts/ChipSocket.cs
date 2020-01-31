using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSocket : MonoBehaviour
{
    public Chip.ChipTypes type;
    public Chip socketedChip;
    public Transform snapPoint;

    private void OnTriggerEnter(Collider other)
    {
        Chip chip = other.GetComponent<Chip>();
        if(chip != null && socketedChip == null)
        {
            socketedChip = chip;
            socketedChip.transform.rotation = snapPoint.rotation;
            socketedChip.transform.position = snapPoint.position;
        }
    }
}
