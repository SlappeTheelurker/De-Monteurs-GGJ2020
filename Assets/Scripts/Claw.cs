using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //temp

public class Claw : MonoBehaviour
{
    public Transform grabPoint;
    public GrabbableObject grabbedObject;
    public RobotManager robotManager;

    private void OnTriggerEnter(Collider other)
    {
        GameObject g = other.gameObject;
        GrabbableObject obj = g.GetComponent<GrabbableObject>();
        if (obj != null && grabbedObject == null)
        {
            Grab(obj);
        }
        if(g.GetComponent<ReadyButton>() != null)
        {
            robotManager.RobotDone();
        }
    }

    private void Grab(GrabbableObject obj)
    {
        if (!obj.grabbable)
            return;

        
        grabbedObject = obj;
        grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
        grabbedObject.grabbable = false;
        grabbedObject.transform.SetParent(grabPoint);
    }

    public void Ungrab()
    {
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject.grabbable = true;
        grabbedObject.transform.SetParent(null);
        grabbedObject = null;
    }
}




//temp edit script
[CustomEditor(typeof(Claw))]
public class ClawEditorScript: Editor
{
    public override void OnInspectorGUI()
    {
        Claw myTarget = (Claw)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Release"))
        {
            myTarget.Ungrab();
        }
    }
}