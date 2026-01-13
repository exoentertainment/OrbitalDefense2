using System;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] float turnSpeed;

    [SerializeField] private Rigidbody rb;

    private Vector3 targetPos;
    private bool isMoving;

    private void Update()
    {
        CheckDistanceToTarget();
        MoveShip();
        RotateShip();
    }

    void MoveShip()
    {
        if (isMoving)
        {
            //transform.Translate(transform.forward * (moveSpeed * Time.deltaTime));
            transform.position += transform.forward * (moveSpeed * Time.deltaTime);
        }
    }

    void RotateShip()
    {
        if (isMoving)
        {
            Vector3 targetVector = targetPos - transform.position;
            targetVector.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);

            
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
    
    public void SetTargetPos(Vector3 pos)
    {
        targetPos = new Vector3(pos.x, pos.y + 10, pos.z);
        isMoving = true;
    }

    void CheckDistanceToTarget()
    {
        if(Vector3.Distance(transform.position, targetPos) <= 1)
            isMoving = false;
    }
}
