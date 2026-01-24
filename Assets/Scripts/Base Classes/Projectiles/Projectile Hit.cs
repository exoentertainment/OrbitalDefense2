using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] ProjectileSO projectileSO;

    #endregion
    
    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.TryGetComponent<IHealth>(out IHealth health);
        health?.TakeDamage(1f);
        
        Vector3 collisionNormal = other.contacts[0].normal;
        Quaternion collisionRotation = Quaternion.LookRotation(collisionNormal);
        Instantiate(projectileSO.impactPrefab,  other.contacts[0].point, collisionRotation);

        //Damage any health component attached to collider such as shield
        gameObject.SetActive(false);
    }
}
