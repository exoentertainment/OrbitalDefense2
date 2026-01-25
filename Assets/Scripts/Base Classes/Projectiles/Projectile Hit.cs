using System;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] ProjectileSO projectileSO;
    [SerializeField] private Collider col;

    [Tooltip("Does this projectile have a proximity fuse?")]
    [SerializeField] bool hasProximityFuse;
    [SerializeField] private float proximityRange;
    
    #endregion

    #region Variables

    private Collider collider;

    #endregion

    private void Awake()
    {
        collider  = GetComponent<Collider>();
    }

    private void Update()
    {
        if(hasProximityFuse)
            CheckProximity();
    }

    void CheckProximity()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(collider.bounds.center, proximityRange,
            projectileSO.targetLayers);

        if (possibleTargets.Length > 0)
        {
            possibleTargets[0].gameObject.TryGetComponent<IHealth>(out IHealth health);
            health?.TakeDamage(1f);
            
            gameObject.SetActive(false);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.TryGetComponent<IHealth>(out IHealth health);
        health?.TakeDamage(1f);
        
        Vector3 collisionNormal = other.contacts[0].normal;
        Quaternion collisionRotation = Quaternion.LookRotation(collisionNormal);
        Instantiate(projectileSO.impactPrefab,  other.contacts[0].point, collisionRotation);
        
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (hasProximityFuse)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(col.bounds.center, proximityRange);
        }
    }
}
