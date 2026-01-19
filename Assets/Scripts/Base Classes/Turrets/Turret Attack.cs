using System;
using UnityEngine;
using System.Collections;

public class TurretAttack : MonoBehaviour
{
    #region Serialized Fields

    [Header("Components")] 
    [SerializeField] private Transform raycastOrigin;
    
    [SerializeField] private TurretSO turretSO;
    [SerializeField] private Transform[] spawnPoints;

    #endregion

    BaseTurret turretBase;
    private GameObject target;
    float lastFireTime;

    private void Awake()
    {
        turretBase = GetComponent<BaseTurret>();
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
            // GameObject projectile = projectilePool.GetPooledObject(); 
            // if (projectile != null) {
            //     projectile.transform.position = spawnPoint.position;
            //     projectile.transform.rotation = platformTurret.rotation;
            //     projectile.SetActive(true);
            // }
            
            // Instantiate(turretSO.projectileSO.projectilePrefab, spawnPoint.position, platformTurret.rotation);
            //
            // if (turretSO.projectileSO.dischargePrefab != null)
            //     Instantiate(turretSO.projectileSO.dischargePrefab, spawnPoint.position,
            //         Quaternion.identity);
            
            Instantiate(turretSO.projectileSO.projectilePrefab,  spawnPoint.position, spawnPoint.rotation);
            
            
            GameObject muzzle = Instantiate(turretSO.projectileSO.dischargePrefab,  spawnPoint.position, spawnPoint.rotation);
            Destroy(muzzle, .1f);
            
            yield return new WaitForSeconds(turretSO.barrelFireDelay);
        }
        
        // if(AudioManager.instance != null)
        //     AudioManager.instance.PlaySound(turretSO.fireSFX);
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
}
