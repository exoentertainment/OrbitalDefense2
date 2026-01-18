using System;
using UnityEngine;

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

    void Move()
    {
        rb.MovePosition(rb.position + (transform.forward * (projectileSO.moveSpeed * Time.fixedDeltaTime)));
    }
}
