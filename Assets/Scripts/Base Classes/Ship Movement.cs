using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipMovement : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private BaseShipSO shipSO;
    [SerializeField] private GameObject engines;

    #endregion

    private int movementLayerOffset;
    private GameObject targetObject;
    private Vector3 destinationPos;
    private Vector3 targetPos;

    private bool isMovingToDestination;
    private bool isMovingToTarget;
    bool isWaitingForTarget;
    
    private void Start()
    {
        movementLayerOffset = GameObject.FindGameObjectWithTag("Movement Layer").GetComponent<InteractionLayer>()
            .ReturnOffset();
    }

    private void Update()
    {
        if (targetObject != null)
        {
            CheckDistanceToDestination();
            MoveShip();
            RotateShip();
        }
        else
        {
            if (!isWaitingForTarget)
            {
                isWaitingForTarget = true;
                DeactivateEngines();
            }
        }
    }

    //If either a standard move or move to target is set
    void MoveShip()
    {
        if (isMovingToDestination || isMovingToTarget)
        {
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


            transform.rotation =
                Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
        }
        else if (isMovingToTarget)
        {
            Vector3 targetVector = targetPos - transform.position;
            targetVector.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);


            transform.rotation =
                Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
        }
    }

    //Set the destination position for a move order
    public void SetDestinationPos(Vector3 pos)
    {
        destinationPos = new Vector3(pos.x, pos.y + movementLayerOffset, pos.z);
        isMovingToDestination = true;
        isMovingToTarget = false;
    }

    void CheckDistanceToDestination()
    {
        if (isMovingToDestination)
        {
            if (Vector3.Distance(transform.position, destinationPos) <= 1)
            {
                isMovingToDestination = false;
                return;
            }
        }
        else if (isMovingToTarget)
            if (Vector3.Distance(transform.position, targetPos) <= 1)
                SetNewTargetPos();
    }

    //Receive target from ship controller, set new position around target
    public void SetTarget(GameObject target)
    {
        if (target != null)
        {
            isMovingToDestination = false;
            isMovingToTarget = true;
            isWaitingForTarget = false;

            targetObject = target;

            Vector3 randomSpot = (Random.insideUnitSphere * Random.Range(shipSO.minOrbitRadius, shipSO.maxOrbitRadius));
            targetPos = randomSpot + target.transform.position;
            
            ActivateEngines();
        }
    }

    //Assign a new random spot around the target
    void SetNewTargetPos()
    {
        Vector3 randomSpot = (Random.insideUnitSphere * Random.Range(shipSO.minOrbitRadius, shipSO.maxOrbitRadius));
        targetPos = randomSpot + targetObject.transform.position;
    }

    void ActivateEngines()
    {
        engines.SetActive(true);
    }
    
    void DeactivateEngines()
    {
        engines.SetActive(false);
    }

private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shipSO.minOrbitRadius);
    }
}
