using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipMovement : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private BaseShipSO shipSO;
    [SerializeField] private GameObject engines;

    [Tooltip("This is the step size for the raycast that looks for detour routes")] 
    [SerializeField] private int radarStepAmount;

    #endregion

    private int movementLayerOffset;
    private GameObject targetObject;
    private Vector3 destinationPos;
    private Vector3 targetPos;
    private Vector3 detourPos;

    private bool isMovingToDestination;
    private bool isMovingToTarget;
    private bool isMovingToDetour;
    bool isWaitingForTarget;

    private int radarAngleX = -45;
    private int radarAngleY = -45;
    
    private void Start()
    {
        movementLayerOffset = GameObject.FindGameObjectWithTag("Movement Layer").GetComponent<InteractionLayer>()
            .ReturnOffset();
    }

    private void Update()
    {
        if (targetObject != null || isMovingToDestination || isMovingToTarget || isMovingToDetour)
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

    private void FixedUpdate()
    {
        CheckPathAhead();
    }

    //If either a standard move or move to target is set
    void MoveShip()
    {
        transform.position += transform.forward * (shipSO.moveSpeed * Time.deltaTime);
    }

    void RotateShip()
    {
        if (isMovingToDetour)
        {
            Vector3 targetVector = detourPos - transform.position;
            targetVector.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);


            transform.rotation =
                Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
            
            return;
        }
        
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
        ActivateEngines();
    }

    void CheckDistanceToDestination()
    {
        if (isMovingToDetour)
        {
            if (Vector3.Distance(transform.position, detourPos) <= 1)
            {
                isMovingToDetour = false;

                return;
            }
        }
        
        if (isMovingToDestination)
        {
            if (Vector3.Distance(transform.position, destinationPos) <= 1)
            {
                isMovingToDestination = false;
                DeactivateEngines();
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

    void CheckPathAhead()
    {
        Physics.Raycast(transform.position, transform.forward * shipSO.lookAheadDistance, out RaycastHit hitInfo, shipSO.lookAheadDistance, shipSO.obstacleLayerMask);
        
        if(hitInfo.collider != null)
        {
            FindDetour();
        }
    }

    //Scan a 45 degree cone in front of the ship. If it finds a spot that doesn't have an object in the way then it sets the detour position
    void FindDetour()
    {
        bool detourFound  = false;

        while (!detourFound)
        {
            Physics.Raycast(transform.position, Quaternion.Euler(radarAngleX, radarAngleY, 0) * transform.forward, out RaycastHit hitInfo, shipSO.lookAheadDistance, shipSO.obstacleLayerMask);

            if (hitInfo.collider == null)
            {
                detourPos = transform.position + Quaternion.Euler(radarAngleX, radarAngleY, 0) * (transform.forward *
                    shipSO.lookAheadDistance);
                isMovingToDetour = true;

                detourFound = true;
                break;
            }
            
            radarAngleX++;
            if (radarAngleX >= 45)
            {
                radarAngleX = -45;
                radarAngleY++;
                if (radarAngleY >= 45)
                    radarAngleY = -45;
                
                //Should we stop the ship if it reaches the bottom right scan area without finding a detour?
            }
        }
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
        Debug.DrawRay(transform.position, transform.forward * shipSO.lookAheadDistance, Color.blue);
    }
}
