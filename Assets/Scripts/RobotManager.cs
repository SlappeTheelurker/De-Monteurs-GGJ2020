﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class RobotManager : MonoBehaviour
{
    public GameObject[] robotPrefabs;
    public Robot currentRobot;
    public Transform spawnPoint;
    public HingeJoint futonHinge;
    public GameObject[] motherboardPrefabs;
    public Animator cameraAnimator;

    public delegate void OnRobotFinished();
    public static OnRobotFinished onRobotFinsihed;
    public RobotOutputUIController RobotOutputUIController;

    public float MaxTime = 300f;
    public float timer = 300f;

    public int correctRobots =0;

    public void Start()
    {
        correctRobots = 0;
        timer = MaxTime;
        SpawnRobot();


    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            //Gameover
            scoreScreen.lastscore = correctRobots;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void SpawnRobot()
    {
        if (currentRobot != null)
            return;
        futonHinge.useSpring = false;
        GameObject ob = Instantiate(robotPrefabs[Random.Range(0, robotPrefabs.Length)],spawnPoint.position,spawnPoint.rotation);
        currentRobot = ob.GetComponent<Robot>() ;

        Rigidbody r = ob.GetComponent<Rigidbody>();
        if (r == null)
            r = ob.AddComponent<Rigidbody>();

        GameObject motherboard = Instantiate(motherboardPrefabs[Random.Range(0, motherboardPrefabs.Length)], currentRobot.motherboardPos.position, currentRobot.motherboardPos.rotation, currentRobot.motherboardPos);
        currentRobot.motherboard = motherboard.GetComponent<Motherboard>();

        r.constraints = RigidbodyConstraints.FreezeAll & ~  RigidbodyConstraints.FreezePositionY;

        Invoke("CameraIn", 1f);
        currentRobot.Invoke("openHatch",1.5f);
    }

    public void RobotDone()
    {
        //check robot
        string Error = currentRobot.CheckChips();
        if ( !string.IsNullOrWhiteSpace(Error))
        {
            RobotOutputUIController.Trigger(Error);
            correctRobots++;
        }



        CameraOut();
        Invoke("LaunchRobot",1f);
        Invoke("SpawnRobot",2f);

    }
    public void LaunchRobot()
    {
        futonHinge.useSpring = true;
        currentRobot.GetComponent<Rigidbody>().isKinematic = false;
        currentRobot.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        currentRobot = null;

    }

    public void CameraIn()
    {
        cameraAnimator.SetBool("in", true);
    }
    public void CameraOut()
    {
        cameraAnimator.SetBool("in", false);
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