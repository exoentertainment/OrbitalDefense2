using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseTurret : MonoBehaviour
{
    #region Serialized Fields

    [Header("Components")]
    //[SerializeField] private TurretRotation turretRotation;

    [SerializeField]
    LayerMask targetLayers;

    #endregion

    #region Variables And Properties

    private bool isPrimarySet;
    private bool isSecondarySet;
    private bool needTarget;
    private GameObject primaryTarget;
    private GameObject secondaryTarget;
    private GameObject currentTarget;

    #endregion

    private TurretRotation turretRotation;
    private TurretAttack turretAttack;

    private void Awake()
    {
        turretRotation = GetComponent<TurretRotation>();
        turretAttack = GetComponent<TurretAttack>();
    }

    private void Update()
    {
        if (currentTarget == null)
            SearchForTarget();
        else
        {
            CheckDistanceToTargets();
            CheckLOS();
        }
    }

    //Receive selected target from turret controller and pass it along to the turret's components
    public void SetPrimaryTarget(GameObject target)
    {
        primaryTarget = target;
        currentTarget = primaryTarget;

        turretRotation?.SetTarget(currentTarget);
        turretAttack?.SetTarget(currentTarget);
    }

    void SearchForTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, 25, targetLayers);

        if (possibleTargets.Length > 0)
        {
            float closestEnemy = Mathf.Infinity;

            for (int x = 0; x < possibleTargets.Length; x++)
            {
                float distanceToEnemy =
                    Vector3.Distance(possibleTargets[x].transform.position, transform.position);

                //if (IsLoSClear(possibleTargets[x].gameObject))
                if (distanceToEnemy < closestEnemy)
                {
                    closestEnemy = distanceToEnemy;
                    secondaryTarget = possibleTargets[x].transform.root.gameObject;
                }
            }

            //Set secondary target and pass it along to the turret's components
            currentTarget = secondaryTarget;
            turretRotation.SetTarget(currentTarget);
            turretAttack?.SetTarget(currentTarget);
        }
    }

    void CheckDistanceToTargets()
    {
        if (currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

            //If the distance to current target is beyond the turret's range
            if (distanceToTarget > 25)
            {
                //If the current target is the secondary target then set secondary to null
                if (currentTarget == secondaryTarget)
                    secondaryTarget = null;

                currentTarget = null;
                turretRotation?.SetTarget(null);
                turretAttack?.SetTarget(null);
            }
        }

        //If the turret has a primary target set but is not currently targeting it, check distance to primary to see if it's come into range
        if (primaryTarget != null && currentTarget != primaryTarget)
        {
            float distanceToTarget = Vector3.Distance(transform.position, primaryTarget.transform.position);

            if (distanceToTarget < 25)
                currentTarget = primaryTarget;
        }
    }

    void CheckLOS()
    {
        // if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * turretSO.projectileSO.range,
        //         out RaycastHit hit, turretSO.projectileSO.range, turretSO.targetLayers))
        // {
        // }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 25);
    }
}
