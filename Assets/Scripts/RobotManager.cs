using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RobotManager : MonoBehaviour
{
    public GameObject[] robotPrefabs;
    public GameObject currentRobot;
    public Transform spawnPoint;
    public HingeJoint futonHinge;
    public void SpawnRobot()
    {
        if (currentRobot != null)
            return;
        futonHinge.useSpring = false;
        GameObject ob = Instantiate(robotPrefabs[Random.Range(0, robotPrefabs.Length)],spawnPoint.position,spawnPoint.rotation);
        currentRobot = ob;

        Rigidbody r = currentRobot.GetComponent<Rigidbody>();
        if (r == null)
            r = currentRobot.AddComponent<Rigidbody>();

        r.constraints = RigidbodyConstraints.FreezeAll & ~  RigidbodyConstraints.FreezePositionY;
    }

    public void RobotDone()
    {
        futonHinge.useSpring = true;
        currentRobot.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        currentRobot = null;
        Invoke("SpawnRobot",1f);
    }
}

[CustomEditor(typeof(RobotManager))]
public class RobotManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RobotManager myTarget = (RobotManager)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Spawn"))
        {
            myTarget.SpawnRobot();
        }

        if (GUILayout.Button("Done"))
        {
            myTarget.RobotDone();
        }
    }
}