using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrijperMovement : MonoBehaviour
{
    //editor variables
    [SerializeField] private bool UseTriggerDownMovement = true;
    [SerializeField] private float maxMoveSpeed = 0.2f;
    [SerializeField] private float maxDownMoveSpeed = 0.2f;
    [SerializeField] private float downMoveGrappleLength = 5.0f;
    [SerializeField] private float maxUpMoveSpeed = 0.2f;

    //calculating variables
    private Vector3 curVelocity = new Vector3();
    private Vector3 newPosition = new Vector3();
    private Vector3 grappleReturnDestination = new Vector3();
    private Vector3 grappleTargetDestination = new Vector3();

    //references to other components
    private Rigidbody thisRB;

    private enum movementState 
    { 
        FreeMovement,
        GrapplingDown,
        ReturningUp
    }
    private movementState thisMovementState;

    void Start()
    {
        if (thisRB == null) thisRB = GetComponent<Rigidbody>();
        thisMovementState = movementState.FreeMovement;
    }

    void Update()
    {
        switch (thisMovementState)
        {
            case movementState.FreeMovement:
                //Do movement
                if (UseTriggerDownMovement)
                {
                    float triggerInput = Input.GetAxisRaw("R2");
                    float triggerInputConverted = (triggerInput + 1.0f) / 2.0f; //ps4 trigger goes from -1.0 -> 1.0    >.<

                    float verticalPosition = triggerInputConverted * -downMoveGrappleLength;
                    Debug.Log("R2: " + triggerInput + ", converted: " + triggerInputConverted);
                    Debug.Log("Vertical pos: " + verticalPosition);
                    thisRB.MovePosition(new Vector3(thisRB.position.x, verticalPosition, thisRB.position.z));
                }

                Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                movementInput.Normalize();
                movementInput *= maxMoveSpeed * Time.deltaTime;
                thisRB.MovePosition(thisRB.position + movementInput);

                //Check grapple button
                if (!UseTriggerDownMovement && Input.GetButtonDown("X"))
                {
                    thisMovementState = movementState.GrapplingDown;
                    grappleReturnDestination = thisRB.position;
                    grappleTargetDestination = thisRB.position + new Vector3(0.0f, -downMoveGrappleLength);
                }
                break;

            case movementState.GrapplingDown:
                //Do movement
                curVelocity = new Vector3(0.0f, -maxDownMoveSpeed);
                newPosition = thisRB.position + (curVelocity * Time.deltaTime);
                if (newPosition.y <= grappleTargetDestination.y)
                {
                    thisRB.MovePosition(grappleTargetDestination);
                    thisMovementState = movementState.ReturningUp;
                }
                else
                {
                    thisRB.MovePosition(newPosition);
                }
                break;

            case movementState.ReturningUp:
                //Do movement
                curVelocity = new Vector3(0.0f, maxUpMoveSpeed);
                newPosition = thisRB.position + (curVelocity * Time.deltaTime);
                if (newPosition.y >= grappleReturnDestination.y)
                {
                    thisRB.MovePosition(grappleReturnDestination);
                    thisMovementState = movementState.FreeMovement;
                }
                else
                {
                    thisRB.MovePosition(newPosition);
                }
                break;

            default:
                Debug.LogError("Grijper MovementState not set correctly!");
                break;
        }
    }
}
