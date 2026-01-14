using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipMovement : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private BaseShipSO shipSO;

    #endregion

    private GameObject targetObject;
    private Vector3 destinationPos;
    private Vector3 targetPos;

    private bool isMovingToDestination;
    private bool isMovingToTarget;

    private void Update()
    {
        CheckDistanceToDestination();
        MoveShip();
        RotateShip();
    }

    void MoveShip()
    {
        if (isMovingToDestination || isMovingToTarget)
        {
            //transform.Translate(transform.forward * (moveSpeed * Time.deltaTime));
            transform.position += transform.forward * (shipSO.moveSpeed * Time.deltaTime);
        }
    }

    void RotateShip()
    {
        if (isMovingToDestination)
        {
            Vector3 targetVector = destinationPos - transform.position;
            targetVector.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);

            
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
        }
        else if (isMovingToTarget)
        {
            Vector3 targetVector = targetPos - transform.position;
            targetVector.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);

            
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
        }
    }
    
    public void SetDestinationPos(Vector3 pos)
    {
        destinationPos = new Vector3(pos.x, pos.y + 10, pos.z);
        isMovingToDestination = true;
        isMovingToTarget = false;
    }

    void CheckDistanceToDestination()
    {
        if(isMovingToDestination)
            if (Vector3.Distance(transform.position, destinationPos) <= 1)
            {
                isMovingToDestination = false;
                return;
            }

        if(isMovingToTarget)
            if (Vector3.Distance(transform.position, targetPos) <= 1)
                SetNewTargetPos();
    }
    
    //Receive target from ship controller, set new position around target
    public void SetTarget(GameObject target)
    {
        isMovingToDestination = false;
        isMovingToTarget = true;
        
        targetObject = target;

        Vector3 randomSpot = (Random.insideUnitSphere * Random.Range(shipSO.minOrbitRadius, shipSO.maxOrbitRadius));
        targetPos = randomSpot + target.transform.position;
    }

    void SetNewTargetPos()
    {
        Vector3 randomSpot = (Random.insideUnitSphere * Random.Range(shipSO.minOrbitRadius, shipSO.maxOrbitRadius));
        targetPos = randomSpot + targetObject.transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shipSO.minOrbitRadius);
    }
}
