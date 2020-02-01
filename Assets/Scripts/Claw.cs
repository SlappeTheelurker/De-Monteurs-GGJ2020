using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //temp

public class Claw : MonoBehaviour
{
    public Transform grabPoint;
    public GrabbableObject grabbedObject;
    public RobotManager robotManager;
    [SerializeField] private Collider PickUpCollider;

    public bool UseController = true;
    public float timeTillUngrabAllow = 0.2f;
    public float timeTillGrabAllow = 0.3f;
    private float prevTriggerInput = 0.0f;
    private bool grabAllowed = true;
    private bool ungrabbingAllowed = false;

    public bool getUngrabAllowed()
    {
        return ungrabbingAllowed;
    }

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

    private void Update()
    {
        float triggerInput = 0.0f;
        float triggerInputConverted = 0.0f;
        if (UseController)
        {
            triggerInput = Input.GetAxisRaw("R2");
            triggerInputConverted = (triggerInput + 1.0f) / 2.0f; //ps4 trigger goes from -1.0 -> 1.0    >.<
        }
        else
        {
            triggerInput = triggerInputConverted = Input.GetAxis("Down");
        }
        bool goingDown = triggerInput - prevTriggerInput >= 0.0f;
        prevTriggerInput = triggerInput;

        if (grabbedObject != null)
        {
            if (!goingDown)
            {
                Invoke("allowUngrab", timeTillUngrabAllow);
            }
        }
        
        if(!grabAllowed)
        {
            if (!goingDown)
            {
                Invoke("allowGrab", timeTillGrabAllow);
            }
        }

        if (Input.GetButtonDown("Square"))
        {
            Ungrab();
        }
    }

    private void Grab(GrabbableObject obj)
    {
        if (!obj.grabbable || !grabAllowed)
            return;

        Chip chip = obj.GetComponent<Chip>();
        if (chip)
        {
            chip.claw = this;
        }

        grabbedObject = obj;
        grabbedObject.rigidBody.isKinematic = true;
        grabbedObject.grabbable = false;
        grabbedObject.transform.SetParent(grabPoint);
        grabbedObject.transform.localPosition = new Vector3(0.0f, 0.0f);
        ungrabbingAllowed = false;
    }

    private void allowUngrab()
    {
        ungrabbingAllowed = true;
        grabAllowed = false;
    }
    
    private void allowGrab()
    {
        grabAllowed = true;
    }

    public void Ungrab()
    {
        if (grabbedObject == null || !ungrabbingAllowed)
            return;
        ForceUngrab();
    }

    public void ForceUngrab()
    {
        if (grabbedObject == null) { return; }
        grabbedObject.rigidBody.isKinematic = false;
        grabbedObject.grabbable = true;
        grabbedObject.transform.SetParent(null);
        grabbedObject = null;
    }

    public void SetColliderActive(bool active)
    {
        PickUpCollider.enabled = active;
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