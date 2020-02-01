using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSocket : MonoBehaviour
{
    public Chip.ChipFormats format;
    public string type;
    public Chip socketedChip;
    public Transform snapPoint;
    public int correctOrientation; // 0-3 = 0=360
    public int chipOrientation; // -1 = none
    public float tolerance = 10f;

    private void OnTriggerEnter(Collider other)
    {
        Chip chip = other.GetComponent<Chip>();
        if (chip != null && socketedChip == null)
        {
            
            chip.claw?.Ungrab();

            float rot = other.transform.rotation.eulerAngles.y;
            bool snap = false;
            if (Mathf.Abs(rot % 90) <= tolerance)
            {
                rot = Mathf.RoundToInt(rot / 90) * 90;
                snap = true;
            }

            if (Mathf.Abs(rot % 90) >= 90 - tolerance)
            {
                rot = Mathf.RoundToInt((rot + tolerance) / 90) * 90;
                snap = true;
            }
            if (snap)
            {
                chipOrientation = Mathf.FloorToInt(rot % 360 / 90);

                socketedChip = chip;
                socketedChip.transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0));
                socketedChip.transform.position = snapPoint.position;
            }



        }
    }

    private void OnTriggerExit(Collider other)
    {
        Chip chip = other.GetComponent<Chip>();
        if (chip != null && socketedChip != null)
        {
            socketedChip = null;
        }
    }
}
