using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public GameObject[] robotPrefabs;
    public Transform spawnPoint;
    public void SpawnRobot()
    {
        Instantiate(robotPrefabs[Random.Range(0, robotPrefabs.Length)],spawnPoint.position,Quaternion.identity);
    }

    public void RobotDone()
    {
        Debug.Log("Done");
    }
}
