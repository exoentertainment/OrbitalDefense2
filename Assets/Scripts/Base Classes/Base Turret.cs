using System;
using UnityEngine;

public class BaseTurret : MonoBehaviour
{
    #region Serialized Fields

    

    #endregion

    #region Variables And Properties

    private bool isPrimarySet;
    private bool isSecondarySet;
    private bool needTarget;
    private GameObject primaryTarget;
    private GameObject secondaryTarget;
    private GameObject currentTarget;

    #endregion

    private void Update()
    {
        if(currentTarget == null)
            SearchForTarget();
        
        CheckDistanceToTargets();
    }

    public void SetPrimaryTarget(GameObject target)
    {
        primaryTarget = target;
        currentTarget = primaryTarget;
    }

    void SearchForTarget()
    {
        
    }
    
    void CheckDistanceToTargets()
    {
        if (currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distanceToTarget > 50)
            {
                Debug.Log("Lost target");
                currentTarget = null;
            }
        }

        //If the turret has a primary target set but is not currently targeting it, check distance to primary to see if it's come into range
        if (primaryTarget != null && currentTarget != primaryTarget)
        {
            float distanceToTarget = Vector3.Distance(transform.position, primaryTarget.transform.position);

            if (distanceToTarget < 50)
            {
                Debug.Log("Target in range");
                currentTarget = primaryTarget;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 50);
    }
}
