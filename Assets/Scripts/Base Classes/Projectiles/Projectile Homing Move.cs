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
        //targetOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f),  Random.Range(-1f, 1f));
        StartCoroutine(DeactivateRoutine());
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
        else
        {
            //FindClosestTarget();
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
            Random.Range(-targetCollider.bounds.size.x/2, targetCollider.bounds.size.x/2),
            Random.Range(-targetCollider.bounds.size.y/2, targetCollider.bounds.size.y/2),
            Random.Range(-targetCollider.bounds.size.z/2, targetCollider.bounds.size.z/2));
    }
    
    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(projectileSO.lifetime);
        
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
