using System;
using UnityEngine;
using System.Collections;

public class TurretAttack : MonoBehaviour
{
    #region Serialized Fields

    [Header("Components")] 
    [SerializeField] private Transform raycastOrigin;
    
    [SerializeField] private TurretSO turretSO;
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private Transform[] spawnPoints;

    #endregion
    
    private GameObject target;
    ObjectPool projectilePool;
    float lastFireTime;

    enum WeaponType
    {
        Unguided = 0,
        Guided
    }
    
    private void Awake()
    {
        projectilePool = GetComponent<ObjectPool>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastFireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
            Fire();
    }
    
    public void SetTarget(GameObject newTarget)
    {
        if (newTarget != null)
        {
            target = newTarget;
        }
        else
        {
            target = null;
        }
    }

    void Fire()
    {
        if (IsTargetInLOS())
        {
            if ((Time.time - lastFireTime) > turretSO.fireRate)
            {
                StartCoroutine(FireRoutine());
            }
        }
    }


    protected virtual IEnumerator FireRoutine()
    {
        lastFireTime = Time.time;

        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject projectile = projectilePool.GetPooledObject(); 
            if (projectile != null) {
                projectile.transform.position = spawnPoint.position;
                projectile.transform.rotation = spawnPoint.rotation;
                projectile.SetActive(true);

                if (weaponType == WeaponType.Guided)
                {
                    projectile.GetComponent<ProjectileHomingMove>().SetTarget(target);
                }
            }
            
            // Instantiate(turretSO.projectileSO.projectilePrefab, spawnPoint.position, platformTurret.rotation);
            
            // if (turretSO.projectileSO.dischargePrefab != null)
            //     Instantiate(turretSO.projectileSO.dischargePrefab, spawnPoint.position,
            //         Quaternion.identity);
            
            // Instantiate(turretSO.projectileSO.projectilePrefab,  spawnPoint.position, spawnPoint.rotation);


            if (turretSO.projectileSO.dischargePrefab != null)
            {
                GameObject muzzle = Instantiate(turretSO.projectileSO.dischargePrefab, spawnPoint.position,
                    spawnPoint.rotation);
                Destroy(muzzle, .1f);
            }

            if(AudioManager.instance != null)
                AudioManager.instance.PlaySound(turretSO.fireSFX);
            
            yield return new WaitForSeconds(turretSO.barrelFireDelay);
        }
        

    }
    
    bool IsTargetInLOS()
    {
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * turretSO.engageRange,
                out RaycastHit hit, turretSO.engageRange, turretSO.targetLayers))
        {
            if (hit.collider.gameObject.layer == target.layer)
            {
                return true;
            }
        }
        
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, turretSO.engageRange);
    }
}
