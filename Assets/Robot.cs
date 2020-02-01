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
        foreach (ChipSocket s in motherboard.sockets)
        {
            if (s.needChip)
            {
                if (s.socketedChip == null)
                {
                    Error += "Missing " + s.type + " Chip\n";
                }
                else
                {
                    if(s.type != s.socketedChip.type)
                    {
                        Error += s.socketedChip.type + "in Incorrect location\n";
                    }
                    if(s.format != s.socketedChip.fortmatType)
                    {
                        Error += s.socketedChip.type + "does not fit\n";
                    }
                }
            }
            else
            {
                if(s.socketedChip != null)
                {
                    Error += s.type + "Chip placed where it doesn't belong\n";
                }
            }

        }

        return Error;
    }
}
