using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public Motherboard motherboard;
    public Transform motherboardPos;
    public Animator doorAnimController;
    public RobotManager thisManager;
    public Transform OutOfBoundsSpawnpoint;

    private void Start()
    {
        if (OutOfBoundsSpawnpoint == null)
        {
            Debug.LogError("HE PIEMOL, out of bounds niet set");
        }
    }

    public void openHatch()
    {
        thisManager.spawnMotherBoard();
        doorAnimController.SetTrigger("OpenDoor");
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        GetComponent<Rigidbody>().isKinematic = true;
        Invoke("EnableChipPhysics", 0.5f);
    }

    public void EnableChipPhysics()
    {
        foreach (Chip item in motherboard.chips)
        {
            item.rigidBody.isKinematic = false;
            item.rigidBody.constraints = RigidbodyConstraints.None;
        }
    }

    public string CheckChips()
    {
        string Error = "";
        int[] stats = new int[motherboard.targetStats.Length];
        foreach (ChipSocket s in motherboard.sockets)
        {

            if (s.socketedChip == null)
                continue;

            if(s.chipOrientation != s.correctOrientation)
            {
                Error += s.socketedChip.type + " chip is placed wrong";
                continue;
            }

            //Empathy
            if (s.socketedChip.statType == Chip.StatType.Empathy && s.socketedChip.fortmatType == Chip.ChipFormats.Size1)
            {
                stats[(int)Chip.StatType.Empathy] += 1;
                stats[(int)Chip.StatType.Agression] += -1;
                stats[(int)Chip.StatType.Emotional_Security] += 0;
            }
            if (s.socketedChip.statType == Chip.StatType.Empathy && s.socketedChip.fortmatType == Chip.ChipFormats.Size2)
            {
                stats[(int)Chip.StatType.Empathy] += 2;
                stats[(int)Chip.StatType.Agression] += -2;
                stats[(int)Chip.StatType.Emotional_Security] += 1;
            }
            if (s.socketedChip.statType == Chip.StatType.Empathy && s.socketedChip.fortmatType == Chip.ChipFormats.Size3)
            {
                stats[(int)Chip.StatType.Empathy] += 8;
                stats[(int)Chip.StatType.Agression] += -4;
                stats[(int)Chip.StatType.Emotional_Security] += -2;
            }

            //Aggression
            if (s.socketedChip.statType == Chip.StatType.Agression && s.socketedChip.fortmatType == Chip.ChipFormats.Size1)
            {
                stats[(int)Chip.StatType.Empathy] += 0;
                stats[(int)Chip.StatType.Agression] += 2;
                stats[(int)Chip.StatType.Emotional_Security] += 0;
            }
            if (s.socketedChip.statType == Chip.StatType.Agression && s.socketedChip.fortmatType == Chip.ChipFormats.Size2)
            {
                stats[(int)Chip.StatType.Empathy] += -1;
                stats[(int)Chip.StatType.Agression] += 3;
                stats[(int)Chip.StatType.Emotional_Security] += 0;
            }
            if (s.socketedChip.statType == Chip.StatType.Agression && s.socketedChip.fortmatType == Chip.ChipFormats.Size3)
            {
                stats[(int)Chip.StatType.Empathy] += -6;
                stats[(int)Chip.StatType.Agression] += 6;
                stats[(int)Chip.StatType.Emotional_Security] += 0;
            }

            //ES
            if (s.socketedChip.statType == Chip.StatType.Emotional_Security && s.socketedChip.fortmatType == Chip.ChipFormats.Size1)
            {
                stats[(int)Chip.StatType.Empathy] += 0;
                stats[(int)Chip.StatType.Agression] += 0;
                stats[(int)Chip.StatType.Emotional_Security] += 1;
            }
            if (s.socketedChip.statType == Chip.StatType.Emotional_Security && s.socketedChip.fortmatType == Chip.ChipFormats.Size1)
            {
                stats[(int)Chip.StatType.Empathy] += -1;
                stats[(int)Chip.StatType.Agression] += 0;
                stats[(int)Chip.StatType.Emotional_Security] += 2;
            }
            if (s.socketedChip.statType == Chip.StatType.Emotional_Security && s.socketedChip.fortmatType == Chip.ChipFormats.Size1)
            {
                stats[(int)Chip.StatType.Empathy] += -3;
                stats[(int)Chip.StatType.Agression] += 2;
                stats[(int)Chip.StatType.Emotional_Security] += 3;
            }







            //if (s.needChip)
            //{
            //    if (s.socketedChip == null)
            //    {
            //        Error += "Missing " + s.type + " Chip\n";
            //    }
            //    else
            //    {
            //        if(s.type != s.socketedChip.type)
            //        {
            //            Error += s.socketedChip.type + "in Incorrect location\n";
            //        }
            //        if(s.format != s.socketedChip.fortmatType)
            //        {
            //            Error += s.socketedChip.type + "does not fit\n";
            //        }
            //    }
            //}
            //else
            //{
            //    if(s.socketedChip != null)
            //    {
            //        Error += s.type + "Chip placed where it doesn't belong\n";
            //    }
            //}
        }

        for (int i = 0; i < stats.Length; i++)
        {
            Debug.Log("Chip stat: " + stats[i]);
            Debug.Log("Target stat: " + motherboard.targetStats[i]);
            if (stats[i] < motherboard.targetStats[i])
                Error += System.Enum.GetName(typeof(Chip.StatType), i) + " is too low\n";

            if (stats[i] > motherboard.targetStats[i])
                Error += System.Enum.GetName(typeof(Chip.StatType), i) + " is too high\n";
        }

        return Error;
    }
}
