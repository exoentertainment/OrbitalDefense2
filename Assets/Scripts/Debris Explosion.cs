using System;
using UnityEngine;

public class DebrisExplosion : MonoBehaviour
{
    [SerializeField] private int lifespan;
    [SerializeField] private float explosionForce;
    [SerializeField] private int explosionRadius;
    
    Rigidbody[] rb;

    private void Awake()
    {
        rb  = GetComponentsInChildren<Rigidbody>(); 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifespan);
        ExpandDebris();
    }

    void ExpandDebris()
    {
        foreach (Rigidbody debris in rb)
        {
            debris.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
    }
}
