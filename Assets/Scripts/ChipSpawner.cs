using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSpawner : MonoBehaviour
{
    public Transform SpawnPos;
    public GameObject ChipPrefab;
    public float scale;

    public void Start()
    {
        Spawn();
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Chip>())
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        GameObject g = Instantiate(ChipPrefab, SpawnPos.position, SpawnPos.rotation);
        g.transform.localScale = g.transform.localScale * scale;
    }
}
