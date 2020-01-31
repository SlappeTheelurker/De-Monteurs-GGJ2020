using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrijperMovement : MonoBehaviour
{
    //editor variables
    [SerializeField] private bool UseTriggerDownMovement = true;
    [SerializeField] private float maxMoveSpeed = 1.0f;
    [SerializeField] private float maxDownMoveSpeed = 1.0f;
    [SerializeField] private float downMoveGrappleLength = 3.0f;
    [SerializeField] private float maxUpMoveSpeed = 1.0f;
    [SerializeField] private float vertMoveEaseLength = 1.0f;
    [SerializeField] private float vertMoveEaseMultiplier = 0.2f;

    //calculating variables
    private Vector3 curVelocity = new Vector3();
    private Vector3 newPosition = new Vector3();
    private Vector3 grappleReturnDestination = new Vector3();
    private Vector3 grappleTargetDestination = new Vector3();
    private float prevTriggerInput = 0.0f;

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
                    bool goingDown = triggerInput - prevTriggerInput >= 0.0f;
                    prevTriggerInput = triggerInput;
                    float verticalTargetPos = triggerInputConverted * -downMoveGrappleLength;
                    if (goingDown && verticalTargetPos <= -downMoveGrappleLength + vertMoveEaseLength
                        ||((!goingDown || triggerInputConverted == 0.0f) && verticalTargetPos >= -vertMoveEaseLength))
                    {
                        float vertVel = verticalTargetPos - thisRB.position.y;
                        vertVel *= vertMoveEaseMultiplier;
                        float newPosY = thisRB.position.y + vertVel;
                        thisRB.MovePosition(new Vector3(thisRB.position.x, newPosY, thisRB.position.z));
                    }
                    else
                    {
                        thisRB.MovePosition(new Vector3(thisRB.position.x, verticalTargetPos, thisRB.position.z));
                    }
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
