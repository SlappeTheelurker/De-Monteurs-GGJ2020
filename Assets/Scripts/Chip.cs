using UnityEngine;
using System.Collections;

public class Chip : GrabbableObject
{
    public ChipFormats fortmatType;
    public string type = "NONE";
    [HideInInspector] public Claw claw = null;

    public StatType statType;


    public enum ChipFormats
    {
        Size1,
        Size2,
        Size3
    }

    public enum StatType
    {
        Agression,
        Empathy,
        Emotional_Security
    }
   
}
