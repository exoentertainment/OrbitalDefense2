using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] ProjectileSO projectileSO;

    #endregion
    
    private void OnCollisionEnter(Collision other)
    {
        //Spawn impact VFX
        //Damage any health component attached to collider such as shield
        Destroy(gameObject);
    }
}
