using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepDetectorHandler : MonoBehaviour
{
    [SerializeField] float distance = 0.2f;
    [SerializeField] LayerMask whatIsGround;

    RaycastHit hit;

    public bool IsGroundDetected() => Physics.Raycast(transform.position, -transform.up, out hit, distance, whatIsGround);

}
