using System;
using UnityEngine;
using System.Collections;

public class ProjectileMove : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] ProjectileSO projectileSO;

    #endregion
    
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }
    
    private void OnEnable()
    {
        StartCoroutine(DeactivateRoutine());
    }
    
    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(projectileSO.lifetime);
        
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    void Move()
    {
        rb.MovePosition(rb.position + (transform.forward * (projectileSO.moveSpeed * Time.fixedDeltaTime)));
    }
}
