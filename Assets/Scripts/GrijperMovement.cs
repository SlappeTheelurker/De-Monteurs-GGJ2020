﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrijperMovement : MonoBehaviour
{
    //editor variables
    [SerializeField] private bool UseTriggerDownMovement = true;
    [SerializeField] private bool UseController = true;
    [SerializeField] private float maxMoveSpeed = 0.1f;
    [SerializeField] private float maxDownMoveSpeed = 0.1f;
    [SerializeField] private float downMoveGrappleLength = 3.0f;
    [SerializeField] private float downMoveMaxLength = 3.0f;
    [SerializeField] private float downMoveLengthOffset = 0.2f;
    [SerializeField] private float maxUpMoveSpeed = 0.1f;
    [SerializeField] private float vertMoveEaseLength = 1.0f;
    [SerializeField] private float vertMoveEaseMultiplier = 0.2f;
    [SerializeField] private float maxRotationSpeed = 1.0f;

    //calculating variables
    private Vector3 curVelocity = new Vector3();
    private Vector3 newPosition = new Vector3();
    private Vector3 grappleReturnDestination = new Vector3();
    private Vector3 grappleTargetDestination = new Vector3();
    private float prevTriggerInput = 0.0f;
    private float startHeight = 0.0f;

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
        startHeight = thisRB.position.y;
    }

    void Update()
    {
        switch (thisMovementState)
        {
            case movementState.FreeMovement:
                //Do rotation
                float RSInput = Input.GetAxisRaw("RightStickX");
                if (RSInput != 0.0f)
                {
                    Vector3 eulerAngleVelocity = new Vector3(0.0f, RSInput * maxRotationSpeed, 0.0f);
                    Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
                    thisRB.MoveRotation(thisRB.rotation * deltaRotation);
                }

                //Do movement
                if (UseTriggerDownMovement)
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

                    RaycastHit hit;
                    Vector3 rayStartPoint = new Vector3(thisRB.position.x, startHeight, thisRB.position.z);
                    if (Physics.Raycast(rayStartPoint, -Vector3.up, out hit))
                    {
                        downMoveGrappleLength = hit.distance - downMoveLengthOffset;
                        if (downMoveGrappleLength > downMoveMaxLength)
                        {
                            downMoveGrappleLength = downMoveMaxLength;
                        }
                    }

                    float verticalTargetPos = startHeight - (triggerInputConverted * downMoveGrappleLength);
                    if (goingDown && verticalTargetPos <= startHeight - downMoveGrappleLength + vertMoveEaseLength
                        ||((!goingDown || triggerInputConverted == 0.0f) && verticalTargetPos >= startHeight - vertMoveEaseLength))
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
