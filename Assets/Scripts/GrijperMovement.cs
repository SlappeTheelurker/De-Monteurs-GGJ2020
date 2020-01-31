using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrijperMovement : MonoBehaviour
{
    private bool movementEnabled = true;
    [SerializeField] private float maxMoveSpeed = 1.0f;
    private Rigidbody thisRB;

    // Start is called before the first frame update
    void Start()
    {
        if (thisRB == null) thisRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementEnabled)
        {
            Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            movementInput.Normalize();
            movementInput *= maxMoveSpeed;
            thisRB.MovePosition(thisRB.position + movementInput);
        }
    }
}
