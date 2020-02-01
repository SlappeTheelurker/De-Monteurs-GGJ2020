﻿using System.Collections;
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

            if(s.socketedChip != null)
            {
                stats[(int)s.socketedChip.statType] += (int) s.socketedChip.fortmatType;
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
                Error += System.Enum.GetName(typeof(Chip.StatType),i) + " is too low\n";

            if (stats[i] > motherboard.targetStats[i])
                Error += System.Enum.GetName(typeof(Chip.StatType), i) + " is too high\n";
        }


        return Error;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Chip>() != null)
        {
            if (other.GetComponent<Chip>().claw != null)
            {
                other.GetComponent<Chip>().claw.Ungrab();
            }

            other.transform.position = OutOfBoundsSpawnpoint.position;
            //other.transform.rotation = OutOfBoundsSpawnpoint.rotation;
        }
    }
}
