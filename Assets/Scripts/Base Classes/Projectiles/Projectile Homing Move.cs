using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

public class ProjectileHomingMove : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] ProjectileSO  projectileSO;
    [SerializeField] private float turnSpeed;

    #endregion

    #region Variables

    protected float startTime;
    protected float randomTimeOffset;
    
    private Rigidbody rb;
    private GameObject target;

    private Vector3 targetOffset;
    #endregion
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateRoutine());
    }

    private void Update()
    {
        if(target == null)
            FindClosestTarget();
    }

    private void FixedUpdate()
    {
        Move();
    }
    
    void Move()
    {
        if (target != null)
        {
            Vector3 direction = ((target.transform.position + targetOffset)- transform.position);
            Vector3 rotateDir = Vector3.RotateTowards(transform.forward, direction, Time.deltaTime * turnSpeed, 0.0f);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotateDir), Time.deltaTime * turnSpeed);
            rb.linearVelocity = transform.rotation * Vector3.forward * (projectileSO.moveSpeed * Time.fixedDeltaTime);
        }
    }
    
    public void SetTarget(GameObject potentialTarget)
    {
        target = potentialTarget;
        SetTargetOffset();
    }

    //Grrab target collider and set offset to a random point within collider bounds
    void SetTargetOffset()
    {
        Collider targetCollider = target.GetComponent<Collider>();
        targetOffset = new Vector3(
            Random.Range(-targetCollider.bounds.size.x/3, targetCollider.bounds.size.x/3),
            Random.Range(-targetCollider.bounds.size.y/3, targetCollider.bounds.size.y/3),
            Random.Range(-targetCollider.bounds.size.z/3, targetCollider.bounds.size.z/3));
    }
    
    void FindClosestTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity,
            projectileSO.targetLayers);

        if (possibleTargets.Length > 0)
        {
            float closestEnemy = Mathf.Infinity;

            for (int x = 0; x < possibleTargets.Length; x++)
            {
                float distanceToEnemy =
                    Vector3.Distance(possibleTargets[x].transform.position, transform.position);

                if (distanceToEnemy < closestEnemy)
                {
                    closestEnemy = distanceToEnemy;
                    target = possibleTargets[x].gameObject;
                }
            }
        }
    }
    
    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(projectileSO.lifetime);
        
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
