using System;
using MoreMountains.Tools;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;

public class ShipMovement : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private BaseShipSO shipSO;
    [SerializeField] private GameObject engines;

    [Tooltip("This is the step size for the raycast that looks for detour routes")] 
    [SerializeField] private int radarStepAmount;

    [EnumToggleButtons]
    [Title("Movement Behavior")]
    [HideLabel]
    [SerializeField] MovementBehavior movementBehavior;
    
    [BoxGroup("Orbit Variables")]
    [HideIfGroup("Orbit Variables/movementBehavior", Value = MovementBehavior.MoveAround)]
    [SerializeField] private int orbitDistance;
    [SerializeField] private int orbitSpeed;
    
    [BoxGroup("Move Around Variables")]
    [HideIfGroup("Move Around Variables/movementBehavior", Value = MovementBehavior.OrbitAround)]
    [SerializeField] private int minOrbitRadius;
    [SerializeField] private int maxOrbitRadius;
    
    #endregion

    #region Variables

    private int movementLayerOffset;
    MMAutoRotate autoRotate;
    private GameObject targetObject;
    private Vector3 destinationPos;
    private Vector3 targetPos;
    private Vector3 detourPos;

    private bool isMovingToDestination;
    private bool isMovingToTarget;
    private bool isMovingToDetour;
    bool isWaitingForTarget;
    private bool isOrbitting;

    private int orbitDirection = 1;
    private float orbitStep = .1f;

    private int radarAngleX = -45;
    private int radarAngleY = -45;

    enum MovementBehavior
    {
        MoveAround,
        OrbitAround
    }
    
    #endregion

    private void Awake()
    {
        autoRotate = GetComponent<MMAutoRotate>();
    }

    private void Start()
    {
        movementLayerOffset = GameObject.FindGameObjectWithTag("Movement Layer").GetComponent<InteractionLayer>()
            .ReturnOffset();
    }

    private void Update()
    {
        if (targetObject == null && movementBehavior == MovementBehavior.OrbitAround && autoRotate.isActiveAndEnabled)
        {
            autoRotate.OrbitCenterTransform = null;
            autoRotate.enabled = false;
        }
        
        if (targetObject != null && !isMovingToDetour)
        {
            switch (movementBehavior)
            {
                case MovementBehavior.MoveAround:
                {
                    RotateTowardsTarget();
                    CheckDistanceToTarget();
                    
                    break;
                }

                case MovementBehavior.OrbitAround:
                {
                    if (!isOrbitting)
                    {
                        RotateTowardsTarget();
                        MoveShip();
                        CheckDistanceToOrbitPoint();
                    }
                    else
                    {
                        PointTowardsTarget();
                        ChangeOrbitAxis();
                    }



                    break;
                }
            }

            
            MoveShip();
        }
        else if (isMovingToDestination)
        {
            RotateTowardsDestination();
            CheckDistanceToDestination();
            MoveShip();
        }
        else if (isMovingToDetour)
        {
            RotateTowardsDetour();
            CheckDistanceToDetour();
            MoveShip();
        }
        
        // if (!isWaitingForTarget)
        // {
        //     isWaitingForTarget = true;
        //     DeactivateEngines();
        // }
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

    #region Rotating Methods

    void RotateTowardsDetour()
    {
        Vector3 targetVector = detourPos - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);


        transform.rotation =
            Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
    }

    void RotateTowardsDestination()
    {
        Vector3 targetVector = destinationPos - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);


        transform.rotation =
            Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
    }

    void RotateTowardsTarget()
    {
        if (movementBehavior == MovementBehavior.MoveAround)
        {
            Vector3 targetVector = targetPos - transform.position;
            targetVector.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);


            transform.rotation =
                Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
        }
        else if (movementBehavior == MovementBehavior.OrbitAround)
        {
            Vector3 targetVector = targetObject.transform.position - transform.position;
            targetVector.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);


            transform.rotation =
                Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
        }
    }

    #endregion


    //Set the destination position for a move order
    public void SetDestinationPos(Vector3 pos)
    {
        destinationPos = new Vector3(pos.x, pos.y + movementLayerOffset, pos.z);
        isMovingToDestination = true;
        isMovingToTarget = false;
        ActivateEngines();
    }

    #region Distance Checking

    void CheckDistanceToTarget()
    {
        if (Vector3.Distance(transform.position, targetPos) <= 1)
            SetNewTargetPos();
    }

    void CheckDistanceToDetour()
    {
        if (Vector3.Distance(transform.position, detourPos) <= 1)
        {
            isMovingToDetour = false;
            
            if(targetObject == null)
                isMovingToDestination = true;
        }
    }
    
    void CheckDistanceToDestination()
    {
        if (Vector3.Distance(transform.position, destinationPos) <= 1)
        {
            isMovingToDestination = false;
            DeactivateEngines();
        }
    }

    void CheckDistanceToOrbitPoint()
    {
        if (Vector3.Distance(transform.position, targetObject.transform.position) <= orbitDistance)
        {
            autoRotate.enabled = true;
            autoRotate.OrbitCenterTransform = targetObject.transform;
            autoRotate.OrbitRadius = orbitDistance;
            
            
            isOrbitting = true;
        }
    }
    
    #endregion

    //Slowly adjust the orbit axis over time and switch the Z axis once it reaches (-)3
    void ChangeOrbitAxis()
    {
        autoRotate.OrbitRotationAxis.x += orbitStep * Time.deltaTime;
        autoRotate.OrbitRotationAxis.z += (orbitStep * Time.deltaTime) * orbitDirection;

        if (autoRotate.OrbitRotationAxis.z > 3 || autoRotate.OrbitRotationAxis.z < -3)
            orbitDirection *= -1;
    }
    
    //Keep the nose of the ship pointed towards the target
    void PointTowardsTarget()
    {
        transform.LookAt(targetObject.transform);
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

            Vector3 randomSpot = (Random.insideUnitSphere * Random.Range(minOrbitRadius, maxOrbitRadius));
            targetPos = randomSpot + target.transform.position;
            
            ActivateEngines();
        }
    }

    //Assign a new random spot around the target
    void SetNewTargetPos()
    {
        Vector3 randomSpot = (Random.insideUnitSphere * Random.Range(minOrbitRadius, maxOrbitRadius));
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
                isMovingToDestination = false;
                isMovingToTarget = false;
                isMovingToDetour = true;

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

        switch (movementBehavior)
        {
            case MovementBehavior.MoveAround:
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, minOrbitRadius);
                Gizmos.DrawWireSphere(transform.position, maxOrbitRadius);
                
                break;
            }
            
            case MovementBehavior.OrbitAround:
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, orbitDistance);
                
                break;
            }
        }
    }
}
