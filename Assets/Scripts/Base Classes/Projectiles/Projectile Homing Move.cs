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

    #endregion
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //randomTimeOffset = Random.Range(0f, 10f);
        randomTimeOffset = 1;
        //startTime = Time.time;
    }

    private void OnEnable()
    {
        startTime = Time.time;
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
            Vector3 direction = (target.transform.position - transform.position);
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
    }
    
    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(projectileSO.lifetime);
        
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
